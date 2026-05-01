using MediatR;
using SpecPilot.Application.Projects.GenerateQuestions.Models;
using SpecPilot.Domain.Common;

namespace SpecPilot.Application.Projects.GenerateQuestions;

public record GenerateRefinementQuestionsCommand(Guid ProjectId) : IRequest<Result<GenerateRefinementQuestionsResult>>;
