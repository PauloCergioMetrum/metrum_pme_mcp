using Dapper;
using Pme_MCP_Metrum.Domain.Entities;
using Pme_MCP_Metrum.Domain.Repositories.PQEvents;
using Pme_MCP_Metrum.Infrastructure.Persistence;

namespace Pme_MCP_Metrum.Infrastructure.Repositories.PQEvents;

public sealed class PQEventRepository : IPQEventsRepository
{
    private readonly SqlConnectionFactory _factory;
    public PQEventRepository(SqlConnectionFactory factory) => _factory = factory;

    // Snapshot da linha da view
    private sealed record Row(
        int EventId,
        int SourceId,
        DateTime DatalogTimestampUtc,
        DateTime? StartTimestampUtc,
        DateTime? EndTimestampUtc,
        string? WorstPhase,
        short? Direction,
        double? WorstPhaseDuration,
        double? WorstPhaseMagnitude,
        double? WorstPhaseSeverity,
        string? Classification,
        bool HasProcessImpact
    );

    // === NOVO: requerido pela interface ===
    public async Task<IReadOnlyList<PQEvent>> ListTopAsync(int top, CancellationToken ct = default)
    {
        if (top <= 0) top = 1000;

        const string sql = """
        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
        SELECT TOP (@top)
               [EventId],
               [SourceId],
               [DatalogTimestampUtc],
               [StartTimestampUtc],
               [EndTimestampUtc],
               [WorstPhase],
               CAST([Direction] AS smallint) AS [Direction],
               CAST([WorstPhaseDuration]  AS float) AS [WorstPhaseDuration],
               CAST([WorstPhaseMagnitude] AS float) AS [WorstPhaseMagnitude],
               CAST([WorstPhaseSeverity]  AS float) AS [WorstPhaseSeverity],
               [Classification],
               [HasProcessImpact]
        FROM [ION_Data].[dbo].[vPQ_Events] WITH (NOLOCK)
        ORDER BY [DatalogTimestampUtc] DESC;
        """;

        using var conn = _factory.Create();
        await conn.OpenAsync(ct);

        var rows = await conn.QueryAsync<Row>(
            new CommandDefinition(sql, new { top }, commandTimeout: 10, cancellationToken: ct));

        return rows.Select(Map).ToList();
    }

    public async Task<IEnumerable<PQEvent>> ListAsync(
        int? top, int? sourceId, string? classification, bool? hasProcessImpact, string? worstPhase,
        short? direction, DateTime? fromUtc, DateTime? toUtc, CancellationToken ct)
    {
        var take = top.GetValueOrDefault();
        if (take <= 0) take = 1000;

        const string sql = """
        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
        SELECT TOP (@top)
               [EventId],
               [SourceId],
               [DatalogTimestampUtc],
               [StartTimestampUtc],
               [EndTimestampUtc],
               [WorstPhase],
               CAST([Direction] AS smallint) AS [Direction],
               CAST([WorstPhaseDuration]  AS float) AS [WorstPhaseDuration],
               CAST([WorstPhaseMagnitude] AS float) AS [WorstPhaseMagnitude],
               CAST([WorstPhaseSeverity]  AS float) AS [WorstPhaseSeverity],
               [Classification],
               [HasProcessImpact]
        FROM [ION_Data].[dbo].[vPQ_Events] WITH (NOLOCK)
        WHERE (@sourceId IS NULL OR [SourceId] = @sourceId)
          AND (@classification IS NULL OR [Classification] = @classification)
          AND (@hasProcessImpact IS NULL OR [HasProcessImpact] = @hasProcessImpact)
          AND (@worstPhase IS NULL OR [WorstPhase] = @worstPhase)
          AND (@direction IS NULL OR [Direction] = @direction)
          AND (@fromUtc IS NULL OR [DatalogTimestampUtc] >= @fromUtc)
          AND (@toUtc   IS NULL OR [DatalogTimestampUtc] <  @toUtc)
        ORDER BY [DatalogTimestampUtc] DESC;
        """;

        using var conn = _factory.Create();
        await conn.OpenAsync(ct);

        var rows = await conn.QueryAsync<Row>(new CommandDefinition(
            sql,
            new { top = take, sourceId, classification, hasProcessImpact, worstPhase, direction, fromUtc, toUtc },
            commandTimeout: 10,
            cancellationToken: ct));

        return rows.Select(Map).ToList();
    }

    public async Task<PQEvent?> GetLatestAsync(int? sourceId, CancellationToken ct)
    {
        const string sql = """
        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
        SELECT TOP (1)
               [EventId],
               [SourceId],
               [DatalogTimestampUtc],
               [StartTimestampUtc],
               [EndTimestampUtc],
               [WorstPhase],
               CAST([Direction] AS smallint) AS [Direction],
               CAST([WorstPhaseDuration]  AS float) AS [WorstPhaseDuration],
               CAST([WorstPhaseMagnitude] AS float) AS [WorstPhaseMagnitude],
               CAST([WorstPhaseSeverity]  AS float) AS [WorstPhaseSeverity],
               [Classification],
               [HasProcessImpact]
        FROM [ION_Data].[dbo].[vPQ_Events] WITH (NOLOCK)
        WHERE (@sourceId IS NULL OR [SourceId] = @sourceId)
        ORDER BY [DatalogTimestampUtc] DESC;
        """;

        using var conn = _factory.Create();
        await conn.OpenAsync(ct);

        var row = await conn.QueryFirstOrDefaultAsync<Row>(new CommandDefinition(
            sql, new { sourceId }, commandTimeout: 10, cancellationToken: ct));

        return row is null ? null : Map(row);
    }

    public async Task<int> CountByDayAsync(DateTime dayUtc, int? sourceId, CancellationToken ct)
    {
        var fromUtc = dayUtc.Date;
        var toUtc = fromUtc.AddDays(1);

        const string sql = """
        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
        SELECT COUNT(*)
        FROM [ION_Data].[dbo].[vPQ_Events] WITH (NOLOCK)
        WHERE [DatalogTimestampUtc] >= @fromUtc
          AND [DatalogTimestampUtc] <  @toUtc
          AND (@sourceId IS NULL OR [SourceId] = @sourceId);
        """;

        using var conn = _factory.Create();
        await conn.OpenAsync(ct);

        return await conn.ExecuteScalarAsync<int>(new CommandDefinition(
            sql, new { fromUtc, toUtc, sourceId }, commandTimeout: 10, cancellationToken: ct));
    }

    private static PQEvent Map(Row r) => new()
    {
        EventId = r.EventId,
        SourceId = r.SourceId,
        DatalogTimestampUtc = r.DatalogTimestampUtc,
        StartTimestampUtc = r.StartTimestampUtc,
        EndTimestampUtc = r.EndTimestampUtc,
        WorstPhase = r.WorstPhase,
        Direction = r.Direction,
        WorstPhaseDuration = r.WorstPhaseDuration,
        WorstPhaseMagnitude = r.WorstPhaseMagnitude,
        WorstPhaseSeverity = r.WorstPhaseSeverity,
        Classification = r.Classification,
        HasProcessImpact = r.HasProcessImpact
    };
}
