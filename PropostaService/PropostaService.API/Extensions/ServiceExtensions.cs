using PropostaService.Application.UseCases.AtualizarStatusProposta;
using PropostaService.Application.UseCases.CriarProposta;
using PropostaService.Application.UseCases.ObterPropostaPorId;
using PropostaService.Application.UseCases.ObterPropostas;
using PropostaService.Domain.Interfaces;
using PropostaService.Infrastructure.Repositories;

namespace PropostaService.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IPropostaRepository, PropostaRepository>();
            return services;
        }

        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<CriarPropostaUseCase>();
            services.AddScoped<ObterPropostasUseCase>();
            services.AddScoped<AtualizarStatusPropostaUseCase>();
            services.AddScoped<ObterPropostaPorIdUseCase>();
            return services;
        }        
    }
}