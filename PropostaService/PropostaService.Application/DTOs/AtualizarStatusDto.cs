using PropostaService.Domain.Enums;

namespace PropostaService.Application.DTOs
{
    public class AtualizarStatusDto
    {
        public Guid Id { get; set; }
        public StatusProposta Status { get; set; }
    }
}