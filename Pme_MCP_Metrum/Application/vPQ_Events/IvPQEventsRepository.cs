using Pme_MCP_Metrum.Domain.Entities;

namespace Pme_MCP_Metrum.Domain.Repositories.PQEvents
{
    public interface IPQEventsRepository
    {
  
        Task<IReadOnlyList<PQEvent>> ListTopAsync(int top, CancellationToken ct = default);


        Task<IEnumerable<PQEvent>> ListAsync(
            int? top,
            int? sourceId,
            string? classification,
            bool? hasProcessImpact,
            string? worstPhase,
            short? direction,
            DateTime? fromUtc,
            DateTime? toUtc,
            CancellationToken ct);

        Task<PQEvent?> GetLatestAsync(int? sourceId, CancellationToken ct);

        Task<int> CountByDayAsync(DateTime dayUtc, int? sourceId, CancellationToken ct);
    }
}
