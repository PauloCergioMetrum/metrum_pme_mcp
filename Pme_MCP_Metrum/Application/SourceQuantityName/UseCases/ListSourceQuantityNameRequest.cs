namespace Pme_MCP_Metrum.Application.SourceQuantityName.UseCases;

public sealed record ListSourceQuantityNameRequest(
    int? Top,
    int? SourceId,
    short? QuantityId,
    string? SourceNameLike,
    string? QuantityNameLike,
    DateTime? FromUtc,
    DateTime? ToUtc
);
