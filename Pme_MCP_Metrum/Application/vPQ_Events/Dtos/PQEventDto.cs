namespace Pme_MCP_Metrum.Application.PQEvents.Dtos;

public sealed record PQEventDto(
    int EventId,
    int SourceId,
    DateTime DatalogTimestampUtc,
    DateTime? StartTimestampUtc,
    DateTime? EndTimestampUtc,
    string? WorstPhase,
    short? Direction,
    double? WorstPhaseDuration,
    double? WorstPhaseMagnitude,
    double? WorstPhaseSeverity,
    string? Classification,
    bool HasProcessImpact
);
