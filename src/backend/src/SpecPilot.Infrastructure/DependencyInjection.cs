using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SpecPilot.Application.Abstractions.Ai;
using SpecPilot.Application.Abstractions.Auth;
using SpecPilot.Application.Abstractions.Persistence;
using SpecPilot.Infrastructure.Ai;
using SpecPilot.Infrastructure.Authentication;
using SpecPilot.Infrastructure.Persistence;

namespace SpecPilot.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SpecPilotDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? configuration["DATABASE_URL"]
                ?? "Host=localhost;Port=5432;Database=specpilot;Username=specpilot;Password=specpilot";

            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<SpecPilotDbContext>());
        services.AddHttpContextAccessor();
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
        services.Configure<AiOptions>(options =>
        {
            configuration.GetSection(AiOptions.SectionName).Bind(options);

            if (string.IsNullOrWhiteSpace(options.Provider))
            {
                options.Provider = "Fake";
            }
        });
        services.AddScoped<IAiService>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<AiOptions>>().Value;

            return options.Provider.Trim().ToLowerInvariant() switch
            {
                "fake" => new FakeAiService(),
                _ => new FakeAiService()
            };
        });

        services.Configure<JwtOptions>(options =>
            configuration.GetSection(JwtOptions.SectionName).Bind(options));

        return services;
    }
}
