using System.Net.Http.Json;
using ContratacaoService.Application.Services;
using ContratacaoService.Application.DTOs;
using ContratacaoService.Domain.Enums;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ContratacaoService.Infrastructure.Http;

public class PropostaServiceHttpClient : IPropostaServiceHttpClient
{
    private readonly HttpClient _httpClient;

    public PropostaServiceHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<StatusProposta> ObterStatusPropostaAsync(Guid propostaId)
    {
        var response = await _httpClient.GetAsync($"/api/Proposta/{propostaId}");

        if (!response.IsSuccessStatusCode)
            throw new Exception("Erro ao consultar proposta.");

        var resultado = await response.Content.ReadFromJsonAsync<PropostaDto>();

        if (resultado is null)
            throw new Exception("Proposta não encontrada.");

        return resultado.Status;
    }

    public async Task<PropostaDto?> ObterPorIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"/api/Proposta/{id}");

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<PropostaDto>(new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        });
    }
}