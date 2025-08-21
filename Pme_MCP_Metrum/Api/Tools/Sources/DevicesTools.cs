using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

using Pme_MCP_Metrum.Application.Devices.UseCases;
using System.ComponentModel;

namespace Pme_MCP_Metrum.Api.Tools.Devices;

[McpServerToolType]
public sealed class DevicesTools
{
    private readonly ListDevices _handler;
    private readonly ILogger<DevicesTools> _log;

    public DevicesTools(ListDevices handler, ILogger<DevicesTools> log)
    {
        _handler = handler;
        _log = log;
    }

    public sealed record DevicesListArgs(string? type = null, string? siteStatus = null, string? protocol = null);

    [McpServerTool(Name = "devices_list_v1")]
    [Description("Lista dispositivos da view vPMCDevice. Pode filtrar por tipo, siteStatus ou protocolo.")]
    public async Task<object> ListAsync(DevicesListArgs? args = default, CancellationToken ct = default)
    {
        try
        {
            _log.LogInformation("Chamada iniciada: devices_list_v1");
            var request = new ListDevicesRequest(args?.type, args?.siteStatus, args?.protocol);
            return await _handler.Execute(request, ct);
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Erro ao executar devices_list_v1: {Message}", ex.Message);
            return new
            {
                ok = false,
                error = ex.GetType().Name,
                message = ex.Message
            };
        }
    }

}
