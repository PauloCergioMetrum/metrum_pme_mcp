namespace Pme_MCP_Metrum.Domain.Entities;

public sealed class DataLog2
{
    public long ID { get; init; }
    public double? Value { get; init; }
    public int SourceID { get; init; }
    public short QuantityID { get; init; }
    public DateTime TimestampUTC { get; init; }
    public string Type { get; init; } = default!;
}
