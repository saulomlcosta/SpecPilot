using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        services.Configure<JwtOptions>(options =>
            configuration.GetSection(JwtOptions.SectionName).Bind(options));

        return services;
    }
}
