using ContratacaoService.API;
using ContratacaoService.Application.UseCases.RealizarContratacao;
using ContratacaoService.IntegrationTests.Base;
using ContratacaoService.IntegrationTests.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using ContratacaoService.IntegrationTests.Helpers;


namespace ContratacaoService.IntegrationTests.Endpoints
{
    public class ContratacaoControllerTests : IntegrationTestBase
    {
        private readonly PropostaServiceHelper _propostaHelper;
        private const string URL_CONTRATACAO = "/api/Contratacao";
        private const string PROPOSTA_SERVICE_BASE_URL = "https://localhost:7213/api/Proposta";

        public ContratacaoControllerTests(WebApplicationFactory<Program> factory) : base(factory)
        {
            _propostaHelper = new PropostaServiceHelper();
        }

        [Fact]
        public async Task RealizarContratacao_DeveRetornar200_QuandoPropostaAprovada()
        {
            // Arrange
            var propostaId = await _propostaHelper.CriarPropostaAprovadaAsync();

            var requestBody = new
            {
                propostaId = propostaId
            };

            // Act
            var response = await HttpClient.PostAsJsonAsync(URL_CONTRATACAO, requestBody);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Be("Proposta contratada com sucesso.");
        }

        [Fact]
        public async Task RealizarContratacao_DeveRetornar422_QuandoPropostaNaoExistir()
        {
            // Arrange
            var request = new RealizarContratacaoRequest
            {
                PropostaId = Guid.NewGuid()
            };

            // Act
            var response = await HttpClient.PostAsJsonAsync(URL_CONTRATACAO, request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Be("Proposta não encontrada.");
        }

        [Fact]
        public async Task RealizarContratacao_DeveRetornar422_QuandoPropostaNaoEstiverAprovada()
        {
            // Arrange
            var propostaId = await _propostaHelper.CriarPropostaComStatusAsync("EmAnalise");

            var request = new RealizarContratacaoRequest
            {
                PropostaId = propostaId
            };

            // Act
            var response = await HttpClient.PostAsJsonAsync(URL_CONTRATACAO, request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Be("A proposta não está aprovada.");
        }

        [Fact]
        public async Task RealizarContratacao_DeveRetornar422_QuandoPropostaJaContratada()
        {
            // Arrange
            var propostaId = await _propostaHelper.CriarPropostaAprovadaAsync();

            var request = new RealizarContratacaoRequest
            {
                PropostaId = propostaId
            };

            // Primeira contratação
            var primeiraResponse = await HttpClient.PostAsJsonAsync(URL_CONTRATACAO, request);
            primeiraResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // Segunda tentativa com a mesma proposta
            var segundaResponse = await HttpClient.PostAsJsonAsync(URL_CONTRATACAO, request);

            // Assert
            segundaResponse.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
            var content = await segundaResponse.Content.ReadAsStringAsync();
            content.Should().StartWith("A proposta código").And.Contain("já foi contratada");
        }               
    }
}