# Base runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# Honra a porta do EasyPanel (PORT) e expõe 8080 por padrão
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080
 
# SDK p/ build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
 
# Copia apenas o csproj primeiro (usa caminho relativo ao CONTEXTO /code)
COPY Pme_MCP_Metrum/Pme_MCP_Metrum.csproj Pme_MCP_Metrum/
RUN dotnet restore "Pme_MCP_Metrum/Pme_MCP_Metrum.csproj"
 
# Agora copia o restante do código da subpasta
COPY Pme_MCP_Metrum/. Pme_MCP_Metrum/
 
# Publica
WORKDIR /src/Pme_MCP_Metrum
RUN dotnet publish -c Release -o /app/publish
 
# Final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Pme_MCP_Metrum.dll"]
