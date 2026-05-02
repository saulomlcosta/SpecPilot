using System.Text.Json;
using SpecPilot.Domain.Entities;
using SpecPilot.Domain.Enums;

namespace SpecPilot.Application.Projects.Common;

public static class ProjectDocumentMappings
{
    public static ProjectDocumentResponse ToResponse(this ProjectDocument document)
    {
        return new ProjectDocumentResponse
        {
            ProjectId = document.ProjectId,
            Status = ProjectStatus.DocumentGenerated.ToString(),
            Overview = document.Overview,
            FunctionalRequirements = DeserializeList(document.FunctionalRequirements),
            NonFunctionalRequirements = DeserializeList(document.NonFunctionalRequirements),
            UseCases = DeserializeList(document.UseCases),
            Risks = DeserializeList(document.Risks)
        };
    }

    public static string SerializeList(IReadOnlyList<string> values)
    {
        return JsonSerializer.Serialize(values);
    }

    private static IReadOnlyList<string> DeserializeList(string value)
    {
        return JsonSerializer.Deserialize<List<string>>(value) ?? [];
    }
}
