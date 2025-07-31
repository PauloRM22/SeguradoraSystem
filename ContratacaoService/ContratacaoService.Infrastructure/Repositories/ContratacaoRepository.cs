using ContratacaoService.Domain.Entities;
using ContratacaoService.Domain.Interfaces;
using ContratacaoService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ContratacaoService.Infrastructure.Repositories;

public class ContratacaoRepository : IContratacaoRepository
{
    private readonly ContratacaoDbContext _dbContext;

    public ContratacaoRepository(ContratacaoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CriarAsync(Contratacao contratacao)
    {
        _dbContext.Contratacoes.Add(contratacao);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Contratacao?> ObterPorPropostaIdAsync(Guid propostaId)
    {
        return await _dbContext.Contratacoes
            .FirstOrDefaultAsync(c => c.PropostaId == propostaId);
    }
}