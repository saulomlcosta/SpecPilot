using SpecPilot.Domain.Common;
using SpecPilot.Domain.Enums;

namespace SpecPilot.Domain.Entities;

public class Project : BaseEntity
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string InitialDescription { get; set; } = string.Empty;
    public ProjectStatus Status { get; set; } = ProjectStatus.Draft;
    public User? User { get; set; }
    public ICollection<RefinementQuestion> RefinementQuestions { get; set; } = new List<RefinementQuestion>();
    public ProjectDocument? ProjectDocument { get; set; }
    public ICollection<AiInteractionLog> AiInteractionLogs { get; set; } = new List<AiInteractionLog>();
}
