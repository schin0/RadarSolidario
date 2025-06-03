using Infra.Interfaces;
using Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Core.Extensions;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddInfoRepository(this IServiceCollection services)
    {
        services.AddScoped<IInfoRepository, InfoRepository>();
        return services;
    }
}
