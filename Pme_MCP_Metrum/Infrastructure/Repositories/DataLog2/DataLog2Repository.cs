using Dapper;
using Pme_MCP_Metrum.Domain.Repositories;
using Pme_MCP_Metrum.Infrastructure.Persistence;
using Entity = Pme_MCP_Metrum.Domain.Entities.DataLog2;

namespace Pme_MCP_Metrum.Infrastructure.Repositories.DataLogs2; 

public sealed class DataLog2Repository(SqlConnectionFactory factory) : IDataLog2Repository
{
    public async Task<IEnumerable<Entity>> ListAsync(
        int? top, int? sourceId, short? quantityId, string? type, DateTime? fromUtc, DateTime? toUtc, CancellationToken ct)
    {
        using var conn = factory.Create();
        await conn.OpenAsync(ct);

        var sql = $"""
        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
        SELECT {(top.HasValue ? "TOP (@top)" : "")}
               [ID], [Value], [SourceID], [QuantityID], [TimestampUTC], [Type]
        FROM [ION_Data].[dbo].[vwDataLog2] WITH (NOLOCK)
        WHERE (@sourceId IS NULL OR [SourceID] = @sourceId)
          AND (@quantityId IS NULL OR [QuantityID] = @quantityId)
          AND (@type IS NULL OR [Type] = @type)
          AND (@fromUtc IS NULL OR [TimestampUTC] >= @fromUtc)
          AND (@toUtc IS NULL OR [TimestampUTC] <= @toUtc)
        ORDER BY [TimestampUTC] DESC;
        """;

        return await conn.QueryAsync<Entity>(sql, new { top, sourceId, quantityId, type, fromUtc, toUtc });
    }
}
