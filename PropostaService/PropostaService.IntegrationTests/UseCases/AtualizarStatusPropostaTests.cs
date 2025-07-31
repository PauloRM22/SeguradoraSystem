using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using PropostaService.API;
using PropostaService.Application.DTOs;
using PropostaService.Domain.Enums;
using PropostaService.IntegrationTests.Support;
using System.Net;
using System.Net.Http.Json;

namespace PropostaService.IntegrationTests.UseCases
{
    public class AtualizarStatusPropostaTests : IntegrationTestBase
    {
        private const string PROPOSTA_ENDPOINT = "/api/Proposta";

        public AtualizarStatusPropostaTests(WebApplicationFactory<Program> factory) : base(factory) { }
                
        [Fact]
        public async Task Patch_DeveAtualizarStatusParaAprovada()
        {
            // Arrange
            var novaProposta = new NovaPropostaDto
            {
                Cliente = "Cliente Aprovação",
                Valor = 1234.56M
            };
            var postResponse = await Client.PostAsJsonAsync(PROPOSTA_ENDPOINT, novaProposta);
            postResponse.EnsureSuccessStatusCode();

            var content = await postResponse.Content.ReadFromJsonAsync<Dictionary<string, Guid>>();
            var id = content!["id"];

            var patchDto = new AtualizarStatusDto
            {
                Id = id,
                Status = StatusProposta.Aprovada
            };

            // Act
            var patchResponse = await Client.PatchAsJsonAsync(PROPOSTA_ENDPOINT, patchDto);

            // Assert
            patchResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Patch_DeveAtualizarStatusParaRejeitada()
        {
            // Arrange
            var novaProposta = new NovaPropostaDto
            {
                Cliente = "Cliente Rejeição",
                Valor = 4321.00M
            };
            var postResponse = await Client.PostAsJsonAsync(PROPOSTA_ENDPOINT, novaProposta);
            postResponse.EnsureSuccessStatusCode();

            var content = await postResponse.Content.ReadFromJsonAsync<Dictionary<string, Guid>>();
            var id = content!["id"];

            var patchDto = new AtualizarStatusDto
            {
                Id = id,
                Status = StatusProposta.Rejeitada
            };

            // Act
            var patchResponse = await Client.PatchAsJsonAsync(PROPOSTA_ENDPOINT, patchDto);

            // Assert
            patchResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }        
    }
}