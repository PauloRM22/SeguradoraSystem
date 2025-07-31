using Microsoft.AspNetCore.Mvc;
using PropostaService.Application.DTOs;
using PropostaService.Application.UseCases.AtualizarStatusProposta;
using PropostaService.Application.UseCases.CriarProposta;
using PropostaService.Application.UseCases.ObterPropostaPorId;
using PropostaService.Application.UseCases.ObterPropostas;
using Swashbuckle.AspNetCore.Annotations;

namespace PropostaService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Propostas")]
    public class PropostaController : ControllerBase
    {
        private readonly CriarPropostaUseCase _useCase;
        private readonly ObterPropostasUseCase _obterUseCase;
        private readonly AtualizarStatusPropostaUseCase _atualizarStatusUseCase;
        private readonly ObterPropostaPorIdUseCase _obterPorIdUseCase;

        public PropostaController(
            CriarPropostaUseCase useCase,
            ObterPropostasUseCase obterUseCase,
            AtualizarStatusPropostaUseCase atualizarStatusUseCase,
            ObterPropostaPorIdUseCase obterPorIdUseCase)
        {
            _useCase = useCase;
            _obterUseCase = obterUseCase;
            _atualizarStatusUseCase = atualizarStatusUseCase;
            _obterPorIdUseCase = obterPorIdUseCase;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Cria uma nova proposta",
            Description = "Registra uma nova proposta informando apenas o nome do cliente e o valor. A data de criação será preenchida automaticamente e o status inicial será 'Em análise'."
        )]
        public async Task<IActionResult> Post([FromBody] NovaPropostaDto dto)
        {
            var id = await _useCase.ExecuteAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todas as propostas",
            Description = "Retorna a lista de todas as propostas cadastradas no sistema."
        )]
        public async Task<IActionResult> Get()
        {
            var propostas = await _obterUseCase.ExecuteAsync();
            return Ok(propostas);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Consulta uma proposta por ID",
            Description = "Retorna os dados de uma proposta específica com base no ID informado."
        )]
        public async Task<IActionResult> GetById(Guid id)
        {
            var proposta = await _obterPorIdUseCase.ExecuteAsync(id);
            if (proposta is null)
                return NotFound();

            return Ok(proposta);
        }

        [HttpPatch]
        [SwaggerOperation(
            Summary = "Atualiza o status da proposta",
            Description = "Permite alterar o status de uma proposta existente para 'Aprovada' ou 'Rejeitada'."
        )]
        public async Task<IActionResult> Patch([FromBody] AtualizarStatusDto dto)
        {
            await _atualizarStatusUseCase.ExecuteAsync(dto);
            return Ok("Status atualizado");
        }
    }
}