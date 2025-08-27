using ModelContextProtocol.Server;
using Pme_MCP_Metrum.Application.SourceQuantityName.UseCases;
using Pme_MCP_Metrum.Application.SourceQuantityNames.UseCases;
using System.ComponentModel;

namespace Pme_MCP_Metrum.Api.Tools.SourceQuantityName;

[McpServerToolType]
public sealed class SourceQuantityNameTools(
    ListSourceQuantityName listUseCase
)
{
    public sealed record ListArgs(
        int? top,
        int? sourceId,
        short? quantityId,
        string? sourceNameLike,
        string? quantityNameLike,
        DateTime? fromUtc,
        DateTime? toUtc
    );

    [McpServerTool(Name = "sqn_list_v1")]
    [Description("Lista vSourceQuantityName com filtros opcionais: top, sourceId, quantityId, sourceNameLike, quantityNameLike, fromUtc, toUtc.")]
    public Task<ListSourceQuantityNameResponse> ListAsync(ListArgs? a = default, CancellationToken ct = default)
    {
        var req = new ListSourceQuantityNameRequest(
            Top: a?.top,
            SourceId: a?.sourceId,
            QuantityId: a?.quantityId,
            SourceNameLike: a?.sourceNameLike,
            QuantityNameLike: a?.quantityNameLike,
            FromUtc: a?.fromUtc,
            ToUtc: a?.toUtc
        );

        return listUseCase.HandleAsync(req, ct);
    }
}
