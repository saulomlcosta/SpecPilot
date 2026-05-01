using SpecPilot.Domain.Common;

namespace SpecPilot.Domain.Entities;

public class AiInteractionLog : BaseEntity
{
    public Guid ProjectId { get; set; }
    public string InteractionType { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public string PromptName { get; set; } = string.Empty;
    public string InputPayload { get; set; } = string.Empty;
    public string OutputPayload { get; set; } = string.Empty;
    public bool IsSuccessful { get; set; }
    public Project? Project { get; set; }
}
