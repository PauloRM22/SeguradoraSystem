using ContratacaoService.Application.Services;
using ContratacaoService.Application.UseCases.RealizarContratacao;
using ContratacaoService.Domain.Interfaces;
using ContratacaoService.Infrastructure.Data;
using ContratacaoService.Infrastructure.Http;
using ContratacaoService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ContratacaoService.API.Extensions;

public static class ServiceExtensions
{
    public static void AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<RealizarContratacaoUseCase>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IContratacaoRepository, ContratacaoRepository>();
    }

    public static void AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IPropostaServiceHttpClient, PropostaServiceHttpClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["PropostaService:BaseUrl"]!);
        })
        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });
    }

    public static void AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ContratacaoDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
    }
}