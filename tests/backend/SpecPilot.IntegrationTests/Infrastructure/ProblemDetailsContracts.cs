using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpecPilot.IntegrationTests.Infrastructure;

internal sealed class ProblemDetailsContract
{
    public string Title { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public int? Status { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement> Extensions { get; set; } = [];
}
