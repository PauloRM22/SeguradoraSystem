using FluentAssertions;
using Moq;
using PropostaService.Application.DTOs;
using PropostaService.Application.UseCases.AtualizarStatusProposta;
using PropostaService.Domain.Entities;
using PropostaService.Domain.Enums;
using PropostaService.Domain.Interfaces;
using Xunit;

namespace PropostaService.Tests.UseCases
{
    public class AtualizarStatusPropostaUseCaseTests
    {
        [Fact]
        public async Task QuandoPropostaNaoExistir_DeveLancarArgumentException()
        {
            // Arrange
            var dto = new AtualizarStatusDto
            {
                Id = Guid.NewGuid(),
                Status = StatusProposta.Aprovada
            };

            var repositoryMock = new Mock<IPropostaRepository>();
            repositoryMock.Setup(r => r.ObterPorIdAsync(dto.Id))
                          .ReturnsAsync((Proposta?)null);

            var useCase = new AtualizarStatusPropostaUseCase(repositoryMock.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(dto));
            ex.Message.Should().Be("Proposta não encontrada.");
        }

        [Fact]
        public async Task QuandoStatusForInvalido_DeveLancarArgumentException()
        {
            // Arrange
            var proposta = new Proposta("Cliente Teste", 1000);
            var dto = new AtualizarStatusDto
            {
                Id = proposta.Id,
                Status = (StatusProposta)99
            };

            var repositoryMock = new Mock<IPropostaRepository>();
            repositoryMock.Setup(r => r.ObterPorIdAsync(dto.Id)).ReturnsAsync(proposta);

            var useCase = new AtualizarStatusPropostaUseCase(repositoryMock.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(dto));
            ex.Message.Should().Be("Status inválido.");
        }

        [Fact]
        public async Task QuandoStatusForValido_DeveAtualizarProposta()
        {
            // Arrange
            var proposta = new Proposta("Cliente Teste", 1000);
            var dto = new AtualizarStatusDto
            {
                Id = proposta.Id,
                Status = StatusProposta.Rejeitada
            };

            var repositoryMock = new Mock<IPropostaRepository>();
            repositoryMock.Setup(r => r.ObterPorIdAsync(dto.Id)).ReturnsAsync(proposta);
            repositoryMock.Setup(r => r.AtualizarAsync(proposta)).Returns(Task.CompletedTask).Verifiable();

            var useCase = new AtualizarStatusPropostaUseCase(repositoryMock.Object);

            // Act
            await useCase.ExecuteAsync(dto);

            // Assert
            proposta.Status.Should().Be(StatusProposta.Rejeitada);
            repositoryMock.Verify();
        }
    }
}