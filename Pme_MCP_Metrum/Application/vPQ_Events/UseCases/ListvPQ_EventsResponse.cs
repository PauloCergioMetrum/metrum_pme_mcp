using Pme_MCP_Metrum.Application.PQEvents.Dtos;
using Pme_MCP_Metrum.Domain.Repositories;
using Pme_MCP_Metrum.Domain.Repositories.PQEvents;

namespace Pme_MCP_Metrum.Application.PQEvents.UseCases;

public sealed class GetLatestPQEvent(IPQEventsRepository repo)
{
    public async Task<PQEventDto?> HandleAsync(int? sourceId, CancellationToken ct)
    {
        var e = await repo.GetLatestAsync(sourceId, ct);
        return e is null ? null : new PQEventDto(
            e.EventId, e.SourceId, e.DatalogTimestampUtc, e.StartTimestampUtc, e.EndTimestampUtc,
            e.WorstPhase, e.Direction, e.WorstPhaseDuration, e.WorstPhaseMagnitude, e.WorstPhaseSeverity,
            e.Classification, e.HasProcessImpact
        );
    }
}
