using PropostaService.Domain.Entities;

namespace PropostaService.Domain.Interfaces
{
    public interface IPropostaRepository
    {
        Task<Guid> CriarAsync(Proposta proposta);
        Task<Proposta?> ObterPorIdAsync(Guid id);
        Task<List<Proposta>> ObterTodasAsync();
        Task AtualizarAsync(Proposta proposta);
    }
}