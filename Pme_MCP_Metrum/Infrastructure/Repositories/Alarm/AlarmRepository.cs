using Dapper;
using Pme_MCP_Metrum.Application.Alarms;
using Pme_MCP_Metrum.Application.Alarms.Dtos;

using Pme_MCP_Metrum.Domain.Entities;
using Pme_MCP_Metrum.Infrastructure.Persistence;

namespace Pme_MCP_Metrum.Infrastructure.Repositories.Alarms;

public sealed class AlarmRepository : IAlarmRepository
{
    private readonly SqlConnectionFactoryNetwork _factory;

    public AlarmRepository(SqlConnectionFactoryNetwork factory)
    {
        _factory = factory;
    }

    public async Task<IReadOnlyList<Alarm>> ListAsync(ListAlarmsRequest request, CancellationToken ct)
    {
        const string sql = """
    SELECT TOP 1000 *
    FROM [ION_Data].[dbo].[vAlarm]
    WHERE (@ID IS NULL OR ID = @ID)
      AND (@AlarmDefinitionID IS NULL OR AlarmDefinitionID = @AlarmDefinitionID)
      AND (@SourceID IS NULL OR SourceID = @SourceID)
      AND (@SourceSystemName IS NULL OR SourceSystemName = @SourceSystemName)
      AND (@SourceName IS NULL OR SourceName = @SourceName)
      AND (@TimeZoneID IS NULL OR TimeZoneID = @TimeZoneID)
      AND (@StartFrom IS NULL OR StartTimestampUTC >= @StartFrom)
      AND (@StartTo IS NULL OR StartTimestampUTC <= @StartTo)
      AND (@EndFrom IS NULL OR EndTimestampUTC >= @EndFrom)
      AND (@EndTo IS NULL OR EndTimestampUTC <= @EndTo)
      AND (@Category IS NULL OR Category = @Category)
      AND (@Priority IS NULL OR Priority = @Priority)
      AND (@RepresentationKey IS NULL OR RepresentationKey = @RepresentationKey)
      AND (@IsActive IS NULL OR IsActive = @IsActive)
      AND (@AcknowledgementID IS NULL OR AcknowledgementID = @AcknowledgementID)
      AND (@PQEventID IS NULL OR PQEventID = @PQEventID)
      AND (@ModifiedFrom IS NULL OR LastModifiedUTC >= @ModifiedFrom)
      AND (@ModifiedTo IS NULL OR LastModifiedUTC <= @ModifiedTo)
    ORDER BY StartTimestampUTC DESC
    """;

        using var conn = _factory.Create();
        await conn.OpenAsync(ct);

        var result = await conn.QueryAsync<Alarm>(sql, request);
        return result.ToList();
    }

}