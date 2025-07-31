using System.Text.Json.Serialization;

namespace ContratacaoService.Application.UseCases.RealizarContratacao;

public class RealizarContratacaoRequest
{
    [JsonPropertyName("propostaId")]
    public Guid PropostaId { get; set; }
}