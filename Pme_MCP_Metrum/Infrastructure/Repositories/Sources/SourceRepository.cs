
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
        if (top <= 0) top = 100;
        const string sql = """
        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
        SELECT TOP (@top)
               [ID],[Name],
               CAST([NamespaceID]  AS int) AS [NamespaceID],
               CAST([SourceTypeID] AS int) AS [SourceTypeID],
               CAST([TimeZoneID]   AS int) AS [TimeZoneID],
               [Description],[Signature],[DisplayName]
        FROM [ION_Data].[dbo].[Source] WITH (NOLOCK)
        ORDER BY [ID];
        """;
        using var conn = _factory.Create();
        await conn.OpenAsync(ct);
        var rows = await conn.QueryAsync<Row>(new CommandDefinition(sql, new { top }, commandTimeout: 15, cancellationToken: ct));
        return rows.Select(ToDomain).ToList();
    }

    public async Task<IReadOnlyList<Source>> SearchAsync(
        int skip, int top,
        int? id, string? nameLike, int? namespaceId, int? sourceTypeId, int? timeZoneId,
        bool? hasDescription, bool? hasSignature, string? orderBy,
        CancellationToken ct)
    {
        top = Math.Clamp(top <= 0 ? 100 : top, 1, 1000);
        skip = Math.Max(0, skip);
        orderBy = (orderBy ?? "Name") switch
        {
            "ID" => "[ID]",
            "DisplayName" => "[DisplayName]",
            _ => "[Name]"
        };

        var sql = """
        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
        WITH Q AS (
          SELECT
            [ID],[Name],
            CAST([NamespaceID]  AS int) AS [NamespaceID],
            CAST([SourceTypeID] AS int) AS [SourceTypeID],
            CAST([TimeZoneID]   AS int) AS [TimeZoneID],
            [Description],[Signature],[DisplayName]
          FROM [ION_Data].[dbo].[Source] WITH (NOLOCK)
          WHERE ( @id IS NULL OR [ID] = @id )
            AND ( @namespaceId IS NULL OR [NamespaceID] = @namespaceId )
            AND ( @sourceTypeId IS NULL OR [SourceTypeID] = @sourceTypeId )
            AND ( @timeZoneId IS NULL OR [TimeZoneID] = @timeZoneId )
            AND ( @nameLike IS NULL OR [Name] LIKE @nameLike )
            AND ( @hasDescription IS NULL OR
                  ( @hasDescription = 1 AND NULLIF(LTRIM(RTRIM([Description])), '') IS NOT NULL ) OR
                  ( @hasDescription = 0 AND ( [Description] IS NULL OR LTRIM(RTRIM([Description])) = '' ) )
                )
            AND ( @hasSignature IS NULL OR
                  ( @hasSignature = 1 AND NULLIF(LTRIM(RTRIM([Signature])), '') IS NOT NULL ) OR
                  ( @hasSignature = 0 AND ( [Signature] IS NULL OR LTRIM(RTRIM([Signature])) = '' ) )
                )
        )
        SELECT *
        FROM Q
        ORDER BY
          CASE WHEN @orderBy = '[ID]' THEN [ID] END,
          CASE WHEN @orderBy = '[Name]' THEN [Name] END,
          CASE WHEN @orderBy = '[DisplayName]' THEN [DisplayName] END
        OFFSET @skip ROWS FETCH NEXT @top ROWS ONLY;
        """;

        var p = new
        {
            id,
            nameLike = nameLike is null ? null : $"%{nameLike}%",
            namespaceId,
            sourceTypeId,
            timeZoneId,
            hasDescription,
            hasSignature,
            skip,
            top,
            orderBy
        };

        using var conn = _factory.Create();
        await conn.OpenAsync(ct);
        var rows = await conn.QueryAsync<Row>(new CommandDefinition(sql, p, commandTimeout: 30, cancellationToken: ct));
        return rows.Select(ToDomain).ToList();
    }

    public async Task<int> CountAsync(
        int? id, string? nameLike, int? namespaceId, int? sourceTypeId, int? timeZoneId,
        bool? hasDescription, bool? hasSignature,
        CancellationToken ct)
    {
        const string sql = """
        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
        SELECT COUNT(1)
        FROM [ION_Data].[dbo].[Source] WITH (NOLOCK)
        WHERE ( @id IS NULL OR [ID] = @id )
          AND ( @namespaceId IS NULL OR [NamespaceID] = @namespaceId )
          AND ( @sourceTypeId IS NULL OR [SourceTypeID] = @sourceTypeId )
          AND ( @timeZoneId IS NULL OR [TimeZoneID] = @timeZoneId )
          AND ( @nameLike IS NULL OR [Name] LIKE @nameLike )
          AND ( @hasDescription IS NULL OR
                ( @hasDescription = 1 AND NULLIF(LTRIM(RTRIM([Description])), '') IS NOT NULL ) OR
                ( @hasDescription = 0 AND ( [Description] IS NULL OR LTRIM(RTRIM([Description])) = '' ) )
              )
          AND ( @hasSignature IS NULL OR
                ( @hasSignature = 1 AND NULLIF(LTRIM(RTRIM([Signature])), '') IS NOT NULL ) OR
                ( @hasSignature = 0 AND ( [Signature] IS NULL OR LTRIM(RTRIM([Signature])) = '' ) )
              );
        """;

        var p = new
        {
            id,
            nameLike = nameLike is null ? null : $"%{nameLike}%",
            namespaceId,
            sourceTypeId,
            timeZoneId,
            hasDescription,
            hasSignature
        };

        using var conn = _factory.Create();
        await conn.OpenAsync(ct);
        return await conn.ExecuteScalarAsync<int>(new CommandDefinition(sql, p, commandTimeout: 30, cancellationToken: ct));
    }

    public async Task<Source?> GetByIdAsync(int id, CancellationToken ct)
    {
        const string sql = """
        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
        SELECT TOP (1)
               [ID],[Name],
               CAST([NamespaceID]  AS int) AS [NamespaceID],
               CAST([SourceTypeID] AS int) AS [SourceTypeID],
               CAST([TimeZoneID]   AS int) AS [TimeZoneID],
               [Description],[Signature],[DisplayName]
        FROM [ION_Data].[dbo].[Source] WITH (NOLOCK)
        WHERE [ID] = @id;
        """;
        using var conn = _factory.Create();
        await conn.OpenAsync(ct);
        var row = await conn.QueryFirstOrDefaultAsync<Row>(new CommandDefinition(sql, new { id }, commandTimeout: 15, cancellationToken: ct));
        return row is null ? null : ToDomain(row);
    }

    private static Source ToDomain(Row r) => new(
        r.ID, r.Name, r.NamespaceID, r.SourceTypeID, r.TimeZoneID, r.Description, r.Signature, r.DisplayName
    );
}
