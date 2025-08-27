
using Pme_MCP_Metrum.Application.SourceQuantityNames.Dtos;

namespace Pme_MCP_Metrum.Application.SourceQuantityName.UseCases;

public sealed record ListSourceQuantityNameResponse(
    IReadOnlyList<SourceQuantityNameDto> Items,
    int Total
);
