using FluentAssertions;
using Moq;
using PropostaService.Application.DTOs;
using PropostaService.Application.UseCases.CriarProposta;
using PropostaService.Domain.Entities;
using PropostaService.Domain.Interfaces;
using Xunit;

namespace PropostaService.Tests.UseCases
{
    public class CriarPropostaUseCaseTests
    {
        [Fact]
        public async Task ExecuteAsync_DeveCriarProposta_QuandoDadosValidos()
        {
            // Arrange
            var dto = new NovaPropostaDto
            {
                Cliente = "Maria da Silva",
                Valor = 1000
            };

            var propostaEsperada = new Proposta(dto.Cliente, dto.Valor);

            var mockRepo = new Mock<IPropostaRepository>();
            mockRepo.Setup(r => r.CriarAsync(It.IsAny<Proposta>()))
                    .ReturnsAsync(propostaEsperada.Id);

            var useCase = new CriarPropostaUseCase(mockRepo.Object);

            // Act
            var resultado = await useCase.ExecuteAsync(dto);

            // Assert
            resultado.Should().Be(propostaEsperada.Id);
        }

        [Fact]
        public async Task QuandoClienteForVazio_DeveLancarArgumentException()
        {
            // Arrange
            var dto = new NovaPropostaDto
            {
                Cliente = "",
                Valor = 1000
            };

            var repositoryMock = new Mock<IPropostaRepository>();
            var useCase = new CriarPropostaUseCase(repositoryMock.Object);

            // Act & Assert
            var excecao = await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(dto));
            excecao.Message.Should().Be("O nome do cliente é obrigatório.");
        }

        [Fact]
        public async Task QuandoValorForZero_DeveLancarArgumentException()
        {
            // Arrange
            var dto = new NovaPropostaDto
            {
                Cliente = "João",
                Valor = 0
            };

            var repositoryMock = new Mock<IPropostaRepository>();
            var useCase = new CriarPropostaUseCase(repositoryMock.Object);

            // Act & Assert
            var excecao = await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(dto));
            excecao.Message.Should().Be("O valor da proposta deve ser maior que zero.");
        }

        [Fact]
        public async Task QuandoValorForNegativo_DeveLancarArgumentException()
        {
            // Arrange
            var dto = new NovaPropostaDto
            {
                Cliente = "Maria",
                Valor = -100
            };

            var repositoryMock = new Mock<IPropostaRepository>();
            var useCase = new CriarPropostaUseCase(repositoryMock.Object);

            // Act & Assert
            var excecao = await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(dto));
            excecao.Message.Should().Be("O valor da proposta deve ser maior que zero.");
        }

        [Fact]
        public async Task QuandoValorTiverMaisDeDuasCasasDecimais_DeveArredondarParaDuasCasas()
        {
            // Arrange
            var dto = new NovaPropostaDto
            {
                Cliente = "Carlos",
                Valor = 12.3456M
            };

            var repositoryMock = new Mock<IPropostaRepository>();
            Proposta? propostaSalva = null;

            repositoryMock
                .Setup(r => r.CriarAsync(It.IsAny<Proposta>()))
                .ReturnsAsync(Guid.NewGuid())
                .Callback<Proposta>(p => propostaSalva = p);

            var useCase = new CriarPropostaUseCase(repositoryMock.Object);

            // Act
            await useCase.ExecuteAsync(dto);

            // Assert
            propostaSalva.Should().NotBeNull();
            propostaSalva!.Valor.Should().Be(12.35M);
        }

    }
}