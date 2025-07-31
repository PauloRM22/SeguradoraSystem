using FluentAssertions;
using Moq;
using PropostaService.Application.UseCases.ObterPropostaPorId;
using PropostaService.Domain.Entities;
using PropostaService.Domain.Enums;
using PropostaService.Domain.Exceptions;
using PropostaService.Domain.Interfaces;
using Xunit;

namespace PropostaService.Tests.UseCases.ObterPropostaPorId
{
    public class ObterPropostaPorIdUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_DeveRetornarProposta_QuandoIdForValido()
        {
            // Arrange
            var id = Guid.NewGuid();
            var proposta = new Proposta("Carlos Teste", 1500.00m)
            {
                Id = id,
                Status = StatusProposta.EmAnalise,
                DataCriacao = DateTime.UtcNow
            };

            var repositoryMock = new Mock<IPropostaRepository>();
            repositoryMock.Setup(r => r.ObterPorIdAsync(id)).ReturnsAsync(proposta);

            var useCase = new ObterPropostaPorIdUseCase(repositoryMock.Object);

            // Act
            var resultado = await useCase.ExecuteAsync(id);

            // Assert
            resultado.Id.Should().Be(id);
            resultado.Cliente.Should().Be("Carlos Teste");
            resultado.Valor.Should().Be(1500.00m);
            resultado.Status.Should().Be("EmAnalise");
        }

        [Fact]
        public async Task ExecuteAsync_DeveLancarArgumentException_QuandoPropostaNaoForEncontrada()
        {
            // Arrange
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<IPropostaRepository>();
            repositoryMock.Setup(r => r.ObterPorIdAsync(id)).ReturnsAsync((Proposta?)null);

            var useCase = new ObterPropostaPorIdUseCase(repositoryMock.Object);

            // Act
            var acao = async () => await useCase.ExecuteAsync(id);

            // Assert
            var ex = await Assert.ThrowsAsync<NotFoundException>(acao);
            ex.Message.Should().Be("Proposta não encontrada.");
        }
    }
}