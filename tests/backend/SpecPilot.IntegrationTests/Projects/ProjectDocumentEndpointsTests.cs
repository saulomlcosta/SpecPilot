using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpecPilot.Domain.Entities;
using SpecPilot.Domain.Enums;
using SpecPilot.Infrastructure.Persistence;
using SpecPilot.IntegrationTests.Auth;
using SpecPilot.IntegrationTests.Infrastructure;

namespace SpecPilot.IntegrationTests.Projects;

public class ProjectDocumentEndpointsTests : IClassFixture<SpecPilotApiFactory>
{
    private readonly SpecPilotApiFactory _factory;

    public ProjectDocumentEndpointsTests(SpecPilotApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Should_generate_document_after_answering_questions()
    {
        var client = await CreateAuthenticatedClientAsync();
        var projectId = await CreateProjectAsync(client);
        var questions = await GenerateQuestionsAsync(client, projectId);
        await AnswerQuestionsAsync(client, projectId, questions);

        var response = await client.PostAsync($"/api/projects/{projectId}/generate-document", content: null);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<ProjectDocumentResponseContract>();
        body.Should().NotBeNull();
        body!.ProjectId.Should().Be(projectId);
        body.Status.Should().Be("DocumentGenerated");
        body.FunctionalRequirements.Should().NotBeEmpty();

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<SpecPilotDbContext>();
        (await context.ProjectDocuments.AsNoTracking().CountAsync(x => x.ProjectId == projectId)).Should().Be(1);
        (await context.AiInteractionLogs.AsNoTracking().CountAsync(x => x.ProjectId == projectId && x.InteractionType == "GenerateProjectDocument")).Should().Be(1);
        (await context.Projects.AsNoTracking().SingleAsync(x => x.Id == projectId)).Status.Should().Be(ProjectStatus.DocumentGenerated);
    }

    [Fact]
    public async Task Should_get_generated_document()
    {
        var client = await CreateAuthenticatedClientAsync();
        var projectId = await CreateProjectAsync(client);
        var questions = await GenerateQuestionsAsync(client, projectId);
        await AnswerQuestionsAsync(client, projectId, questions);
        await client.PostAsync($"/api/projects/{projectId}/generate-document", content: null);

        var response = await client.GetAsync($"/api/projects/{projectId}/document");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<ProjectDocumentResponseContract>();
        body.Should().NotBeNull();
        body!.ProjectId.Should().Be(projectId);
        body.Overview.Should().NotBeNullOrWhiteSpace();
        body.UseCases.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Should_return_conflict_when_generating_document_before_answering_questions()
    {
        var client = await CreateAuthenticatedClientAsync();
        var projectId = await CreateProjectAsync(client);
        await client.PostAsync($"/api/projects/{projectId}/generate-questions", content: null);

        using var beforeScope = _factory.Services.CreateScope();
        var beforeContext = beforeScope.ServiceProvider.GetRequiredService<SpecPilotDbContext>();
        var previousLogCount = await beforeContext.AiInteractionLogs
            .AsNoTracking()
            .CountAsync(x => x.ProjectId == projectId && x.InteractionType == "GenerateProjectDocument");

        var response = await client.PostAsync($"/api/projects/{projectId}/generate-document", content: null);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsContract>();
        problem.Should().NotBeNull();
        problem!.Extensions["code"].GetString().Should().Be("projects.invalid_status_for_document_generation");

        using var afterScope = _factory.Services.CreateScope();
        var afterContext = afterScope.ServiceProvider.GetRequiredService<SpecPilotDbContext>();
        var currentLogCount = await afterContext.AiInteractionLogs
            .AsNoTracking()
            .CountAsync(x => x.ProjectId == projectId && x.InteractionType == "GenerateProjectDocument");
        currentLogCount.Should().Be(previousLogCount);
    }

    [Fact]
    public async Task Should_return_not_found_when_getting_document_from_another_user()
    {
        var clientA = await CreateAuthenticatedClientAsync();
        var clientB = await CreateAuthenticatedClientAsync();
        var projectId = await CreateProjectAsync(clientA);
        var questions = await GenerateQuestionsAsync(clientA, projectId);
        await AnswerQuestionsAsync(clientA, projectId, questions);
        await clientA.PostAsync($"/api/projects/{projectId}/generate-document", content: null);

        var response = await clientB.GetAsync($"/api/projects/{projectId}/document");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsContract>();
        problem.Should().NotBeNull();
        problem!.Extensions["code"].GetString().Should().Be("projects.not_found");
    }

    [Fact]
    public async Task Should_return_not_found_when_document_has_not_been_generated()
    {
        var client = await CreateAuthenticatedClientAsync();
        var projectId = await CreateProjectAsync(client);

        var response = await client.GetAsync($"/api/projects/{projectId}/document");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsContract>();
        problem.Should().NotBeNull();
        problem!.Extensions["code"].GetString().Should().Be("projects.document_not_found");
    }

    [Fact]
    public async Task Should_complete_main_mvp_journey_with_fake_ai_service()
    {
        var client = _factory.CreateClient();
        var email = $"journey-{Guid.NewGuid():N}@example.com";

        var registerResponse = await client.PostAsJsonAsync("/api/auth/register", new
        {
            name = "Saulo",
            email,
            password = "12345678"
        });
        registerResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", new
        {
            email,
            password = "12345678"
        });
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var authBody = await loginResponse.Content.ReadFromJsonAsync<AuthResponseContract>();
        authBody.Should().NotBeNull();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authBody!.Token);

        var createProjectResponse = await client.PostAsJsonAsync("/api/projects", new
        {
            name = "Projeto Jornada",
            initialDescription = "Sistema para apoiar a descoberta de requisitos.",
            goal = "Gerar uma base tecnica inicial.",
            targetAudience = "Analistas e estudantes"
        });
        createProjectResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdProject = await createProjectResponse.Content.ReadFromJsonAsync<ProjectResponseContract>();
        createdProject.Should().NotBeNull();

        var generateQuestionsResponse = await client.PostAsync($"/api/projects/{createdProject!.Id}/generate-questions", content: null);
        generateQuestionsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var generatedQuestions = await LoadGeneratedQuestionsAsync(createdProject.Id);

        var answerQuestionsResponse = await client.PutAsJsonAsync($"/api/projects/{createdProject.Id}/questions/answers", new
        {
            answers = generatedQuestions.Select((question, index) => new
            {
                questionId = question.Id,
                answer = $"Resposta da jornada {index + 1}"
            })
        });
        answerQuestionsResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var generateDocumentResponse = await client.PostAsync($"/api/projects/{createdProject.Id}/generate-document", content: null);
        generateDocumentResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var getDocumentResponse = await client.GetAsync($"/api/projects/{createdProject.Id}/document");
        getDocumentResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var document = await getDocumentResponse.Content.ReadFromJsonAsync<ProjectDocumentResponseContract>();
        document.Should().NotBeNull();
        document!.ProjectId.Should().Be(createdProject.Id);
        document.Status.Should().Be("DocumentGenerated");
        document.Overview.Should().NotBeNullOrWhiteSpace();
        document.FunctionalRequirements.Should().NotBeEmpty();
        document.NonFunctionalRequirements.Should().NotBeEmpty();
        document.UseCases.Should().NotBeEmpty();
        document.Risks.Should().NotBeEmpty();

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<SpecPilotDbContext>();
        (await context.AiInteractionLogs.AsNoTracking().CountAsync(x => x.ProjectId == createdProject.Id)).Should().Be(2);
        (await context.Projects.AsNoTracking().SingleAsync(x => x.Id == createdProject.Id)).Status.Should().Be(ProjectStatus.DocumentGenerated);
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

        return await LoadGeneratedQuestionsAsync(projectId);
    }

