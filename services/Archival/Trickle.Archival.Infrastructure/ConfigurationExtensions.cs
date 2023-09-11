using Trickle.Archival.Infrastructure.Clients;
using Trickle.Archival.Infrastructure.Persistence;
using Trickle.Archival.Infrastructure.Scheduling;
using Trickle.Directory.Api.Contracts;
using Trickle.SharedKernel.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Trickle.Archival.Infrastructure;

public static class ConfigurationExtensions
{
    public static IHostBuilder ConfigureInfrastructure(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureLogging();
    }

    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSharedKernelLogging(configuration);
        services.AddScheduling(configuration);
        services.AddDirectoryApiClient(configuration);
        services.AddClients();
        services.AddPersistence(configuration);
    }

    public static void UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseLogging();
    }
}
