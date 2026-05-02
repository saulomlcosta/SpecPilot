namespace SpecPilot.Application.Ai.Models;

public class AiResponseMetadata
{
    public string Provider { get; init; } = string.Empty;
    public string Model { get; init; } = string.Empty;
    public string RenderedPrompt { get; init; } = string.Empty;
    public string RawResponse { get; init; } = string.Empty;
    public string? RequestId { get; init; }
    public string? FinishReason { get; init; }
    public IReadOnlyDictionary<string, string> AdditionalData { get; init; } = new Dictionary<string, string>();
}
