using SpecPilot.Application.Ai.Models;

namespace SpecPilot.Application.Abstractions.Ai;

public interface IAiService
{
    Task<GenerateRefinementQuestionsResponse> GenerateRefinementQuestionsAsync(
        GenerateRefinementQuestionsRequest request,
        CancellationToken cancellationToken = default);

    Task<GenerateProjectDocumentResponse> GenerateProjectDocumentAsync(
        GenerateProjectDocumentRequest request,
        CancellationToken cancellationToken = default);
}
