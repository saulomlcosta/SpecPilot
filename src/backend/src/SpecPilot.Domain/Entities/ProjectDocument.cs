using SpecPilot.Domain.Common;

namespace SpecPilot.Domain.Entities;

public class ProjectDocument : BaseEntity
{
    public Guid ProjectId { get; set; }
    public string Overview { get; set; } = string.Empty;
    public string FunctionalRequirements { get; set; } = string.Empty;
    public string NonFunctionalRequirements { get; set; } = string.Empty;
    public string UseCases { get; set; } = string.Empty;
    public string Risks { get; set; } = string.Empty;
    public DateTime GeneratedAtUtc { get; set; } = DateTime.UtcNow;
    public Project? Project { get; set; }
}
