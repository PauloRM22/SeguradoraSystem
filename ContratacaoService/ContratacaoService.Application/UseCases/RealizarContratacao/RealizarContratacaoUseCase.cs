using ContratacaoService.Application.DTOs;
using ContratacaoService.Application.Services;
using ContratacaoService.Domain.Entities;
using ContratacaoService.Domain.Enums;
using ContratacaoService.Domain.Interfaces;

namespace ContratacaoService.Application.UseCases.RealizarContratacao
{
    public class RealizarContratacaoUseCase
    {
        private readonly IPropostaServiceHttpClient _propostaClient;
        private readonly IContratacaoRepository _repository;

        public RealizarContratacaoUseCase(
            IPropostaServiceHttpClient propostaClient,
            IContratacaoRepository repository)
        {
            _propostaClient = propostaClient;
            _repository = repository;
        }

        public async Task ExecuteAsync(RealizarContratacaoRequest request)
        {
            await ValidarPropostaParaContratacaoAsync(request.PropostaId);

            var contratacao = new Contratacao
            {
                PropostaId = request.PropostaId
            };

            await _repository.CriarAsync(contratacao);
        }

        private async Task ValidarPropostaParaContratacaoAsync(Guid propostaId)
        {
            if (propostaId == Guid.Empty)
                throw new ArgumentException("PropostaId inválido.");

            var proposta = await _propostaClient.ObterPorIdAsync(propostaId);

            if (proposta is null)
                throw new InvalidOperationException("Proposta não encontrada.");

            var contratacaoExistente = await _repository.ObterPorPropostaIdAsync(propostaId);
            if (contratacaoExistente is not null)
            {
                throw new InvalidOperationException(
                    $"A proposta código {proposta.Codigo} já foi contratada na contratação código {contratacaoExistente.Codigo}. " +
                    $"Por favor, verifique e tente novamente.");
            }

            if (proposta.Status != StatusProposta.Aprovada)
                throw new InvalidOperationException("A proposta não está aprovada.");
        }
    }

}