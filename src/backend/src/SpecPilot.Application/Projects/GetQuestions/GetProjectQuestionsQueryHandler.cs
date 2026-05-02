using MediatR;
using Microsoft.EntityFrameworkCore;
using SpecPilot.Application.Abstractions.Auth;
using SpecPilot.Application.Abstractions.Persistence;
using SpecPilot.Application.Projects.GetQuestions.Models;
using SpecPilot.Domain.Common;

namespace SpecPilot.Application.Projects.GetQuestions;

public class GetProjectQuestionsQueryHandler
    : IRequestHandler<GetProjectQuestionsQuery, Result<GetProjectQuestionsResult>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public GetProjectQuestionsQueryHandler(
        IApplicationDbContext context,
        ICurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<Result<GetProjectQuestionsResult>> Handle(
        GetProjectQuestionsQuery request,
        CancellationToken cancellationToken)
    {
        if (_currentUserAccessor.UserId is null)
        {
            return Result.Failure<GetProjectQuestionsResult>(Error.Unauthorized(
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
            return Result.Failure<GetProjectQuestionsResult>(Error.NotFound(
                "projects.not_found",
                "Projeto nao encontrado."));
        }

        if (project.RefinementQuestions.Count == 0)
        {
            return Result.Failure<GetProjectQuestionsResult>(Error.NotFound(
                "projects.questions_not_found",
                "Perguntas de refinamento nao encontradas para este projeto."));
        }

        return Result.Success(new GetProjectQuestionsResult
        {
            ProjectId = project.Id,
            Status = project.Status.ToString(),
            Questions = project.RefinementQuestions
                .OrderBy(x => x.Order)
                .Select(x => new ProjectQuestionResponse
                {
                    Id = x.Id,
                    Order = x.Order,
                    QuestionText = x.QuestionText,
                    Answer = x.AnswerText
                })
                .ToList()
        });
    }
}
