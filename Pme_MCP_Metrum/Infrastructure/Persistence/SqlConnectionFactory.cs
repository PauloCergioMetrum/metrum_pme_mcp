using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Pme_MCP_Metrum.Infrastructure.Persistence;

public sealed class SqlConnectionFactory
{
    private readonly string _cs;
    public SqlConnectionFactory(IConfiguration cfg)
        => _cs = cfg.GetConnectionString("IonData")
           ?? throw new InvalidOperationException("ConnectionStrings:IonData not found in appsettings.");

    public SqlConnection Create() => new SqlConnection(_cs);
}
