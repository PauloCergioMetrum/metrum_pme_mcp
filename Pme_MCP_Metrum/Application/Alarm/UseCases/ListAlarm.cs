using Pme_MCP_Metrum.Application.Alarms;
using Pme_MCP_Metrum.Application.Alarms.Dtos;
using Pme_MCP_Metrum.Application.Devices.Dtos;

public sealed class ListAlarms
{
    private readonly IAlarmRepository _repository;

    public ListAlarms(IAlarmRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<AlarmDto>> Execute(ListAlarmsRequest request, CancellationToken ct = default)
    {
        var alarms = await _repository.ListAsync(request, ct);

        return alarms.Select(alarm => new AlarmDto(
            alarm.ID,
            alarm.AlarmDefinitionID,
            alarm.SourceID,
            alarm.SourceSystemName,
            alarm.SourceName,
            alarm.TimeZoneID,
            alarm.StartTimestampUTC,
            alarm.EndTimestampUTC,
            alarm.Category,
            alarm.Priority,
            alarm.RepresentationKey,
            alarm.IsActive,
            alarm.AcknowledgementID,
            alarm.PQEventID,
            alarm.LastModifiedUTC
        )).ToList();
    }
}
