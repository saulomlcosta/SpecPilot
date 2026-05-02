using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpecPilot.Application.Abstractions.Auth;
using SpecPilot.Application.Abstractions.Persistence;
using SpecPilot.Application.Projects.AnswerQuestions.Models;
using SpecPilot.Domain.Common;
using SpecPilot.Domain.Enums;

namespace SpecPilot.Application.Projects.AnswerQuestions;

public class AnswerRefinementQuestionsCommandHandler
    : IRequestHandler<AnswerRefinementQuestionsCommand, Result<AnswerRefinementQuestionsResult>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly ILogger<AnswerRefinementQuestionsCommandHandler> _logger;

    public AnswerRefinementQuestionsCommandHandler(
        IApplicationDbContext context,
        ICurrentUserAccessor currentUserAccessor,
        ILogger<AnswerRefinementQuestionsCommandHandler> logger)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
        _logger = logger;
    }

    public async Task<Result<AnswerRefinementQuestionsResult>> Handle(
        AnswerRefinementQuestionsCommand request,
        CancellationToken cancellationToken)
    {
        if (_currentUserAccessor.UserId is null)
        {
            return Result.Failure<AnswerRefinementQuestionsResult>(Error.Unauthorized(
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
            _logger.LogWarning("Projeto nao encontrado para resposta de refinamento. ProjectId={ProjectId} UserId={UserId}", request.ProjectId, _currentUserAccessor.UserId);
            return Result.Failure<AnswerRefinementQuestionsResult>(Error.NotFound(
                "projects.not_found",
                "Projeto nao encontrado."));
        }

        if (project.Status != ProjectStatus.QuestionsGenerated)
        {
            _logger.LogWarning("Status invalido para resposta de refinamento. ProjectId={ProjectId} Status={Status}", project.Id, project.Status);
            return Result.Failure<AnswerRefinementQuestionsResult>(Error.Conflict(
                "projects.invalid_status_for_question_answering",
                "Somente projetos com status QuestionsGenerated podem receber respostas de refinamento."));
        }

        if (project.RefinementQuestions.Count == 0)
        {
            return Result.Failure<AnswerRefinementQuestionsResult>(Error.Conflict(
                "projects.questions_not_generated",
                "O projeto ainda nao possui perguntas de refinamento geradas."));
        }

        if (request.Answers.Count != project.RefinementQuestions.Count)
        {
            return Result.Failure<AnswerRefinementQuestionsResult>(Error.Validation(
                "projects.incomplete_answers",
                "E necessario responder todas as perguntas de refinamento."));
        }

        var answersByQuestionId = request.Answers
            .GroupBy(x => x.QuestionId)
            .ToDictionary(x => x.Key, x => x.Last().Answer);

        if (answersByQuestionId.Count != project.RefinementQuestions.Count ||
            project.RefinementQuestions.Any(x => !answersByQuestionId.ContainsKey(x.Id)))
        {
            return Result.Failure<AnswerRefinementQuestionsResult>(Error.Validation(
                "projects.incomplete_answers",
                "E necessario responder todas as perguntas de refinamento."));
        }

        var answeredAtUtc = DateTime.UtcNow;

        foreach (var question in project.RefinementQuestions)
        {
            question.AnswerText = answersByQuestionId[question.Id];
            question.AnsweredAtUtc = answeredAtUtc;
        }

        project.Status = ProjectStatus.QuestionsAnswered;
        project.UpdatedAtUtc = answeredAtUtc;

        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Perguntas respondidas com sucesso. ProjectId={ProjectId} UserId={UserId}", project.Id, project.UserId);

        return Result.Success(new AnswerRefinementQuestionsResult
        {
            ProjectId = project.Id,
            Status = project.Status.ToString()
        });
    }
}
