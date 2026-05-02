using System.Net;
using FluentAssertions;

namespace SpecPilot.IntegrationTests.Infrastructure;

public class CorsPolicyTests : IClassFixture<SpecPilot.IntegrationTests.Auth.SpecPilotApiFactory>
{
    private readonly SpecPilot.IntegrationTests.Auth.SpecPilotApiFactory _factory;

    public CorsPolicyTests(SpecPilot.IntegrationTests.Auth.SpecPilotApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Preflight_request_should_allow_local_frontend_origin_and_authorization_header()
    {
        var client = _factory.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Options, "/api/auth/login");
        request.Headers.Add("Origin", "http://localhost:3000");
        request.Headers.Add("Access-Control-Request-Method", "POST");
        request.Headers.Add("Access-Control-Request-Headers", "authorization,content-type");

        using var response = await client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        response.Headers.Should().Contain(header => header.Key == "Access-Control-Allow-Origin"
            && header.Value.Contains("http://localhost:3000"));
        response.Headers.Should().Contain(header => header.Key == "Access-Control-Allow-Headers"
            && header.Value.Any(value => value.Contains("authorization", StringComparison.OrdinalIgnoreCase)));
        response.Headers.Should().Contain(header => header.Key == "Access-Control-Allow-Methods"
            && header.Value.Any(value =>
                value.Contains("GET", StringComparison.OrdinalIgnoreCase)
                && value.Contains("POST", StringComparison.OrdinalIgnoreCase)
                && value.Contains("PUT", StringComparison.OrdinalIgnoreCase)
                && value.Contains("DELETE", StringComparison.OrdinalIgnoreCase)
                && value.Contains("OPTIONS", StringComparison.OrdinalIgnoreCase)));
    }
}
