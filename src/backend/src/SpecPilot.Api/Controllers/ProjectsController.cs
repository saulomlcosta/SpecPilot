using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpecPilot.Api.Extensions;
using SpecPilot.Application.Projects.Create;
using SpecPilot.Application.Projects.Delete;
using SpecPilot.Application.Projects.GetById;
using SpecPilot.Application.Projects.List;
using SpecPilot.Application.Projects.Update;

namespace SpecPilot.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProjectCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return this.ToActionResult(result, response => CreatedAtAction(nameof(GetById), new { id = response.Id }, response));
    }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProjectsQuery(), cancellationToken);
        return this.ToActionResult(result, Ok);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetProjectByIdQuery(id), cancellationToken);
        return this.ToActionResult(result, Ok);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectCommand command, CancellationToken cancellationToken)
    {
        var request = new UpdateProjectCommand
        {
            Id = id,
            Name = command.Name,
            InitialDescription = command.InitialDescription,
            Goal = command.Goal,
            TargetAudience = command.TargetAudience,
            Status = command.Status
        };

        var result = await _mediator.Send(request, cancellationToken);
        return this.ToActionResult(result, Ok);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteProjectCommand(id), cancellationToken);
        return this.ToActionResult(result, NoContent);
    }
}
