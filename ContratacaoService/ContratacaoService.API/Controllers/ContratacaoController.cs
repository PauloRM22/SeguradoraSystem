using ContratacaoService.Application.UseCases.RealizarContratacao;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ContratacaoService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Tags("Contratações")]
public class ContratacaoController : ControllerBase
{
    private readonly RealizarContratacaoUseCase _realizarContratacaoUseCase;

    public ContratacaoController(RealizarContratacaoUseCase realizarContratacaoUseCase)
    {
        _realizarContratacaoUseCase = realizarContratacaoUseCase;
    }

    [HttpPost]
    [SwaggerOperation(
            Summary = "Realiza a contratação de uma proposta",
            Description = "Efetua a contratação de uma proposta previamente aprovada. A proposta não pode estar rejeitada ou já contratada. O ID da proposta deve ser informado."
        )]
    public async Task<IActionResult> RealizarContratacao([FromBody] RealizarContratacaoRequest request)
    {
        try
        {
            await _realizarContratacaoUseCase.ExecuteAsync(request);
            return Ok("Proposta contratada com sucesso.");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return UnprocessableEntity(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro interno ao realizar contratação.");
        }
    }
}