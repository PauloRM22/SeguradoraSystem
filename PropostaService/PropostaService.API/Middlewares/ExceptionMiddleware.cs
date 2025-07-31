using PropostaService.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace PropostaService.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Recurso não encontrado");
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new { erro = ex.Message });
                await context.Response.WriteAsync(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro de validação");
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new { erro = ex.Message });
                await context.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new { erro = "Ocorreu um erro inesperado." });
                await context.Response.WriteAsync(result);
            }
        }
    }
}
