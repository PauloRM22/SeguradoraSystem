using ContratacaoService.Domain.Entities;

namespace ContratacaoService.Domain.Interfaces;

public interface IContratacaoRepository
{
    Task CriarAsync(Contratacao contratacao);
    Task<Contratacao?> ObterPorPropostaIdAsync(Guid propostaId);
}