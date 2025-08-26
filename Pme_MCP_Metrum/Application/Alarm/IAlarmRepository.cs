using Pme_MCP_Metrum.Application.Alarms.Dtos;


namespace Pme_MCP_Metrum.Application.Alarms;

public interface IAlarmRepository
{
    Task<IReadOnlyList<Alarm>> ListAsync(ListAlarmsRequest request, CancellationToken ct);
}
