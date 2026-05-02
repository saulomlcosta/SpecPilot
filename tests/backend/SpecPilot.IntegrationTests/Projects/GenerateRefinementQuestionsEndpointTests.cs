using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpecPilot.Domain.Enums;
using SpecPilot.Infrastructure.Persistence;
using SpecPilot.IntegrationTests.Auth;
using SpecPilot.IntegrationTests.Infrastructure;

namespace SpecPilot.IntegrationTests.Projects;

public class GenerateRefinementQuestionsEndpointTests : IClassFixture<SpecPilotApiFactory>
{
    private readonly SpecPilotApiFactory _factory;

    public GenerateRefinementQuestionsEndpointTests(SpecPilotApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Should_generate_questions_for_draft_project()
    {
        var client = await CreateAuthenticatedClientAsync();
        var projectId = await CreateProjectAsync(client);

        var response = await client.PostAsync($"/api/projects/{projectId}/generate-questions", content: null);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<GenerateQuestionsResponseContract>();
        body.Should().NotBeNull();
        body!.Status.Should().Be("QuestionsGenerated");
        body.Questions.Should().HaveCount(5);

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<SpecPilotDbContext>();
        (await context.RefinementQuestions.AsNoTracking().CountAsync(x => x.ProjectId == projectId)).Should().Be(5);
        (await context.AiInteractionLogs.AsNoTracking().CountAsync(x => x.ProjectId == projectId)).Should().Be(1);
        (await context.Projects.AsNoTracking().SingleAsync(x => x.Id == projectId)).Status.Should().Be(ProjectStatus.QuestionsGenerated);
    }

    [Fact]
    public async Task Should_return_not_found_for_project_from_another_user()
    {
        var clientA = await CreateAuthenticatedClientAsync();
        var clientB = await CreateAuthenticatedClientAsync();
        var projectId = await CreateProjectAsync(clientA);

        var response = await clientB.PostAsync($"/api/projects/{projectId}/generate-questions", content: null);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsContract>();
        problem.Should().NotBeNull();
        problem!.Extensions["code"].GetString().Should().Be("projects.not_found");
    }

    [Fact]
    public async Task Should_return_conflict_when_project_is_not_in_draft_status()
    {
        var client = await CreateAuthenticatedClientAsync();
        var projectId = await CreateProjectAsync(client);

        await client.PostAsync($"/api/projects/{projectId}/generate-questions", content: null);
        var secondResponse = await client.PostAsync($"/api/projects/{projectId}/generate-questions", content: null);

        secondResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
        var problem = await secondResponse.Content.ReadFromJsonAsync<ProblemDetailsContract>();
        problem.Should().NotBeNull();
        problem!.Extensions["code"].GetString().Should().Be("projects.invalid_status_for_question_generation");
    }

    private async Task<HttpClient> CreateAuthenticatedClientAsync()
    {
        var client = _factory.CreateClient();
        var email = $"user-{Guid.NewGuid():N}@example.com";

        var registerResponse = await client.PostAsJsonAsync("/api/auth/register", new
        {
            name = "Saulo",
            email,
            password = "12345678"
        });

        var authBody = await registerResponse.Content.ReadFromJsonAsync<AuthResponseContract>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authBody!.Token);
        return client;
    }

    private static async Task<Guid> CreateProjectAsync(HttpClient client)
    {
        var response = await client.PostAsJsonAsync("/api/projects", new
        {
            name = "Projeto A",
            initialDescription = "Descricao A",
            goal = "Objetivo A",
            targetAudience = "Publico A"
        });

        var body = await response.Content.ReadFromJsonAsync<ProjectResponseContract>();
        return body!.Id;
    }

    private sealed class AuthResponseContract
    {
        public string Token { get; set; } = string.Empty;
    }

    private sealed class ProjectResponseContract
    {
        public Guid Id { get; set; }
    }

    private sealed class GenerateQuestionsResponseContract
    {
        public Guid ProjectId { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<string> Questions { get; set; } = [];
    }
}
