namespace Pme_MCP_Metrum.Application.Devices.UseCases;

public sealed record ListDevicesRequest(string? Type, string? SiteStatus, string? Protocol);
