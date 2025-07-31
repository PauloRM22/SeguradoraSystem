using PropostaService.Domain.Interfaces;

namespace PropostaService.Application.UseCases.ObterPropostas
{
    public class ObterPropostasUseCase
    {
        private readonly IPropostaRepository _repository;

        public ObterPropostasUseCase(IPropostaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PropostaResponse>> ExecuteAsync()
        {
            var propostas = await _repository.ObterTodasAsync();

            return propostas.Select(p => new PropostaResponse
            {
                Id = p.Id,
                Codigo = p.Codigo,
                Cliente = p.Cliente,
                Valor = p.Valor,
                Status = p.Status.ToString(),
                DataCriacao = p.DataCriacao
            });
        }
    }

}
