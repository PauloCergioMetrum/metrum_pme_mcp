namespace Pme_MCP_Metrum.Application.Devices.Dtos;

public sealed record DeviceDto(
    int ID,
    string? Name,
    string? Type,
    string? Address,
    string? Site,
    string? SiteStatus,
    string? Enabled,
    string? Protocol,
    string? Description,
    string? SecureConnectionEnabled,
    string? CertificateValidation
);
