
namespace Pme_MCP_Metrum.Application.Sources.UseCases;

public sealed record SourceSearchArgs(
    int? id = null,
    string? nameLike = null,
    int? namespaceId = null,
    int? sourceTypeId = null,
    int? timeZoneId = null,
    bool? hasDescription = null,
    bool? hasSignature = null,
    int skip = 0,
    int top = 100,
    string? orderBy = "Name" 
);


public sealed record PagedResponse<T>(
    IReadOnlyList<T> Items, int Total, int Skip, int Top, string? Warning = null
);
