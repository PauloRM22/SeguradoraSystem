using PropostaService.Application.DTOs;
using PropostaService.Domain.Entities;
using PropostaService.Domain.Enums;
using PropostaService.Domain.Interfaces;

namespace PropostaService.Application.UseCases.CriarProposta
{
    public class CriarPropostaUseCase
    {
        private readonly IPropostaRepository _repository;

        public CriarPropostaUseCase(IPropostaRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> ExecuteAsync(NovaPropostaDto dto)
        {
            Validar(dto);

            dto.Valor = AjustarValorMonetario(dto.Valor);

            var proposta = new Proposta(dto.Cliente, dto.Valor);

            return await _repository.CriarAsync(proposta);
        }

        private void Validar(NovaPropostaDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Cliente))
                throw new ArgumentException("O nome do cliente é obrigatório.");

            if (dto.Valor <= 0)
                throw new ArgumentException("O valor da proposta deve ser maior que zero.");
        }

        private decimal AjustarValorMonetario(decimal valor)
        {
            return Math.Round(valor, 2, MidpointRounding.AwayFromZero);
        }
    }
}
