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

    //TABELA SOURCES
    [McpServerTool(Name = "sources_list_v1")]
    [Description("Lista fontes (Source) do ION_Data. Parâmetro opcional: top (int).")]
    public Task<ListSourcesResponse> ListAsync(SourcesListArgs? args = default, CancellationToken ct = default)
        => _listSources.Execute(new ListSourcesRequest(args?.top ?? 1000), ct);



}
