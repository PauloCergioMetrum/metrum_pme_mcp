namespace Pme_MCP_Metrum.Application.DataLog2.UseCases;

public sealed record ListDataLog2Request(
    int? Top,
    int? SourceId,
    short? QuantityId,
    string? Type,
    DateTime? FromUtc,
    DateTime? ToUtc
);
