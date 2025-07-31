using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using PropostaService.API;
using PropostaService.Application.DTOs;
using PropostaService.IntegrationTests.Support;
using System.Net;
using System.Net.Http.Json;

namespace PropostaService.IntegrationTests.UseCases;

public class CriarPropostaTests : IntegrationTestBase
{
    public CriarPropostaTests(WebApplicationFactory<Program> factory) : base(factory) { }
    private const string PROPOSTA_ENDPOINT = "/api/Proposta";

    [Fact]
    public async Task Post_DeveCriarProposta_QuandoDadosForemValidos()
    {
        // Arrange
        var dto = new NovaPropostaDto
        {
            Cliente = "João da Silva",
            Valor = 1234.56M
        };

        // Act
        var response = await Client.PostAsJsonAsync(PROPOSTA_ENDPOINT, dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseBody = await response.Content.ReadFromJsonAsync<PropostaResponse>();
        responseBody!.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task Post_DeveRetornarBadRequest_QuandoClienteForVazio()
    {
        var dto = new NovaPropostaDto
        {
            Cliente = "",
            Valor = 1000
        };

        var response = await Client.PostAsJsonAsync(PROPOSTA_ENDPOINT, dto);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        error!.Erro.Should().Be("O nome do cliente é obrigatório.");
    }

    [Fact]
    public async Task Post_DeveRetornarBadRequest_QuandoValorForZero()
    {
        var dto = new NovaPropostaDto
        {
            Cliente = "Maria",
            Valor = 0
        };

        var response = await Client.PostAsJsonAsync(PROPOSTA_ENDPOINT, dto);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("O valor da proposta deve ser maior que zero.");
    }

    [Fact]
    public async Task Post_DeveRetornarBadRequest_QuandoValorForNegativo()
    {
        var dto = new NovaPropostaDto
        {
            Cliente = "João",
            Valor = -500
        };

        var response = await Client.PostAsJsonAsync(PROPOSTA_ENDPOINT, dto);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("O valor da proposta deve ser maior que zero.");
    }

    [Fact]
    public async Task Post_DeveArredondarValorParaDuasCasas_QuandoValorTiverMuitasCasasDecimais()
    {
        // Arrange
        var dto = new NovaPropostaDto
        {
            Cliente = "Carlos",
            Valor = 1234.56789M
        };

        var response = await Client.PostAsJsonAsync(PROPOSTA_ENDPOINT, dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<PropostaResponse>();
        result!.Id.Should().NotBe(Guid.Empty);        
    }
}