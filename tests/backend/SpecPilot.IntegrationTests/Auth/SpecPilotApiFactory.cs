using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SpecPilot.Infrastructure.Persistence;
using System.Reflection;

namespace SpecPilot.IntegrationTests.Auth;

public class SpecPilotApiFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["UseHttpsRedirection"] = "false"
            });
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<SpecPilotDbContext>));
            services.RemoveAll(typeof(SpecPilotDbContext));

            services.AddDbContext<SpecPilotDbContext>(options =>
            {
                options.UseInMemoryDatabase(_databaseName);
            });

            services.Configure<Microsoft.AspNetCore.Mvc.MvcOptions>(_ => { });
            services.AddControllers()
                .PartManager.ApplicationParts.Add(new AssemblyPart(typeof(SpecPilotApiFactory).Assembly));
        });
    }
}
