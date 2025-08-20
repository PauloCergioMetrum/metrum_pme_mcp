namespace Pme_MCP_Metrum.Domain.Entities;

public sealed class Source
{
    public int ID { get; }
    public string? Name { get; }
    public int? NamespaceID { get; }
    public int? SourceTypeID { get; }
    public int? TimeZoneID { get; }
    public string? Description { get; }
    public string? Signature { get; }
    public string? DisplayName { get; }

    public Source(
        int id, string? name, int? namespaceId, int? sourceTypeId, int? timeZoneId,
        string? description, string? signature, string? displayName)
    {
        ID = id;
        Name = name;
        NamespaceID = namespaceId;
        SourceTypeID = sourceTypeId;
        TimeZoneID = timeZoneId;
        Description = description;
        Signature = signature;
        DisplayName = displayName;
    }
}
