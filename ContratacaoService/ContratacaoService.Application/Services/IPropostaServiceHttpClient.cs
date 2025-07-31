using ContratacaoService.Application.DTOs;
using ContratacaoService.Domain.Enums;

namespace ContratacaoService.Application.Services;

public interface IPropostaServiceHttpClient
{
    Task<StatusProposta> ObterStatusPropostaAsync(Guid propostaId);
    Task<PropostaDto?> ObterPorIdAsync(Guid id);
}