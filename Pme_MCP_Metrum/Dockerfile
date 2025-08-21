# Usar imagem base do .NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

# SDK para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Pme_MCP_Metrum.csproj", "./"]
RUN dotnet restore "Pme_MCP_Metrum.csproj"

COPY . .
RUN dotnet build "Pme_MCP_Metrum.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pme_MCP_Metrum.csproj" -c Release -o /app/publish

# Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pme_MCP_Metrum.dll"]
