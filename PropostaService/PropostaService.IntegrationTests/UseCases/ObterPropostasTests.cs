using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using PropostaService.API;
using PropostaService.Application.DTOs;
using PropostaService.IntegrationTests.Support;
using System.Net;
using System.Net.Http.Json;

namespace PropostaService.IntegrationTests.UseCases
{
    public class ObterPropostasTests : IntegrationTestBase
    {
        public ObterPropostasTests(WebApplicationFactory<Program> factory) : base(factory) { }
        private const string PROPOSTA_ENDPOINT = "/api/Proposta";

        [Fact]
        public async Task Get_DeveRetornarTodasAsPropostas()
        {
            // Arrange
            var novaProposta = new NovaPropostaDto
            {
                Cliente = "Cliente de Teste",
                Valor = 987.65M
            };
            var postResponse = await Client.PostAsJsonAsync(PROPOSTA_ENDPOINT, novaProposta);
            postResponse.EnsureSuccessStatusCode();

            // Act
            var response = await Client.GetAsync(PROPOSTA_ENDPOINT);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var propostas = await response.Content.ReadFromJsonAsync<List<PropostaResponse>>();
            propostas.Should().NotBeNullOrEmpty();
            propostas!.Any(p => p.Cliente == "Cliente de Teste").Should().BeTrue();
        }
    }
}