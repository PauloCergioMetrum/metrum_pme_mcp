using Pme_MCP_Metrum.Application.Devices.Dtos;

namespace Pme_MCP_Metrum.Application.Devices.UseCases;

public sealed record ListDevicesResponse(IReadOnlyList<DeviceDto> Items);
