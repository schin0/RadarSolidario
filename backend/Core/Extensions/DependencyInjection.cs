using Infra.Interfaces;
using Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfoRepository(this IServiceCollection services)
    {
        services.AddScoped<IInfoRepository, InfoRepository>();
        return services;
    }
}
