using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using PropostaService.API;
using PropostaService.Application.DTOs;
using PropostaService.IntegrationTests.Support;
using System.Net;
using System.Net.Http.Json;

namespace PropostaService.IntegrationTests.UseCases
{
    public class ObterPropostaPorIdTests : IntegrationTestBase
    {
        private const string PROPOSTA_ENDPOINT = "/api/Proposta";

        public ObterPropostaPorIdTests(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task GetPorId_DeveRetornarProposta_QuandoIdForValido()
        {
            // Arrange
            var novaProposta = new NovaPropostaDto
            {
                Cliente = "Maria Oliveira",
                Valor = 4567.89M
            };

            var postResponse = await Client.PostAsJsonAsync(PROPOSTA_ENDPOINT, novaProposta);
            postResponse.EnsureSuccessStatusCode();

            var content = await postResponse.Content.ReadFromJsonAsync<Dictionary<string, Guid>>();
            var id = content!["id"];

            // Act
            var getResponse = await Client.GetAsync($"{PROPOSTA_ENDPOINT}/{id}");

            // Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var proposta = await getResponse.Content.ReadFromJsonAsync<PropostaResponse>();
            proposta!.Id.Should().Be(id);
            proposta.Cliente.Should().Be("Maria Oliveira");
            proposta.Valor.Should().Be(4567.89M);
        }

        [Fact]
        public async Task GetPorId_DeveRetornarNotFound_QuandoIdNaoExistir()
        {
            // Arrange
            var idInexistente = Guid.NewGuid();

            // Act
            var response = await Client.GetAsync($"{PROPOSTA_ENDPOINT}/{idInexistente}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            error!.Erro.Should().Be("Proposta não encontrada.");
        }
    }
}
