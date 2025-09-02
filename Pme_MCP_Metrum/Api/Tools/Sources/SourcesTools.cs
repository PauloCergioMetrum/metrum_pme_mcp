using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using Pme_MCP_Metrum.Application.Sources.UseCases;
using System.ComponentModel;

namespace Pme_MCP_Metrum.Api.Tools.Sources;

[McpServerToolType]
public sealed class SourcesTools
{
    private readonly ListSources _listTopLegacy;
    private readonly SearchSources _search;
    private readonly GetSourceById _getById;
    private readonly ILogger<SourcesTools> _log;


    private const string SearchName = "source_search_v1";
    private const string GetName = "source_get_v1";
    private const string CountName = "source_count_v1";
    private const string LegacyList = "sources_list_v1";

    public SourcesTools(ListSources listSources, SearchSources search, GetSourceById getById, ILogger<SourcesTools> log)
    {
        _listTopLegacy = listSources;
        _search = search;
        _getById = getById;
        _log = log;
    }

    [McpServerTool(Name = SearchName)]
    [Description("Busca fontes aplicando filtros opcionais como ID, Name, NamespaceID, SourceTypeID, TimeZoneID, além de suporte a paginação e ordenação.")]
    public Task<PagedResponse<Pme_MCP_Metrum.Application.Sources.Dtos.SourceDto>> SearchAsync(
        SourceSearchArgs? args = default, CancellationToken ct = default)
    {
        var a = args ?? new();
        _log.LogInformation("{tool} chamada com filtros: {@args}", SearchName, a);
        return _search.Execute(a, ct);
    }

    public sealed record SourceGetArgs(int id);

    [McpServerTool(Name = GetName)]
    [Description("Obtém uma única fonte  pelo seu identificador (ID).")]
    public Task<Pme_MCP_Metrum.Application.Sources.Dtos.SourceDto?> GetAsync(SourceGetArgs a, CancellationToken ct = default)
        => _getById.Execute(a.id, ct);

    [McpServerTool(Name = CountName)]
    [Description("Conta quantas fontes  existem de acordo com os filtros informados (mesmos filtros de source_search_v1).")]
    public async Task<int> CountAsync(SourceSearchArgs? args = default, CancellationToken ct = default)
    {
        var a = args ?? new();
        return await _search
            .Execute(a with { top = 1, skip = 0 }, ct)
            .ContinueWith(t => t.Result.Total, ct);
    }


    public sealed record SourcesListArgs(int? top);

    [McpServerTool(Name = LegacyList)]
    [Description(" Lista as primeiras fontes  da tabela ION_Data. Parâmetro opcional.")]
    public Task<ListSourcesResponse> ListAsync(SourcesListArgs? args = default, CancellationToken ct = default)
        => _listTopLegacy.Execute(new ListSourcesRequest(args?.top ?? 100), ct);
}
