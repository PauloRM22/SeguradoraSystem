using Microsoft.EntityFrameworkCore;
using PropostaService.API.Extensions;
using PropostaService.API.Middlewares;
using PropostaService.Infrastructure.Contexts;
using System.Text.Json.Serialization;

namespace PropostaService.API
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // INJETAR O DbContext com SQL Server e indicar onde está a migration
            builder.Services.AddDbContext<PropostaDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly("PropostaService.Infrastructure")));

            builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
            });

            //Repositorys
            builder.Services.AddRepositories();

            //UseCases
            builder.Services.AddUseCases();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.UseMiddleware<ExceptionMiddleware>();

            app.Run();
        }
    }

}