    private async Task<List<RefinementQuestion>> LoadGeneratedQuestionsAsync(Guid projectId)
    {

        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<SpecPilotDbContext>();
        return await context.RefinementQuestions
            .AsNoTracking()
            .Where(x => x.ProjectId == projectId)
            .OrderBy(x => x.Order)
            .Select(x => new RefinementQuestion
            {
                Id = x.Id,
                ProjectId = x.ProjectId,
                Order = x.Order,
                QuestionText = x.QuestionText
            })
            .ToListAsync();
    }

    private static async Task AnswerQuestionsAsync(HttpClient client, Guid projectId, List<RefinementQuestion> questions)
    {
        using var response = await client.PutAsJsonAsync($"/api/projects/{projectId}/questions/answers", new
        {
            answers = questions.Select((question, index) => new
            {
                questionId = question.Id,
                answer = $"Resposta {index + 1}"
            })
        });

        response.EnsureSuccessStatusCode();
    }

    private sealed class AuthResponseContract
    {
        public string Token { get; set; } = string.Empty;
    }

    private sealed class ProjectResponseContract
    {
        public Guid Id { get; set; }
    }

    private sealed class ProjectDocumentResponseContract
    {
        public Guid ProjectId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Overview { get; set; } = string.Empty;
        public List<string> FunctionalRequirements { get; set; } = [];
        public List<string> NonFunctionalRequirements { get; set; } = [];
        public List<string> UseCases { get; set; } = [];
        public List<string> Risks { get; set; } = [];
    }
}
