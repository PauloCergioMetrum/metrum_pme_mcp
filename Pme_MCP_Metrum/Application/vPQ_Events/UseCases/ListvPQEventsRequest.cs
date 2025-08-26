using Pme_MCP_Metrum.Domain.Repositories;
using Pme_MCP_Metrum.Domain.Repositories.PQEvents;

namespace Pme_MCP_Metrum.Application.PQEvents.UseCases;

public sealed class CountPQEventsByDay(IPQEventsRepository repo)
{
    public Task<int> HandleAsync(DateTime dayUtc, int? sourceId, CancellationToken ct)
        => repo.CountByDayAsync(dayUtc, sourceId, ct);
}
