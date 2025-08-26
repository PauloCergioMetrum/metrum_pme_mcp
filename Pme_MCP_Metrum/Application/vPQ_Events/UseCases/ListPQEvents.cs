using Pme_MCP_Metrum.Application.PQEvents.Dtos;
using Pme_MCP_Metrum.Domain.Repositories;
using Pme_MCP_Metrum.Domain.Repositories.PQEvents;

namespace Pme_MCP_Metrum.Application.PQEvents.UseCases;

public sealed class ListPQEvents(IPQEventsRepository repo)
{
    public async Task<IEnumerable<PQEventDto>> HandleAsync(
        int? top, int? sourceId, string? classification, bool? hasProcessImpact, string? worstPhase,
        short? direction, DateTime? fromUtc, DateTime? toUtc, CancellationToken ct)
    {
        var items = await repo.ListAsync(top, sourceId, classification, hasProcessImpact, worstPhase, direction, fromUtc, toUtc, ct);
        return items.Select(e => new PQEventDto(
            e.EventId, e.SourceId, e.DatalogTimestampUtc, e.StartTimestampUtc, e.EndTimestampUtc,
            e.WorstPhase, e.Direction, e.WorstPhaseDuration, e.WorstPhaseMagnitude, e.WorstPhaseSeverity,
            e.Classification, e.HasProcessImpact
        ));
    }
}
