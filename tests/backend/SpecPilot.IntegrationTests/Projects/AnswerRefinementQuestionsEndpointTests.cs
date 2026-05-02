using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SpecPilot.Domain.Entities;
using SpecPilot.Domain.Enums;
using SpecPilot.Infrastructure.Persistence;
using SpecPilot.IntegrationTests.Auth;
using SpecPilot.IntegrationTests.Infrastructure;

namespace SpecPilot.IntegrationTests.Projects;

public class AnswerRefinementQuestionsEndpointTests : IClassFixture<SpecPilotApiFactory>
{
    private readonly SpecPilotApiFactory _factory;

    public AnswerRefinementQuestionsEndpointTests(SpecPilotApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Should_answer_questions_for_owned_project()
    {
        var client = await CreateAuthenticatedClientAsync();
        var projectId = await CreateProjectAsync(client);
        var generatedQuestions = await GenerateQuestionsAsync(client, projectId);

        var response = await client.PutAsJsonAsync($"/api/projects/{projectId}/questions/answers", new
        {
            answers = generatedQuestions.Select((question, index) => new
            {
                questionId = question.Id,
                answer = $"Resposta {index + 1}"
            })
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<AnswerQuestionsResponseContract>();
        body.Should().NotBeNull();
        body!.ProjectId.Should().Be(projectId);
        body.Status.Should().Be("QuestionsAnswered");

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<SpecPilotDbContext>();
        context.Projects.Single(x => x.Id == projectId).Status.Should().Be(ProjectStatus.QuestionsAnswered);
        context.RefinementQuestions.Where(x => x.ProjectId == projectId).Should().OnlyContain(x => !string.IsNullOrWhiteSpace(x.AnswerText));
        context.RefinementQuestions.Where(x => x.ProjectId == projectId).Should().OnlyContain(x => x.AnsweredAtUtc.HasValue);
    }

    [Fact]
    public async Task Should_return_unauthorized_when_request_has_no_token()
    {
        var client = _factory.CreateClient();

        var response = await client.PutAsJsonAsync($"/api/projects/{Guid.NewGuid()}/questions/answers", new
        {
            answers = new[]
            {
                new
                {
                    questionId = Guid.NewGuid(),
                    answer = "Resposta"
                }
            }
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsContract>();
        problem.Should().NotBeNull();
        problem!.Detail.Should().Be("Token ausente ou invalido.");
    }

    [Fact]
    public async Task Should_return_not_found_for_project_from_another_user()
    {
        var clientA = await CreateAuthenticatedClientAsync();
        var clientB = await CreateAuthenticatedClientAsync();
        var projectId = await CreateProjectAsync(clientA);
        var generatedQuestions = await GenerateQuestionsAsync(clientA, projectId);

        var response = await clientB.PutAsJsonAsync($"/api/projects/{projectId}/questions/answers", new
        {
            answers = generatedQuestions.Select(question => new
            {
                questionId = question.Id,
                answer = "Resposta"
            })
        });

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsContract>();
        problem.Should().NotBeNull();
        problem!.Extensions["code"].GetString().Should().Be("projects.not_found");
    }

    [Fact]
    public async Task Should_return_conflict_when_project_has_no_generated_questions()
    {
        var client = await CreateAuthenticatedClientAsync();
        var projectId = await CreateProjectAsync(client);

        var response = await client.PutAsJsonAsync($"/api/projects/{projectId}/questions/answers", new
        {
            answers = new[]
            {
                new
                {
                    questionId = Guid.NewGuid(),
                    answer = "Resposta"
                }
            }
        });

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsContract>();
        problem.Should().NotBeNull();
        problem!.Extensions["code"].GetString().Should().Be("projects.invalid_status_for_question_answering");
    }

    [Fact]
    public async Task Should_return_bad_request_when_answers_are_incomplete()
    {
        var client = await CreateAuthenticatedClientAsync();
        var projectId = await CreateProjectAsync(client);
        var generatedQuestions = await GenerateQuestionsAsync(client, projectId);

        var response = await client.PutAsJsonAsync($"/api/projects/{projectId}/questions/answers", new
        {
            answers = new[]
            {
                new
                {
                    questionId = generatedQuestions[0].Id,
                    answer = "Resposta parcial"
                }
            }
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsContract>();
        problem.Should().NotBeNull();
        problem!.Extensions["code"].GetString().Should().Be("projects.incomplete_answers");
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

    private async Task<List<RefinementQuestion>> GenerateQuestionsAsync(HttpClient client, Guid projectId)
    {
        using var response = await client.PostAsync($"/api/projects/{projectId}/generate-questions", content: null);
        response.EnsureSuccessStatusCode();

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<SpecPilotDbContext>();
        return context.RefinementQuestions
            .Where(x => x.ProjectId == projectId)
            .OrderBy(x => x.Order)
            .Select(x => new RefinementQuestion
            {
                Id = x.Id,
                ProjectId = x.ProjectId,
                Order = x.Order,
                QuestionText = x.QuestionText
            })
            .ToList();
    }

    private sealed class AuthResponseContract
    {
        public string Token { get; set; } = string.Empty;
    }

    private sealed class ProjectResponseContract
    {
        public Guid Id { get; set; }
    }

    private sealed class AnswerQuestionsResponseContract
    {
        public Guid ProjectId { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
