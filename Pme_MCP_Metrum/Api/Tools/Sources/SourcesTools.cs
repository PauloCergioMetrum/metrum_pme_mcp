using System.ComponentModel;
using Dapper;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using Pme_MCP_Metrum.Application.Sources.UseCases;
using Pme_MCP_Metrum.Infrastructure.Persistence;

namespace Pme_MCP_Metrum.Api.Tools.Sources;

[McpServerToolType]
public sealed class SourcesTools
{
    private readonly ListSources _listSources;
    private readonly SqlConnectionFactory _factory;
    private readonly ILogger<SourcesTools> _log;

    public SourcesTools(ListSources listSources, SqlConnectionFactory factory, ILogger<SourcesTools> log)
    {
        _listSources = listSources;
        _factory = factory;
        _log = log;
    }

    public sealed record SourcesListArgs(int? top);

    [McpServerTool(Name = "sources_list_v1")]
    [Description("Lista fontes (Source) do ION_Data. Parâmetro opcional: top (int).")]
    public Task<ListSourcesResponse> ListAsync(SourcesListArgs? args = default, CancellationToken ct = default)
        => _listSources.Execute(new ListSourcesRequest(args?.top ?? 1000), ct);

    public sealed record PingArgs(string? message);

    [McpServerTool(Name = "ping_v1")]
    [Description("Retorna a mensagem enviada para teste de conectividade.")]
    public Task<object> PingAsync(PingArgs? args = default, CancellationToken ct = default)
        => Task.FromResult<object>(new { ok = true, echo = args?.message ?? "pong" });

    [McpServerTool(Name = "sources_count_v1")]
    [Description("Retorna COUNT(*) de ION_Data.dbo.Source para diagnosticar conectividade e permissões.")]
    public async Task<object> CountAsync(CancellationToken ct = default)
    {
        try
        {
            using var conn = _factory.Create();
            await conn.OpenAsync(ct);

            var cmd = new CommandDefinition(
                "SELECT COUNT(*) FROM [ION_Data].[dbo].[Source]",
                parameters: null,
                commandTimeout: 10,
                cancellationToken: ct
            );

            var count = await conn.ExecuteScalarAsync<int>(cmd);
            return new { ok = true, count };
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Erro ao executar sources_count_v1");
            return new { ok = false, error = ex.GetType().FullName, message = ex.Message };
        }
    }
}
