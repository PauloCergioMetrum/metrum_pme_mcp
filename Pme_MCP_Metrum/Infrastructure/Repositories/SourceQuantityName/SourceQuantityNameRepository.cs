
using Dapper;
using Pme_MCP_Metrum.Application.SourceQuantityNames;
using Pme_MCP_Metrum.Application.SourceQuantityNames.Dtos;
using Pme_MCP_Metrum.Infrastructure.Persistence;

namespace Pme_MCP_Metrum.Infrastructure.Repositories.SourceQuantityNames;

public sealed class SourceQuantityNameRepository(SqlConnectionFactory factory)
    : ISourceQuantityNameRepository
{
    public async Task<IEnumerable<SourceQuantityNameDto>> ListAsync(
        int? top, int? sourceId, short? quantityId,
        string? sourceNameLike, string? quantityNameLike,
        DateTime? fromUtc, DateTime? toUtc, CancellationToken ct)
    {
        var sb = new System.Text.StringBuilder();
        sb.Append("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; SELECT ");
        if (top is > 0) sb.Append("TOP (@top) ");
        sb.Append("""
            [SourceID],[SourceName],[QuantityID],[QuantityName],
            [MinTimestampUtc],[MaxTimestampUtc]
            FROM [ION_Data].[dbo].[vSourceQuantityName] WITH (NOLOCK)
            WHERE 1=1
            """);

        var p = new DynamicParameters();
        if (top is > 0) p.Add("top", top);
        if (sourceId is not null) { sb.Append(" AND [SourceID]=@sourceId"); p.Add("sourceId", sourceId); }
        if (quantityId is not null) { sb.Append(" AND [QuantityID]=@quantityId"); p.Add("quantityId", quantityId); }
        if (!string.IsNullOrWhiteSpace(sourceNameLike))
        { sb.Append(" AND [SourceName] LIKE @sn"); p.Add("sn", $"%{sourceNameLike}%"); }
        if (!string.IsNullOrWhiteSpace(quantityNameLike))
        { sb.Append(" AND [QuantityName] LIKE @qn"); p.Add("qn", $"%{quantityNameLike}%"); }


        if (fromUtc is not null) { sb.Append(" AND [MaxTimestampUtc] >= @fromUtc"); p.Add("fromUtc", fromUtc); }
        if (toUtc is not null) { sb.Append(" AND [MinTimestampUtc] <= @toUtc"); p.Add("toUtc", toUtc); }

        sb.Append(" ORDER BY [SourceID],[QuantityID]");

        using var conn = factory.Create();
        await conn.OpenAsync(ct);
        return await conn.QueryAsync<SourceQuantityNameDto>(new CommandDefinition(sb.ToString(), p, cancellationToken: ct));
    }

    public async Task<SourceQuantityNameDto?> GetByKeyAsync(int sourceId, short quantityId, CancellationToken ct)
    {
        const string sql = """
        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
        SELECT TOP (1)
            [SourceID],[SourceName],[QuantityID],[QuantityName],
            [MinTimestampUtc],[MaxTimestampUtc]
        FROM [ION_Data].[dbo].[vSourceQuantityName] WITH (NOLOCK)
        WHERE [SourceID]=@sourceId AND [QuantityID]=@quantityId;
        """;

        using var conn = factory.Create();
        await conn.OpenAsync(ct);
        return await conn.QueryFirstOrDefaultAsync<SourceQuantityNameDto>(
            new CommandDefinition(sql, new { sourceId, quantityId }, cancellationToken: ct));
    }

    public async Task<(DateTime? MinUtc, DateTime? MaxUtc)> GetTimeRangeAsync(
        int? sourceId, short? quantityId, CancellationToken ct)
    {
        var sb = new System.Text.StringBuilder("""
        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
        SELECT MIN([MinTimestampUtc]) AS MinUtc, MAX([MaxTimestampUtc]) AS MaxUtc
        FROM [ION_Data].[dbo].[vSourceQuantityName] WITH (NOLOCK)
        WHERE 1=1
        """);
        var p = new DynamicParameters();
        if (sourceId is not null) { sb.Append(" AND [SourceID]=@sourceId"); p.Add("sourceId", sourceId); }
        if (quantityId is not null) { sb.Append(" AND [QuantityID]=@quantityId"); p.Add("quantityId", quantityId); }

        using var conn = factory.Create();
        await conn.OpenAsync(ct);
        return await conn.QueryFirstOrDefaultAsync<(DateTime? MinUtc, DateTime? MaxUtc)>(
            new CommandDefinition(sb.ToString(), p, cancellationToken: ct));
    }

    public async Task<int> CountAsync(
        int? sourceId, short? quantityId, string? sourceNameLike, string? quantityNameLike,
        DateTime? fromUtc, DateTime? toUtc, CancellationToken ct)
    {
        var sb = new System.Text.StringBuilder("""
        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
        SELECT COUNT(1)
        FROM [ION_Data].[dbo].[vSourceQuantityName] WITH (NOLOCK)
        WHERE 1=1
        """);
        var p = new DynamicParameters();
        if (sourceId is not null) { sb.Append(" AND [SourceID]=@sourceId"); p.Add("sourceId", sourceId); }
        if (quantityId is not null) { sb.Append(" AND [QuantityID]=@quantityId"); p.Add("quantityId", quantityId); }
        if (!string.IsNullOrWhiteSpace(sourceNameLike))
        { sb.Append(" AND [SourceName] LIKE @sn"); p.Add("sn", $"%{sourceNameLike}%"); }
        if (!string.IsNullOrWhiteSpace(quantityNameLike))
        { sb.Append(" AND [QuantityName] LIKE @qn"); p.Add("qn", $"%{quantityNameLike}%"); }
        if (fromUtc is not null) { sb.Append(" AND [MaxTimestampUtc] >= @fromUtc"); p.Add("fromUtc", fromUtc); }
        if (toUtc is not null) { sb.Append(" AND [MinTimestampUtc] <= @toUtc"); p.Add("toUtc", toUtc); }

        using var conn = factory.Create();
        await conn.OpenAsync(ct);
        return await conn.ExecuteScalarAsync<int>(new CommandDefinition(sb.ToString(), p, cancellationToken: ct));
    }
}
