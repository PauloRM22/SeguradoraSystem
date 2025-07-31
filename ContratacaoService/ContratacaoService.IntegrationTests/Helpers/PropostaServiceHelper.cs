using ContratacaoService.IntegrationTests.Models;
using System.Net.Http.Json;

namespace ContratacaoService.IntegrationTests.Helpers
{
    public class PropostaServiceHelper
    {
        private readonly HttpClient _propostaHttpClient;
        private const string PROPOSTA_SERVICE_BASE_URL = "https://localhost:7213/api/Proposta";

        public PropostaServiceHelper()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            _propostaHttpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://localhost:7213")
            };
        }

        public async Task<Guid> CriarPropostaAprovadaAsync()
        {
            var propostaRequest = new
            {
                cliente = $"Teste Aprovada {Guid.NewGuid()}",
                valor = 1000m
            };

            var response = await _propostaHttpClient.PostAsJsonAsync("/api/Proposta", propostaRequest);
            response.EnsureSuccessStatusCode();

            var propostas = await _propostaHttpClient.GetFromJsonAsync<List<PropostaResponse>>("/api/Proposta");

            if (propostas == null || propostas.Count == 0)
                throw new InvalidOperationException("Nenhuma proposta foi retornada da API.");

            var proposta = propostas.Last(p => p.Cliente == propostaRequest.cliente);

            var patchRequest = new
            {
                id = proposta.Id,
                status = "Aprovada"
            };

            var patchResponse = await _propostaHttpClient.PatchAsJsonAsync("/api/Proposta", patchRequest);
            patchResponse.EnsureSuccessStatusCode();

            return proposta.Id;
        }

        public async Task<Guid> CriarPropostaComStatusAsync(string status)
        {
            var propostaRequest = new
            {
                cliente = "Teste " + status,
                valor = 2000
            };

            var response = await _propostaHttpClient.PostAsJsonAsync("/api/Proposta", propostaRequest);
            response.EnsureSuccessStatusCode();

            var propostas = await _propostaHttpClient.GetFromJsonAsync<List<PropostaResponse>>("/api/Proposta");

            if (propostas == null || propostas.Count == 0)
                throw new InvalidOperationException("Nenhuma proposta foi retornada da API.");

            var proposta = propostas.Last(p => p.Cliente == "Teste " + status);

            var patchRequest = new
            {
                id = proposta.Id,
                status = status
            };

            var patchResponse = await _propostaHttpClient.PatchAsJsonAsync("/api/Proposta", patchRequest);
            patchResponse.EnsureSuccessStatusCode();

            return proposta.Id;
        }
    }
}