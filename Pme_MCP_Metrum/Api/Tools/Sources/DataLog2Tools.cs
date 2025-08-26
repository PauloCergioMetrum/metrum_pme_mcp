using System.ComponentModel;
using ModelContextProtocol.Server;
using Pme_MCP_Metrum.Application.DataLog2.Dtos;
using Pme_MCP_Metrum.Application.DataLog2.UseCases;

namespace Pme_MCP_Metrum.Api.Tools.DataLog2;

[McpServerToolType]
public sealed class DataLog2Tools(ListDataLog2 list)
{
    public sealed record DataLog2ListArgs(
        int? top,
        int? sourceId,
        short? quantityId,
        string? type,
        DateTime? fromUtc,
        DateTime? toUtc
    );

    [McpServerTool(Name = "datalog2_list_v1")]
    [Description("Lista registros de vwDataLog2 com filtros opcionais (top, sourceId, quantityId, type, fromUtc, toUtc). Ordena por TimestampUTC desc.")]
    public Task<IEnumerable<DataLog2Dto>> ListAsync(DataLog2ListArgs? args = default, CancellationToken ct = default)
        => list.HandleAsync(args?.top, args?.sourceId, args?.quantityId, args?.type, args?.fromUtc, args?.toUtc, ct);
}
