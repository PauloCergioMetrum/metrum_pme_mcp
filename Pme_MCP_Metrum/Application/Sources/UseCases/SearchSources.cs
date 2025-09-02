
using Pme_MCP_Metrum.Application.Sources;
using Pme_MCP_Metrum.Application.Sources.Dtos;
using Pme_MCP_Metrum.Application.Sources.UseCases;

namespace Pme_MCP_Metrum.Application.Sources.UseCases;

public sealed class SearchSources
{
    private readonly ISourceRepository _repo;
    public SearchSources(ISourceRepository repo) => _repo = repo;

    public async Task<PagedResponse<SourceDto>> Execute(SourceSearchArgs a, CancellationToken ct = default)
    {
        var items = await _repo.SearchAsync(
            a.skip, a.top, a.id, a.nameLike, a.namespaceId, a.sourceTypeId, a.timeZoneId,
            a.hasDescription, a.hasSignature, a.orderBy, ct);

        var total = await _repo.CountAsync(
            a.id, a.nameLike, a.namespaceId, a.sourceTypeId, a.timeZoneId, a.hasDescription, a.hasSignature, ct);

        var dtos = items.Select(s => new SourceDto(
            s.ID, s.Name, s.NamespaceID, s.SourceTypeID, s.TimeZoneID, s.Description, s.Signature, s.DisplayName
        )).ToList();

        return new PagedResponse<SourceDto>(dtos, total, a.skip, a.top);
    }
}




public sealed class GetSourceById
{
    private readonly ISourceRepository _repo;
    public GetSourceById(ISourceRepository repo) => _repo = repo;

    public async Task<SourceDto?> Execute(int id, CancellationToken ct = default)
    {
        var s = await _repo.GetByIdAsync(id, ct);
        return s is null ? null : new SourceDto(
            s.ID, s.Name, s.NamespaceID, s.SourceTypeID, s.TimeZoneID, s.Description, s.Signature, s.DisplayName
        );
    }
}
