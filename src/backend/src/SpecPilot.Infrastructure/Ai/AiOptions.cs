namespace SpecPilot.Infrastructure.Ai;

public class AiOptions
{
    public const string SectionName = "Ai";

    public string Provider { get; set; } = "Fake";
    public string? PromptsRoot { get; set; }
    public OpenAiOptions OpenAi { get; set; } = new();
}
