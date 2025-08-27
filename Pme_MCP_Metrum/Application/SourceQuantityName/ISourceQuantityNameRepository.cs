using Pme_MCP_Metrum.Application.SourceQuantityNames.Dtos;
namespace Pme_MCP_Metrum.Application.SourceQuantityNames;
public interface ISourceQuantityNameRepository
{
    Task<IEnumerable<SourceQuantityNameDto>> ListAsync(
        int? top,
        int? sourceId,
        short? quantityId,
        string? sourceNameLike,
        string? quantityNameLike,
        DateTime? fromUtc,
        DateTime? toUtc,
        CancellationToken ct);

    Task<SourceQuantityNameDto?> GetByKeyAsync(int sourceId, short quantityId, CancellationToken ct);

    Task<(DateTime? MinUtc, DateTime? MaxUtc)> GetTimeRangeAsync(
        int? sourceId, short? quantityId, CancellationToken ct);

    Task<int> CountAsync(
        int? sourceId,
        short? quantityId,
        string? sourceNameLike,
        string? quantityNameLike,
        DateTime? fromUtc,
        DateTime? toUtc,
        CancellationToken ct);
}
