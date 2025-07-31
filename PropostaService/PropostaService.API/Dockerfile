FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiando os .csproj com caminho relativo ao contexto ./PropostaService
COPY PropostaService.Application/PropostaService.Application.csproj ./PropostaService.Application/
COPY PropostaService.Domain/PropostaService.Domain.csproj ./PropostaService.Domain/
COPY PropostaService.Infrastructure/PropostaService.Infrastructure.csproj ./PropostaService.Infrastructure/
COPY PropostaService.API/PropostaService.API.csproj ./PropostaService.API/

RUN dotnet restore ./PropostaService.API/PropostaService.API.csproj

COPY . .
WORKDIR /src/PropostaService.API
RUN dotnet build "PropostaService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PropostaService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "PropostaService.API.dll"]
