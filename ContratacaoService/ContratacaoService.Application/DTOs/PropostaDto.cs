using ContratacaoService.Domain.Enums;

namespace ContratacaoService.Application.DTOs;

public class PropostaDto
{
    public Guid Id { get; set; }
    public int Codigo { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public StatusProposta Status { get; set; }
    public DateTime DataCriacao { get; set; }
}
