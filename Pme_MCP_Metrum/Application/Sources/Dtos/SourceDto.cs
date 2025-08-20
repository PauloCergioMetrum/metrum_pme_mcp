namespace Pme_MCP_Metrum.Application.Sources.Dtos;

public sealed record SourceDto(
    int ID,
    string? Name,
    int? NamespaceID,
    int? SourceTypeID,
    int? TimeZoneID,
    string? Description,
    string? Signature,
    string? DisplayName
);
