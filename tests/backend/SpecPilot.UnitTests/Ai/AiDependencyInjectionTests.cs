using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpecPilot.Application.Abstractions.Ai;
using SpecPilot.Infrastructure;
using SpecPilot.Infrastructure.Ai;

namespace SpecPilot.UnitTests.Ai;

public class AiDependencyInjectionTests
{
    [Fact]
    public void Should_register_fake_ai_service_when_provider_is_fake()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = "Host=localhost;Port=5432;Database=specpilot;Username=specpilot;Password=specpilot",
                ["Ai:Provider"] = "Fake"
            })
            .Build();

        services.AddInfrastructure(configuration);
        var provider = services.BuildServiceProvider();

        var aiService = provider.GetRequiredService<IAiService>();

        aiService.Should().BeOfType<FakeAiService>();
    }

    [Fact]
    public void Should_register_fake_ai_service_by_default_when_provider_is_missing()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = "Host=localhost;Port=5432;Database=specpilot;Username=specpilot;Password=specpilot"
            })
            .Build();

        services.AddInfrastructure(configuration);
        var provider = services.BuildServiceProvider();

        var aiService = provider.GetRequiredService<IAiService>();

        aiService.Should().BeOfType<FakeAiService>();
    }
}
