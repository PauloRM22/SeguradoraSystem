using PropostaService.Application.DTOs;
using PropostaService.Domain.Enums;
using PropostaService.Domain.Interfaces;

namespace PropostaService.Application.UseCases.AtualizarStatusProposta
{
    public class AtualizarStatusPropostaUseCase
    {
        private readonly IPropostaRepository _repository;

        public AtualizarStatusPropostaUseCase(IPropostaRepository repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(AtualizarStatusDto dto)
        {
            var proposta = await _repository.ObterPorIdAsync(dto.Id);
            if (proposta is null)
                throw new ArgumentException("Proposta não encontrada.");

            if (!Enum.IsDefined(typeof(StatusProposta), dto.Status))
                throw new ArgumentException("Status inválido.");

            proposta.Status = dto.Status;

            await _repository.AtualizarAsync(proposta);
        }
    }
}
