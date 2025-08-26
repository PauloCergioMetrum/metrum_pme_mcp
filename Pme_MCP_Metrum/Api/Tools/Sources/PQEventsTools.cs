using System.ComponentModel;
using ModelContextProtocol.Server;
using Pme_MCP_Metrum.Application.PQEvents.Dtos;
using Pme_MCP_Metrum.Application.PQEvents.UseCases;

namespace Pme_MCP_Metrum.Api.Tools.PQEvents;

[McpServerToolType]
public sealed class PQEventsTools(
    ListPQEvents list,
    GetLatestPQEvent latest,
    CountPQEventsByDay countByDay)
{
    public sealed record ListArgs(
        int? top,
        int? sourceId,
        string? classification,
        bool? hasProcessImpact,
        string? worstPhase,
        short? direction,
        DateTime? fromUtc,
        DateTime? toUtc
    );

    [McpServerTool(Name = "pqevents_list_v1")]
    [Description("Lista eventos de vPQ_Events com filtros (usa DatalogTimestampUtc para from/to).")]
    public Task<IEnumerable<PQEventDto>> ListAsync(ListArgs? a = default, CancellationToken ct = default)
        => list.HandleAsync(a?.top, a?.sourceId, a?.classification, a?.hasProcessImpact, a?.worstPhase, a?.direction, a?.fromUtc, a?.toUtc, ct);

    public sealed record LatestArgs(int? sourceId);

    [McpServerTool(Name = "pqevents_latest_v1")]
    [Description("Retorna o evento mais recente (ORDER BY DatalogTimestampUtc DESC).")]
    public Task<PQEventDto?> LatestAsync(LatestArgs? a = default, CancellationToken ct = default)
        => latest.HandleAsync(a?.sourceId, ct);

    public sealed record CountByDayArgs(DateTime dayUtc, int? sourceId);

    [McpServerTool(Name = "pqevents_count_by_day_v1")]
    [Description("Conta eventos em um dia UTC (intervalo [00:00, 24:00) por DatalogTimestampUtc).")]
    public Task<int> CountByDayAsync(CountByDayArgs args, CancellationToken ct = default)
        => countByDay.HandleAsync(args.dayUtc, args.sourceId, ct);
}
