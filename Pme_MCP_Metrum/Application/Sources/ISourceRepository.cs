
using Pme_MCP_Metrum.Domain.Entities;

namespace Pme_MCP_Metrum.Application.Sources;

public interface ISourceRepository
{
    Task<IReadOnlyList<Source>> ListTopAsync(int top, CancellationToken ct = default); 
    Task<IReadOnlyList<Source>> SearchAsync(
        int skip, int top,
        int? id, string? nameLike, int? namespaceId, int? sourceTypeId, int? timeZoneId,
        bool? hasDescription, bool? hasSignature, string? orderBy,
        CancellationToken ct
    );
    Task<int> CountAsync(
        int? id, string? nameLike, int? namespaceId, int? sourceTypeId, int? timeZoneId,
        bool? hasDescription, bool? hasSignature,
        CancellationToken ct
    );
    Task<Source?> GetByIdAsync(int id, CancellationToken ct);
}
