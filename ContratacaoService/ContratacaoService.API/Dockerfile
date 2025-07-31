FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiando os .csproj (caminhos relativos ao contexto definido no docker-compose)
COPY ContratacaoService.Application/ContratacaoService.Application.csproj ContratacaoService.Application/
COPY ContratacaoService.Domain/ContratacaoService.Domain.csproj ContratacaoService.Domain/
COPY ContratacaoService.Infrastructure/ContratacaoService.Infrastructure.csproj ContratacaoService.Infrastructure/
COPY ContratacaoService.API/ContratacaoService.API.csproj ContratacaoService.API/

# Restore apenas do projeto principal
RUN dotnet restore ContratacaoService.API/ContratacaoService.API.csproj

# Copia o restante dos arquivos
COPY . .

# Build
WORKDIR /src/ContratacaoService.API
RUN dotnet build "ContratacaoService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish ContratacaoService.API.csproj -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "ContratacaoService.API.dll"]