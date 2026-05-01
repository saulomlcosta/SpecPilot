using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;

namespace SpecPilot.IntegrationTests.Auth;

public class AuthEndpointsTests : IClassFixture<SpecPilotApiFactory>
{
    private readonly SpecPilotApiFactory _factory;

    public AuthEndpointsTests(SpecPilotApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Register_should_create_user()
    {
        var client = _factory.CreateClient();
        var email = $"saulo-register-{Guid.NewGuid():N}@example.com";

        var response = await client.PostAsJsonAsync("/api/auth/register", new
        {
            name = "Saulo",
            email,
            password = "12345678"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<AuthResponseContract>();
        body.Should().NotBeNull();
        body!.Token.Should().NotBeNullOrWhiteSpace();
        body.User.Email.Should().Be(email);
    }

    [Fact]
    public async Task Register_should_return_conflict_for_duplicated_email()
    {
        var client = _factory.CreateClient();
        var email = $"saulo-duplicate-{Guid.NewGuid():N}@example.com";

        await client.PostAsJsonAsync("/api/auth/register", new
        {
            name = "Saulo",
            email,
            password = "12345678"
        });

        var response = await client.PostAsJsonAsync("/api/auth/register", new
        {
            name = "Saulo 2",
            email,
            password = "12345678"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsContract>();
        problem.Should().NotBeNull();
        problem!.Status.Should().Be(409);
    }

    [Fact]
    public async Task Login_should_return_jwt_token()
    {
        var client = _factory.CreateClient();
        var email = $"saulo-login-{Guid.NewGuid():N}@example.com";

        await client.PostAsJsonAsync("/api/auth/register", new
        {
            name = "Saulo",
            email,
            password = "12345678"
        });

        var response = await client.PostAsJsonAsync("/api/auth/login", new
        {
            email,
            password = "12345678"
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<AuthResponseContract>();
        body.Should().NotBeNull();
        body!.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Login_should_return_unauthorized_for_invalid_credentials()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/auth/login", new
        {
            email = $"saulo-invalid-{Guid.NewGuid():N}@example.com",
            password = "12345678"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsContract>();
        problem.Should().NotBeNull();
        problem!.Status.Should().Be(401);
    }

    [Fact]
    public async Task Me_should_return_unauthorized_without_token()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/auth/me");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsContract>();
        problem.Should().NotBeNull();
        problem!.Status.Should().Be(401);
    }

    [Fact]
    public async Task Me_should_return_authenticated_user()
    {
        var client = _factory.CreateClient();
        var email = $"saulo-me-{Guid.NewGuid():N}@example.com";

        var registerResponse = await client.PostAsJsonAsync("/api/auth/register", new
        {
            name = "Saulo",
            email,
            password = "12345678"
        });

        var authBody = await registerResponse.Content.ReadFromJsonAsync<AuthResponseContract>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authBody!.Token);

        var response = await client.GetAsync("/api/auth/me");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<UserResponseContract>();
        body.Should().NotBeNull();
        body!.Email.Should().Be(email);
    }

    private sealed class AuthResponseContract
    {
        public string Token { get; set; } = string.Empty;
        public UserResponseContract User { get; set; } = new();
    }

    private sealed class UserResponseContract
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    private sealed class ProblemDetailsContract
    {
        public string Title { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
        public int? Status { get; set; }
    }
}
