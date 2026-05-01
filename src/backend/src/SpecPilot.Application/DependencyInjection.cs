using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SpecPilot.Application.Common;

namespace SpecPilot.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly));
        services.AddValidatorsFromAssembly(typeof(AssemblyMarker).Assembly);

        return services;
    }
}
