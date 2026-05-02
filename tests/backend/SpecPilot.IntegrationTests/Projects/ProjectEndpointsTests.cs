using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using SpecPilot.IntegrationTests.Auth;
using SpecPilot.IntegrationTests.Infrastructure;

namespace SpecPilot.IntegrationTests.Projects;

public class ProjectEndpointsTests : IClassFixture<SpecPilotApiFactory>
{
    private readonly SpecPilotApiFactory _factory;

    public ProjectEndpointsTests(SpecPilotApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Projects_should_require_authentication()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/projects");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Create_should_create_project_for_authenticated_user()
    {
        var client = await CreateAuthenticatedClientAsync();

        var response = await client.PostAsJsonAsync("/api/projects", new
        {
            name = "Projeto A",
            initialDescription = "Descricao A",
            goal = "Objetivo A",
            targetAudience = "Publico A"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<ProjectResponseContract>();
        body.Should().NotBeNull();
        body!.Status.Should().Be("Draft");
    }

    [Fact]
    public async Task List_should_return_only_projects_from_authenticated_user()
    {
        var clientA = await CreateAuthenticatedClientAsync();
        var clientB = await CreateAuthenticatedClientAsync();

        await clientA.PostAsJsonAsync("/api/projects", new
        {
            name = "Projeto A",
            initialDescription = "Descricao A",
            goal = "Objetivo A",
            targetAudience = "Publico A"
        });

        await clientB.PostAsJsonAsync("/api/projects", new
        {
            name = "Projeto B",
            initialDescription = "Descricao B",
            goal = "Objetivo B",
            targetAudience = "Publico B"
        });

        var response = await clientA.GetAsync("/api/projects");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<List<ProjectResponseContract>>();
        body.Should().NotBeNull();
        body.Should().HaveCount(1);
        body![0].Name.Should().Be("Projeto A");
    }

    [Fact]
    public async Task Get_by_id_should_return_owned_project()
    {
        var client = await CreateAuthenticatedClientAsync();
        var createdProject = await CreateProjectAsync(client, "Projeto A");

        var response = await client.GetAsync($"/api/projects/{createdProject.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<ProjectResponseContract>();
        body.Should().NotBeNull();
        body!.Id.Should().Be(createdProject.Id);
    }

    [Fact]
    public async Task Get_by_id_should_return_not_found_for_project_from_another_user()
    {
        var clientA = await CreateAuthenticatedClientAsync();
        var clientB = await CreateAuthenticatedClientAsync();
        var createdProject = await CreateProjectAsync(clientA, "Projeto A");

        var response = await clientB.GetAsync($"/api/projects/{createdProject.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsContract>();
        problem.Should().NotBeNull();
        problem!.Status.Should().Be(404);
        problem.Extensions["code"].GetString().Should().Be("projects.not_found");
    }

    [Fact]
    public async Task Get_by_id_should_return_not_found_for_unknown_project()
    {
        var client = await CreateAuthenticatedClientAsync();

        var response = await client.GetAsync($"/api/projects/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_should_change_owned_project()
    {
        var client = await CreateAuthenticatedClientAsync();
        var createdProject = await CreateProjectAsync(client, "Projeto A");

        var response = await client.PutAsJsonAsync($"/api/projects/{createdProject.Id}", new
        {
            name = "Projeto Atualizado",
            initialDescription = "Descricao Atualizada",
            goal = "Objetivo Atualizado",
            targetAudience = "Publico Atualizado"
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<ProjectResponseContract>();
        body.Should().NotBeNull();
        body!.Name.Should().Be("Projeto Atualizado");
        body.InitialDescription.Should().Be("Descricao Atualizada");
        body.Goal.Should().Be("Objetivo Atualizado");
        body.TargetAudience.Should().Be("Publico Atualizado");
        body.Status.Should().Be("Draft");
    }

    [Fact]
    public async Task Update_should_not_allow_manual_status_change()
    {
        var client = await CreateAuthenticatedClientAsync();
        var createdProject = await CreateProjectAsync(client, "Projeto A");

        var response = await client.PutAsJsonAsync($"/api/projects/{createdProject.Id}", new
        {
            name = "Projeto Atualizado",
            initialDescription = "Descricao Atualizada",
            goal = "Objetivo Atualizado",
            targetAudience = "Publico Atualizado",
            status = "QuestionsGenerated"
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<ProjectResponseContract>();
        body.Should().NotBeNull();
        body!.Status.Should().Be("Draft");
    }

    [Fact]
    public async Task Create_should_return_bad_request_for_invalid_payload()
    {
        var client = await CreateAuthenticatedClientAsync();

        var response = await client.PostAsJsonAsync("/api/projects", new
        {
            name = "",
            initialDescription = "",
            goal = "",
            targetAudience = ""
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsContract>();
        problem.Should().NotBeNull();
        problem!.Status.Should().Be(400);
        problem.Extensions["code"].GetString().Should().Be("common.validation_error");
    }

    [Fact]
    public async Task Update_should_return_not_found_for_project_from_another_user()
    {
        var clientA = await CreateAuthenticatedClientAsync();
        var clientB = await CreateAuthenticatedClientAsync();
        var createdProject = await CreateProjectAsync(clientA, "Projeto A");

        var response = await clientB.PutAsJsonAsync($"/api/projects/{createdProject.Id}", new
        {
            name = "Projeto Atualizado",
            initialDescription = "Descricao Atualizada",
            goal = "Objetivo Atualizado",
            targetAudience = "Publico Atualizado"
        });

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_should_remove_owned_project()
    {
        var client = await CreateAuthenticatedClientAsync();
        var createdProject = await CreateProjectAsync(client, "Projeto A");

        var deleteResponse = await client.DeleteAsync($"/api/projects/{createdProject.Id}");
        var getResponse = await client.GetAsync($"/api/projects/{createdProject.Id}");

        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_should_return_not_found_for_project_from_another_user()
    {
        var clientA = await CreateAuthenticatedClientAsync();
        var clientB = await CreateAuthenticatedClientAsync();
        var createdProject = await CreateProjectAsync(clientA, "Projeto A");

        var response = await clientB.DeleteAsync($"/api/projects/{createdProject.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
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

    private static async Task<ProjectResponseContract> CreateProjectAsync(HttpClient client, string name)
    {
        var response = await client.PostAsJsonAsync("/api/projects", new
        {
            name,
            initialDescription = $"Descricao {name}",
            goal = $"Objetivo {name}",
            targetAudience = $"Publico {name}"
        });

        return (await response.Content.ReadFromJsonAsync<ProjectResponseContract>())!;
    }

    private sealed class AuthResponseContract
    {
        public string Token { get; set; } = string.Empty;
    }

    private sealed class ProjectResponseContract
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string InitialDescription { get; set; } = string.Empty;
        public string Goal { get; set; } = string.Empty;
        public string TargetAudience { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
