using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpecPilot.Application.Abstractions.Ai;
using SpecPilot.Application.Abstractions.Auth;
using SpecPilot.Application.Abstractions.Persistence;
using SpecPilot.Application.Ai.Models;
using SpecPilot.Application.Projects.GenerateQuestions.Models;
using SpecPilot.Domain.Common;
using SpecPilot.Domain.Entities;
using SpecPilot.Domain.Enums;

namespace SpecPilot.Application.Projects.GenerateQuestions;

public class GenerateRefinementQuestionsCommandHandler
    : IRequestHandler<GenerateRefinementQuestionsCommand, Result<GenerateRefinementQuestionsResult>>
{
    private const string PromptName = "generate-refinement-questions.costar.md";
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IAiService _aiService;
    private readonly ILogger<GenerateRefinementQuestionsCommandHandler> _logger;

    public GenerateRefinementQuestionsCommandHandler(
        IApplicationDbContext context,
        ICurrentUserAccessor currentUserAccessor,
        IAiService aiService,
        ILogger<GenerateRefinementQuestionsCommandHandler> logger)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
        _aiService = aiService;
        _logger = logger;
    }

    public async Task<Result<GenerateRefinementQuestionsResult>> Handle(
        GenerateRefinementQuestionsCommand request,
        CancellationToken cancellationToken)
    {
        if (_currentUserAccessor.UserId is null)
        {
            return Result.Failure<GenerateRefinementQuestionsResult>(Error.Unauthorized(
                "projects.unauthenticated",
                "Usuario nao autenticado."));
        }

        var project = await _context.Projects
            .Include(x => x.RefinementQuestions)
            .FirstOrDefaultAsync(
                x => x.Id == request.ProjectId && x.UserId == _currentUserAccessor.UserId.Value,
                cancellationToken);

        if (project is null)
        {
            _logger.LogWarning("Projeto nao encontrado para geracao de perguntas. ProjectId={ProjectId} UserId={UserId}", request.ProjectId, _currentUserAccessor.UserId);
            return Result.Failure<GenerateRefinementQuestionsResult>(Error.NotFound(
                "projects.not_found",
                "Projeto nao encontrado."));
        }

        if (project.Status != ProjectStatus.Draft)
        {
            _logger.LogWarning("Status invalido para geracao de perguntas. ProjectId={ProjectId} Status={Status}", project.Id, project.Status);
            return Result.Failure<GenerateRefinementQuestionsResult>(Error.Conflict(
                "projects.invalid_status_for_question_generation",
                "Somente projetos com status Draft podem gerar perguntas de refinamento."));
        }

        var aiRequest = new GenerateRefinementQuestionsRequest
        {
            ProjectName = project.Name,
            InitialDescription = project.InitialDescription,
            Goal = project.Goal,
            TargetAudience = project.TargetAudience
        };

        var aiResponse = await _aiService.GenerateRefinementQuestionsAsync(aiRequest, cancellationToken);
        _logger.LogInformation("Perguntas de refinamento geradas. ProjectId={ProjectId} UserId={UserId} Provider={Provider} Model={Model}", project.Id, project.UserId, aiResponse.Metadata?.Provider ?? _aiService.GetType().Name, aiResponse.Metadata?.Model ?? string.Empty);

        var questions = aiResponse.Questions
            .Select((question, index) => new RefinementQuestion
            {
                ProjectId = project.Id,
                Order = index + 1,
                QuestionText = question
            })
            .ToList();

        _context.RefinementQuestions.AddRange(questions);

        _context.AiInteractionLogs.Add(new AiInteractionLog
        {
            ProjectId = project.Id,
            InteractionType = "GenerateRefinementQuestions",
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
                Response = aiResponse.Questions,
                RawResponse = aiResponse.Metadata?.RawResponse,
                FinishReason = aiResponse.Metadata?.FinishReason,
                Metadata = aiResponse.Metadata?.AdditionalData
            }),
            IsSuccessful = true
        });

        project.Status = ProjectStatus.QuestionsGenerated;
        project.UpdatedAtUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(new GenerateRefinementQuestionsResult
        {
            ProjectId = project.Id,
            Status = project.Status.ToString(),
            Questions = questions.Select(x => x.QuestionText).ToList()
        });
    }
}
