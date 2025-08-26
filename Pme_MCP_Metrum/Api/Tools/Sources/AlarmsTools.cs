using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using Pme_MCP_Metrum.Application.Alarms.Dtos;
using Pme_MCP_Metrum.Application.Devices.UseCases;
using System.ComponentModel;
using System.Reflection.Metadata;

namespace Pme_MCP_Metrum.Api.Tools.Alarms;

[McpServerToolType]
public sealed class AlarmsTools
{
    private readonly ListAlarms _list;
    private readonly ILogger<AlarmsTools> _log;

    public AlarmsTools(ListAlarms list, ILogger<AlarmsTools> log)
    {
        _list = list;
        _log = log;
    }

    [McpServerTool(Name = "alarms_list_v1")]
    public async Task<object> ListAsync(ListAlarmsRequest? args = default, CancellationToken ct = default)
    {
        _log.LogInformation("Iniciando chamada de alarms_list_v1 com: {@args}", args);

        try
        {
            return await _list.Execute(args ?? new(), ct);
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Erro ao executar alarms_list_v1");
            return new { ok = false, error = ex.GetType().Name, message = ex.Message };
        }
    }


}
