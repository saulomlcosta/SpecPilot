namespace SpecPilot.Application.Projects.Common;

public class ProjectResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string InitialDescription { get; init; } = string.Empty;
    public string Goal { get; init; } = string.Empty;
    public string TargetAudience { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}
