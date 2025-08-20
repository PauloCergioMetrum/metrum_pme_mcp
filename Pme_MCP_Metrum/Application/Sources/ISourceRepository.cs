using Pme_MCP_Metrum.Domain.Entities;

namespace Pme_MCP_Metrum.Application.Sources;

public interface ISourceRepository
{
    Task<IReadOnlyList<Source>> ListTopAsync(int top, CancellationToken ct = default);
}
