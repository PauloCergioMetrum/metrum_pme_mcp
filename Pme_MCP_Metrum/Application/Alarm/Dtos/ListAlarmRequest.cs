namespace Pme_MCP_Metrum.Application.Alarms.Dtos;

public sealed class ListAlarmsRequest
{
    public int? ID { get; init; }
    public int? AlarmDefinitionID { get; init; }
    public int? SourceID { get; init; }
    public string? SourceSystemName { get; init; }
    public string? SourceName { get; init; }
    public string? TimeZoneID { get; init; }
    public DateTime? StartFrom { get; init; }
    public DateTime? StartTo { get; init; }
    public DateTime? EndFrom { get; init; }
    public DateTime? EndTo { get; init; }
    public string? Category { get; init; }
    public byte? Priority { get; init; }
    public string? RepresentationKey { get; init; }
    public bool? IsActive { get; init; }
    public int? AcknowledgementID { get; init; }
    public int? PQEventID { get; init; }
    public DateTime? ModifiedFrom { get; init; }
    public DateTime? ModifiedTo { get; init; }
}
