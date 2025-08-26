using Pme_MCP_Metrum.Application.DataLog2.Dtos;
using Pme_MCP_Metrum.Domain.Repositories;

namespace Pme_MCP_Metrum.Application.DataLog2.UseCases;

public sealed class ListDataLog2(IDataLog2Repository repo)
{
    public async Task<IEnumerable<DataLog2Dto>> HandleAsync(
        int? top, int? sourceId, short? quantityId, string? type, DateTime? fromUtc, DateTime? toUtc, CancellationToken ct)
    {
        var rows = await repo.ListAsync(top, sourceId, quantityId, type, fromUtc, toUtc, ct);
        return rows.Select(x => new DataLog2Dto(x.ID, x.Value, x.SourceID, x.QuantityID, x.TimestampUTC, x.Type));
    }

  
}
