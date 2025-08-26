using Pme_MCP_Metrum.Application.DataLog2.Dtos;

namespace Pme_MCP_Metrum.Application.DataLog2.UseCases;

public sealed record ListDataLog2Response(IEnumerable<DataLog2Dto> Items);
