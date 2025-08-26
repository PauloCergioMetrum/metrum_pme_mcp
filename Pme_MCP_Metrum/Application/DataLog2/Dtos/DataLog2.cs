namespace Pme_MCP_Metrum.Application.DataLog2.Dtos;

public sealed record DataLog2Dto(
    long ID,
    double? Value,
    int SourceID,
    short QuantityID,
    DateTime TimestampUTC,
    string Type
);
