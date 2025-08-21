using Pme_MCP_Metrum.Application.Sources.Dtos;
using Pme_MCP_Metrum.Application.Sources;


namespace Pme_MCP_Metrum.Application.Sources.UseCases;


public sealed class ListSources
{
    private readonly ISourceRepository _repo;


    public ListSources(ISourceRepository repo)
    {
        _repo = repo;
    }


    public async Task<ListSourcesResponse> Execute(ListSourcesRequest req, CancellationToken ct = default)
    {
        var top = req.Top <= 0 ? 1000 : req.Top;
        var items = await _repo.ListTopAsync(top, ct);


        var dtos = items.Select(s => new SourceDto(
        s.ID, s.Name, s.NamespaceID, s.SourceTypeID, s.TimeZoneID,
        s.Description, s.Signature, s.DisplayName
        )).ToList();


        return new ListSourcesResponse(dtos);
    }
}