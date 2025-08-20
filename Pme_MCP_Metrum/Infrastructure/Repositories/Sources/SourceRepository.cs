using Dapper;
using Pme_MCP_Metrum.Application.Sources;
using Pme_MCP_Metrum.Domain.Entities;
using Pme_MCP_Metrum.Infrastructure.Persistence;

namespace Pme_MCP_Metrum.Infrastructure.Repositories.Sources;

public sealed class SourceRepository : ISourceRepository
{
    private readonly SqlConnectionFactory _factory;
    public SourceRepository(SqlConnectionFactory factory) => _factory = factory;
    private sealed record Row(
        int ID,
        string? Name,
        int? NamespaceID,
        int? SourceTypeID,
        int? TimeZoneID,
        string? Description,
        string? Signature,
        string? DisplayName
    );

    public async Task<IReadOnlyList<Source>> ListTopAsync(int top, CancellationToken ct = default)
    {
        if (top <= 0) top = 1000;

        const string sql = """
        SELECT TOP (@top)
               [ID],
               [Name],
               CAST([NamespaceID]  AS int) AS [NamespaceID],
               CAST([SourceTypeID] AS int) AS [SourceTypeID],
               CAST([TimeZoneID]   AS int) AS [TimeZoneID],
               [Description],
               [Signature],
               [DisplayName]
        FROM [ION_Data].[dbo].[Source]
        ORDER BY [ID];
        """;

        using var conn = _factory.Create();
        await conn.OpenAsync(ct);

        var rows = await conn.QueryAsync<Row>(
            new CommandDefinition(sql, new { top }, commandTimeout: 10, cancellationToken: ct));

        return rows.Select(r => new Source(
            r.ID, r.Name, r.NamespaceID, r.SourceTypeID, r.TimeZoneID,
            r.Description, r.Signature, r.DisplayName
        )).ToList();
    }
}
