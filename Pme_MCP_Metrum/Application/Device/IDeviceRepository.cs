using Pme_MCP_Metrum.Domain.Entities;

namespace Pme_MCP_Metrum.Application.Devices
{
    public interface IDeviceRepository
    {
        Task<IReadOnlyList<Device>> ListAsync(
            string? type,
            string? siteStatus,
            string? protocol,
            CancellationToken ct = default
        );
    }
}
