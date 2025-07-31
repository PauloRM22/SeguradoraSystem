using Xunit;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using ContratacaoService.Application.DTOs;
using ContratacaoService.Application.UseCases;
using ContratacaoService.Application.Services;
using ContratacaoService.Domain.Enums;
using ContratacaoService.Domain.Entities;
using ContratacaoService.Domain.Interfaces;
using ContratacaoService.Application.UseCases.RealizarContratacao;

namespace ContratacaoService.Tests.UseCases.RealizarContratacao;

public class RealizarContratacaoUseCaseTests
{
    private readonly Mock<IPropostaServiceHttpClient> _propostaClientMock;
    private readonly Mock<IContratacaoRepository> _repositoryMock;
    private readonly RealizarContratacaoUseCase _useCase;

    public RealizarContratacaoUseCaseTests()
    {
        _propostaClientMock = new Mock<IPropostaServiceHttpClient>();
        _repositoryMock = new Mock<IContratacaoRepository>();
        _useCase = new RealizarContratacaoUseCase(_propostaClientMock.Object, _repositoryMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRealizarContratacao_QuandoPropostaAprovada()
    {
        // Arrange
        var propostaId = Guid.NewGuid();
        var request = new RealizarContratacaoRequest { PropostaId = propostaId };

        var proposta = new PropostaDto
        {
            Id = propostaId,
            Codigo = 123,
            Status = StatusProposta.Aprovada
        };

        _propostaClientMock.Setup(x => x.ObterPorIdAsync(propostaId))
            .ReturnsAsync(proposta);

        _repositoryMock.Setup(x => x.ObterPorPropostaIdAsync(propostaId))
            .ReturnsAsync((Contratacao?)null);

        _repositoryMock.Setup(x => x.CriarAsync(It.IsAny<Contratacao>()))
            .Returns(Task.CompletedTask);

        // Act
        Func<Task> act = async () => await _useCase.ExecuteAsync(request);

        // Assert
        await act.Should().NotThrowAsync();

        _repositoryMock.Verify(r => r.CriarAsync(It.Is<Contratacao>(
            c => c.PropostaId == propostaId)), Times.Once);
    }


    [Fact]
    public async Task ExecuteAsync_DeveLancarExcecao_QuandoPropostaNaoAprovada()
    {
        // Arrange
        var propostaId = Guid.NewGuid();
        var request = new RealizarContratacaoRequest { PropostaId = propostaId };

        var proposta = new PropostaDto
        {
            Id = propostaId,
            Codigo = 123,
            Status = StatusProposta.EmAnalise // <- Não aprovada
        };

        _propostaClientMock.Setup(x => x.ObterPorIdAsync(propostaId))
            .ReturnsAsync(proposta);

        _repositoryMock.Setup(x => x.ObterPorPropostaIdAsync(propostaId))
            .ReturnsAsync((Contratacao?)null); // Não existe ainda

        // Act
        Func<Task> act = async () => await _useCase.ExecuteAsync(request);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("A proposta não está aprovada.");
    }

    [Fact]
    public async Task ExecuteAsync_DeveLancarExcecao_QuandoPropostaJaFoiContratada()
    {
        // Arrange
        var propostaId = Guid.NewGuid();
        var request = new RealizarContratacaoRequest { PropostaId = propostaId };

        var proposta = new PropostaDto
        {
            Id = propostaId,
            Codigo = 123,
            Status = StatusProposta.Aprovada
        };

        var contratacaoExistente = new Contratacao
        {
            Id = Guid.NewGuid(),
            Codigo = 456,
            PropostaId = propostaId
        };

        _propostaClientMock.Setup(x => x.ObterPorIdAsync(propostaId))
            .ReturnsAsync(proposta);

        _repositoryMock.Setup(x => x.ObterPorPropostaIdAsync(propostaId))
            .ReturnsAsync(contratacaoExistente);

        // Act
        Func<Task> act = async () => await _useCase.ExecuteAsync(request);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"A proposta código {proposta.Codigo} já foi contratada na contratação código {contratacaoExistente.Codigo}*");
    }

    [Fact]
    public async Task ExecuteAsync_DeveLancarExcecao_QuandoPropostaNaoForEncontrada()
    {
        // Arrange
        var propostaId = Guid.NewGuid();
        var request = new RealizarContratacaoRequest { PropostaId = propostaId };

        _propostaClientMock.Setup(x => x.ObterPorIdAsync(propostaId))
            .ReturnsAsync((PropostaDto?)null);

        // Act
        Func<Task> act = async () => await _useCase.ExecuteAsync(request);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Proposta não encontrada.");
    }
}