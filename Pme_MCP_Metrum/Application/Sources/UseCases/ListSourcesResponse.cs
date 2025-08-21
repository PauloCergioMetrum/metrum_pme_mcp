using Pme_MCP_Metrum.Application.Sources.Dtos;


namespace Pme_MCP_Metrum.Application.Sources.UseCases;


public sealed record ListSourcesResponse(IReadOnlyList<SourceDto> Items);