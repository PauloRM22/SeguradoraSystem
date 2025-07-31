using PropostaService.Domain.Exceptions;
using PropostaService.Domain.Interfaces;

namespace PropostaService.Application.UseCases.ObterPropostaPorId
{
    public class ObterPropostaPorIdUseCase
    {
        private readonly IPropostaRepository _repository;

        public ObterPropostaPorIdUseCase(IPropostaRepository repository)
        {
            _repository = repository;
        }

        public async Task<PropostaResponse> ExecuteAsync(Guid id)
        {
            var proposta = await _repository.ObterPorIdAsync(id);

            if (proposta is null)
                throw new NotFoundException("Proposta não encontrada.");

            return new PropostaResponse
            {
                Id = proposta.Id,
                Codigo = proposta.Codigo,
                Cliente = proposta.Cliente,
                Valor = proposta.Valor,
                Status = proposta.Status.ToString(),
                DataCriacao = proposta.DataCriacao
            };
        }
    }
}
