using Pme_MCP_Metrum.Application.Devices;
using Pme_MCP_Metrum.Application.Devices.Dtos;
using Pme_MCP_Metrum.Application.Devices.UseCases;

namespace Pme_MCP_Metrum.Application.Devices.UseCases 
{
    public sealed class ListDevices
    {
        private readonly IDeviceRepository _deviceRepository;

        public ListDevices(IDeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        public async Task<ListDevicesResponse> Execute(ListDevicesRequest req, CancellationToken ct = default)
        {
            var devices = await _deviceRepository.ListAsync(req.Type, req.SiteStatus, req.Protocol, ct);

            var dtos = devices.Select(d => new DeviceDto(
                d.ID, d.Name, d.Type, d.Address, d.Site, d.SiteStatus,
                d.Enabled, d.Protocol, d.Description,
                d.SecureConnectionEnabled, d.CertificateValidation
            )).ToList();

            return new ListDevicesResponse(dtos);
        }
    }
}
