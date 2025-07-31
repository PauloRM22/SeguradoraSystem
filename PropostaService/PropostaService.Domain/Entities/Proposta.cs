using PropostaService.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropostaService.Domain.Entities
{
    public class Proposta
    {
        [Key]
        public Guid Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Codigo { get; set; }
        public StatusProposta Status { get; set; } = StatusProposta.EmAnalise;

        public string Cliente { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataCriacao { get; set; }

        public Proposta(string cliente, decimal valor)
        {
            Id = Guid.NewGuid();
            Cliente = cliente;
            Valor = valor;
            Status = StatusProposta.EmAnalise;
            DataCriacao = DateTime.UtcNow;
        }
    }
}