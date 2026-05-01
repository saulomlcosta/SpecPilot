using SpecPilot.Domain.Common;

namespace SpecPilot.Domain.Entities;

public class RefinementQuestion : BaseEntity
{
    public Guid ProjectId { get; set; }
    public int Order { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string? AnswerText { get; set; }
    public DateTime? AnsweredAtUtc { get; set; }
    public Project? Project { get; set; }
}
