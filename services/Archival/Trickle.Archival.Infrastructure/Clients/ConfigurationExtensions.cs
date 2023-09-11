using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace Trickle.Archival.Infrastructure.Clients;

internal static class ConfigurationExtensions
{
    public static void AddClients(this IServiceCollection services)
    {
        services.AddHttpClient<IHttpContentClient, HttpContentClient>()
            .AddTransientHttpErrorPolicy(b => b.WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10)
            }));
    }
}
