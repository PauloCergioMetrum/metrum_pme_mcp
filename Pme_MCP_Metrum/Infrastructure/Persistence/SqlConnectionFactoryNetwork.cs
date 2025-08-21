using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Pme_MCP_Metrum.Infrastructure.Persistence;

public sealed class SqlConnectionFactoryNetwork
{
    private readonly string _cs;

    public SqlConnectionFactoryNetwork(IConfiguration cfg)
        => _cs = cfg.GetConnectionString("IonNetwork")
           ?? throw new InvalidOperationException("ConnectionStrings:IonNetwork not found in appsettings.");

    public SqlConnection Create() => new SqlConnection(_cs);
}
