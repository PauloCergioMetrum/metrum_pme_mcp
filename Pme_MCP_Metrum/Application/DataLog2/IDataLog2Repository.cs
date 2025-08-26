using Pme_MCP_Metrum.Domain.Entities;

namespace Pme_MCP_Metrum.Domain.Repositories;

public interface IDataLog2Repository
{
    Task<IEnumerable<DataLog2>> ListAsync(
        int? top,
        int? sourceId,
        short? quantityId,
        string? type,
        DateTime? fromUtc,
        DateTime? toUtc,
        CancellationToken ct);
}
