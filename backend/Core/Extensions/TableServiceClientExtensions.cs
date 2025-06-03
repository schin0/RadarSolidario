using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Core.Extensions;

[ExcludeFromCodeCoverage]
public static class TableServiceClientExtensions
{
    public static IServiceCollection AddTableServiceClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(sp =>
            new TableServiceClient(configuration.GetConnectionString("AzureTableStorage")));
        return services;
    }
}
