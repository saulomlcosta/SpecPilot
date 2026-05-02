using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpecPilot.Application.Abstractions.Ai;
using SpecPilot.Application.Abstractions.Auth;
using SpecPilot.Application.Abstractions.Persistence;
using SpecPilot.Application.Ai.Models;
using SpecPilot.Application.Projects.Common;
using SpecPilot.Domain.Common;
using SpecPilot.Domain.Entities;
using SpecPilot.Domain.Enums;

namespace SpecPilot.Application.Projects.GenerateDocument;

public class GenerateProjectDocumentCommandHandler
    : IRequestHandler<GenerateProjectDocumentCommand, Result<ProjectDocumentResponse>>
{
    private const string PromptName = "generate-project-document.costar.md";
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IAiService _aiService;
    private readonly ILogger<GenerateProjectDocumentCommandHandler> _logger;

    public GenerateProjectDocumentCommandHandler(
        IApplicationDbContext context,
        ICurrentUserAccessor currentUserAccessor,
        IAiService aiService,
        ILogger<GenerateProjectDocumentCommandHandler> logger)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
        _aiService = aiService;
        _logger = logger;
    }

    public async Task<Result<ProjectDocumentResponse>> Handle(
        GenerateProjectDocumentCommand request,
        CancellationToken cancellationToken)
    {
        if (_currentUserAccessor.UserId is null)
        {
            return Result.Failure<ProjectDocumentResponse>(Error.Unauthorized(
                "projects.unauthenticated",
                "Usuario nao autenticado."));
        }

        var project = await _context.Projects
            .Include(x => x.RefinementQuestions)
            .Include(x => x.ProjectDocument)
            .FirstOrDefaultAsync(
                x => x.Id == request.ProjectId && x.UserId == _currentUserAccessor.UserId.Value,
                cancellationToken);

        if (project is null)
        {
            _logger.LogWarning("Projeto nao encontrado para geracao de documento. ProjectId={ProjectId} UserId={UserId}", request.ProjectId, _currentUserAccessor.UserId);
            return Result.Failure<ProjectDocumentResponse>(Error.NotFound(
                "projects.not_found",
                "Projeto nao encontrado."));
        }

        if (project.Status != ProjectStatus.QuestionsAnswered)
        {
            _logger.LogWarning("Status invalido para geracao de documento. ProjectId={ProjectId} Status={Status}", project.Id, project.Status);
            return Result.Failure<ProjectDocumentResponse>(Error.Conflict(
                "projects.invalid_status_for_document_generation",
                "Somente projetos com status QuestionsAnswered podem gerar documento."));
        }

        if (project.ProjectDocument is not null)
        {
            return Result.Failure<ProjectDocumentResponse>(Error.Conflict(
                "projects.document_already_generated",
                "O documento do projeto ja foi gerado."));
        }

        var aiRequest = new GenerateProjectDocumentRequest
        {
            ProjectName = project.Name,
            InitialDescription = project.InitialDescription,
            Goal = project.Goal,
            TargetAudience = project.TargetAudience,
            Answers = project.RefinementQuestions
                .OrderBy(x => x.Order)
                .Select(x => new RefinementAnswerInput
                {
                    Question = x.QuestionText,
                    Answer = x.AnswerText ?? string.Empty
                })
                .ToList()
        };

        var aiResponse = await _aiService.GenerateProjectDocumentAsync(aiRequest, cancellationToken);
        _logger.LogInformation("Documento gerado com sucesso. ProjectId={ProjectId} UserId={UserId} Provider={Provider} Model={Model}", project.Id, project.UserId, aiResponse.Metadata?.Provider ?? _aiService.GetType().Name, aiResponse.Metadata?.Model ?? string.Empty);

        var document = new ProjectDocument
        {
            ProjectId = project.Id,
            Overview = aiResponse.Overview,
            FunctionalRequirements = ProjectDocumentMappings.SerializeList(aiResponse.FunctionalRequirements),
            NonFunctionalRequirements = ProjectDocumentMappings.SerializeList(aiResponse.NonFunctionalRequirements),
            UseCases = ProjectDocumentMappings.SerializeList(aiResponse.UseCases),
            Risks = ProjectDocumentMappings.SerializeList(aiResponse.Risks)
        };

        _context.ProjectDocuments.Add(document);

        _context.AiInteractionLogs.Add(new AiInteractionLog
        {
            ProjectId = project.Id,
            InteractionType = "GenerateProjectDocument",
            Provider = aiResponse.Metadata?.Provider ?? _aiService.GetType().Name,
            PromptName = PromptName,
            InputPayload = JsonSerializer.Serialize(new
            {
                Request = aiRequest,
                Model = aiResponse.Metadata?.Model,
                Prompt = aiResponse.Metadata?.RenderedPrompt,
                Metadata = aiResponse.Metadata?.AdditionalData
            }),
            OutputPayload = JsonSerializer.Serialize(new
            {
                Response = new
                {
                    aiResponse.Overview,
                    aiResponse.FunctionalRequirements,
                    aiResponse.NonFunctionalRequirements,
                    aiResponse.UseCases,
                    aiResponse.Risks
                },
                RawResponse = aiResponse.Metadata?.RawResponse,
                FinishReason = aiResponse.Metadata?.FinishReason,
                Metadata = aiResponse.Metadata?.AdditionalData
            }),
            IsSuccessful = true
        });

        project.Status = ProjectStatus.DocumentGenerated;
        project.UpdatedAtUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(document.ToResponse());
    }
}
