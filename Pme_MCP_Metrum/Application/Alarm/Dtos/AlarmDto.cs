
namespace Pme_MCP_Metrum.Application.Alarms.Dtos;

public sealed record AlarmDto(
    int ID,
    int AlarmDefinitionID,
    int SourceID,
    string? SourceSystemName,
    string? SourceName,
    string? TimeZoneID,
    DateTime StartTimestampUTC,
    DateTime? EndTimestampUTC,
    string Category,
    byte Priority,
    string RepresentationKey,
    bool IsActive,
    int? AcknowledgementID,
    int? PQEventID,
    DateTime LastModifiedUTC
);
