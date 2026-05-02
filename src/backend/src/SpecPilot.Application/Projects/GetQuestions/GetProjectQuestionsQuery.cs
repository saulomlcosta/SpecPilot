using MediatR;
using SpecPilot.Application.Projects.GetQuestions.Models;
using SpecPilot.Domain.Common;

namespace SpecPilot.Application.Projects.GetQuestions;

public record GetProjectQuestionsQuery(Guid ProjectId) : IRequest<Result<GetProjectQuestionsResult>>;
