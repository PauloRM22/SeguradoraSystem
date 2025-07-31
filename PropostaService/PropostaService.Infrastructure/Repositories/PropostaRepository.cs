using Microsoft.EntityFrameworkCore;
using PropostaService.Domain.Entities;
using PropostaService.Domain.Interfaces;
using PropostaService.Infrastructure.Contexts;

namespace PropostaService.Infrastructure.Repositories
{
    public class PropostaRepository : IPropostaRepository
    {
        private readonly PropostaDbContext _context;

        public PropostaRepository(PropostaDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CriarAsync(Proposta proposta)
        {
            _context.Propostas.Add(proposta);
            await _context.SaveChangesAsync();
            return proposta.Id;
        }

        public async Task<Proposta?> ObterPorIdAsync(Guid id)
        {
            return await _context.Propostas.FindAsync(id);
        }

        public async Task<List<Proposta>> ObterTodasAsync()
        {
            return await _context.Propostas.ToListAsync();
        }

        public async Task AtualizarAsync(Proposta proposta)
        {
            var original = await _context.Propostas.FindAsync(proposta.Id);
            if (original is not null)
            {
                original.Status = proposta.Status;
                await _context.SaveChangesAsync();
            }
        }
    }
}