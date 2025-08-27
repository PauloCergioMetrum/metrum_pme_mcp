using Pme_MCP_Metrum.Application.SourceQuantityName.UseCases;
using Pme_MCP_Metrum.Application.SourceQuantityNames.Dtos;

namespace Pme_MCP_Metrum.Application.SourceQuantityNames.UseCases;

public sealed class ListSourceQuantityName(ISourceQuantityNameRepository repo)
{
    public async Task<ListSourceQuantityNameResponse> HandleAsync(
        ListSourceQuantityNameRequest req,
        CancellationToken ct)
    {
        var items = await repo.ListAsync(
            req.Top, req.SourceId, req.QuantityId,
            req.SourceNameLike, req.QuantityNameLike,
            req.FromUtc, req.ToUtc, ct);


        var total = await repo.CountAsync(
            req.SourceId, req.QuantityId,
            req.SourceNameLike, req.QuantityNameLike,
            req.FromUtc, req.ToUtc, ct);

        return new ListSourceQuantityNameResponse(items.ToList(), total);
    }
}
