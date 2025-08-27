namespace Pme_MCP_Metrum.Application.SourceQuantityNames.Dtos;
public sealed record SourceQuantityNameDto(
    int SourceID,
    string SourceName,
    short QuantityID,
    string QuantityName,
    DateTime? MinTimestampUtc,  
    DateTime? MaxTimestampUtc   
);
