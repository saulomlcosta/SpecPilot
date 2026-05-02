using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SpecPilot.IntegrationTests.Auth;

namespace SpecPilot.IntegrationTests.Infrastructure;

public class ExceptionThrowingEndpointTests : IClassFixture<SpecPilotApiFactory>
{
    private readonly SpecPilotApiFactory _factory;

    public ExceptionThrowingEndpointTests(SpecPilotApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Should_return_problem_details_for_unexpected_exception()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/_test/errors/unhandled");

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        response.Content.Headers.ContentType!.MediaType.Should().Be("application/problem+json");

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetailsContract>();
        problem.Should().NotBeNull();
        problem!.Title.Should().Be("Falha interna.");
        problem.Detail.ToLowerInvariant().Should().NotContain("stack");
        problem.Status.Should().Be(500);
    }
}
