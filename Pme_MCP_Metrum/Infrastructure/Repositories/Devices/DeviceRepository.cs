using Dapper;
using Pme_MCP_Metrum.Application.Devices;
using Pme_MCP_Metrum.Domain.Entities;
using Pme_MCP_Metrum.Infrastructure.Persistence;

namespace Pme_MCP_Metrum.Infrastructure.Repositories.Devices;

public sealed class DeviceRepository : IDeviceRepository
{
    private readonly SqlConnectionFactoryNetwork _factory;

    public DeviceRepository(SqlConnectionFactoryNetwork factory)
    {
        _factory = factory;
    }

    public async Task<IReadOnlyList<Device>> ListAsync(string? type, string? siteStatus, string? protocol, CancellationToken ct = default)
    {
        const string sql = """
            SELECT TOP 1000 *
            FROM [ION_Network].[dbo].[vPMCDevice]
            WHERE (@type IS NULL OR [Type] = @type)
              AND (@siteStatus IS NULL OR [Site Status] = @siteStatus)
              AND (@protocol IS NULL OR [Protocol] = @protocol)
            ORDER BY [ID];
        """;

        using var conn = _factory.Create();
        await conn.OpenAsync(ct);

        var result = await conn.QueryAsync<Device>(sql, new { type, siteStatus, protocol });
        return result.ToList();
    }
}
