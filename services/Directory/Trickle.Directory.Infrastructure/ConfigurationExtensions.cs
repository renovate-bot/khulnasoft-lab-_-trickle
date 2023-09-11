using Trickle.Directory.Infrastructure.Persistence.Commands.Context;
using Trickle.Directory.Infrastructure.Persistence.Queries.Context;
using Trickle.SharedKernel.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
#if DEBUG
using Microsoft.Extensions.Logging;
#endif

namespace Trickle.Directory.Infrastructure;

public static class ConfigurationExtensions
{
    public static IHostBuilder ConfigureInfrastructure(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureLogging();
    }

    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSharedKernelLogging(configuration);
        services.AddDbContextPool<QueryDbContext>(o =>
        {
            o.UseNpgsql(configuration.GetConnectionString("DirectoryConnection"),
                    po => po.MigrationsAssembly("Trickle.Directory.Infrastructure.Migrations"))
                .UseSnakeCaseNamingConvention()
#if DEBUG
                .LogTo(Console.WriteLine, LogLevel.Information);
            o.EnableSensitiveDataLogging();
#else
                ;
#endif
        });
        services.AddDbContextPool<CommandDbContext>(o =>
        {
            o.UseNpgsql(configuration.GetConnectionString("DirectoryConnection"))
                .UseSnakeCaseNamingConvention()
                .UseLazyLoadingProxies()
#if DEBUG
                .LogTo(Console.WriteLine, LogLevel.Information);
            o.EnableSensitiveDataLogging();
#else
                ;
#endif
        });
        services.AddScoped<IQueryContext, QueryContext>();
        services.AddScoped<ICommandContext, CommandDbContext>();
    }

    public static void UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseLogging();
    }
}
