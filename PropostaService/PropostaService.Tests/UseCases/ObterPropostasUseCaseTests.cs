using FluentAssertions;
using Moq;
using PropostaService.Application.UseCases.ObterPropostas;
using PropostaService.Domain.Entities;
using PropostaService.Domain.Enums;
using PropostaService.Domain.Interfaces;
using Xunit;

namespace PropostaService.Tests.UseCases
{
    public class ObterPropostasUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_DeveRetornarListaDePropostas()
        {
            // Arrange
            var propostasFakes = new List<Proposta>
            {
                new Proposta("Cliente A", 1000m),
                new Proposta("Cliente B", 2000m)
            };

            var repositoryMock = new Mock<IPropostaRepository>();
            repositoryMock.Setup(r => r.ObterTodasAsync()).ReturnsAsync(propostasFakes);

            var useCase = new ObterPropostasUseCase(repositoryMock.Object);

            // Act
            var result = await useCase.ExecuteAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(p => p.Cliente == "Cliente A");
            result.Should().Contain(p => p.Cliente == "Cliente B");
        }
    }
}