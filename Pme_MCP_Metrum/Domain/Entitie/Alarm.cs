public sealed class Alarm
{
    public int ID { get; init; }
    public int AlarmDefinitionID { get; init; }
    public int SourceID { get; init; }
    public string? SourceSystemName { get; init; }
    public string? SourceName { get; init; }
    public string? TimeZoneID { get; init; }
    public DateTime StartTimestampUTC { get; init; }
    public DateTime? EndTimestampUTC { get; init; }
    public string Category { get; init; } = default!;
    public byte Priority { get; init; }
    public string RepresentationKey { get; init; } = default!;
    public bool IsActive { get; init; }
    public int? AcknowledgementID { get; init; }
    public int? PQEventID { get; init; }
    public DateTime LastModifiedUTC { get; init; }

    public Alarm(
        int id,
        int alarmDefinitionId,
        int sourceId,
        string? sourceSystemName,
        string? sourceName,
        string? timeZoneID,
        DateTime startTimestampUTC,
        DateTime? endTimestampUTC,
        string category,
        byte priority,
        string representationKey,
        bool isActive,
        int? acknowledgementID,
        int? pqEventID,
        DateTime lastModifiedUTC
    )
    {
        ID = id;
        AlarmDefinitionID = alarmDefinitionId;
        SourceID = sourceId;
        SourceSystemName = sourceSystemName;
        SourceName = sourceName;
        TimeZoneID = timeZoneID;
        StartTimestampUTC = startTimestampUTC;
        EndTimestampUTC = endTimestampUTC;
        Category = category;
        Priority = priority;
        RepresentationKey = representationKey;
        IsActive = isActive;
        AcknowledgementID = acknowledgementID;
        PQEventID = pqEventID;
        LastModifiedUTC = lastModifiedUTC;
    }
}
