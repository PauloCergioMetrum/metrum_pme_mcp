namespace Pme_MCP_Metrum.Domain.Entities;

public sealed class Device
{
    public int ID { get; init; }
    public string? Name { get; init; }
    public string? Type { get; init; }
    public string? Address { get; init; }
    public string? Site { get; init; }
    public string? SiteStatus { get; init; }
    public string? Enabled { get; init; }
    public string? Protocol { get; init; }
    public string? Description { get; init; }
    public string? SecureConnectionEnabled { get; init; }
    public string? CertificateValidation { get; init; }
}
