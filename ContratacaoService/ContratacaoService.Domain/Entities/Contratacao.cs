using System.ComponentModel.DataAnnotations.Schema;

namespace ContratacaoService.Domain.Entities;

public class Contratacao
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Codigo { get; set; }
    public Guid PropostaId { get; set; }
    public DateTime DataContratacao { get; set; } = DateTime.UtcNow;
}