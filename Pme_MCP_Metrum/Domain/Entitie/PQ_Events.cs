namespace Pme_MCP_Metrum.Domain.Entities;

public sealed class PQEvent
{
    public int EventId { get; init; }
    public int SourceId { get; init; }
    public DateTime DatalogTimestampUtc { get; init; }
    public DateTime? StartTimestampUtc { get; init; }
    public DateTime? EndTimestampUtc { get; init; }
    public string? WorstPhase { get; init; }          
    public short? Direction { get; init; }       
    public double? WorstPhaseDuration { get; init; }  
    public double? WorstPhaseMagnitude { get; init; } 
    public double? WorstPhaseSeverity { get; init; } 
    public string? Classification { get; init; }      
    public bool HasProcessImpact { get; init; }       
}
