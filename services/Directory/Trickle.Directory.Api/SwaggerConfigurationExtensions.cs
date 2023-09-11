using System.Reflection;
using Microsoft.OpenApi.Models;

namespace Trickle.Directory.Api;

internal static class SwaggerConfigurationExtensions
{
    public static void AddSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(o =>
        {
            o.SupportNonNullableReferenceTypes();

            o.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "Trickle Directory API",
                    Description = "An ASP.NET Core API serving the core Trickle information.",
                    Version = "v1",
                    //TermsOfService = "",
                    Contact = new OpenApiContact { Name = "Trickle", Url = new Uri("https://trickle.khulnasoft.com") },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://github.com/khulnasoft-lab/Trickle/blob/main/LICENSE")
                    }
                });

            // Swagger UI struggles with lookups of nested types represented by concatenated '+'
            o.CustomSchemaIds(t => t.FullName?.Replace("+", "."));

            var apiXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, apiXmlFile));

            var apiContractsXmlFile = $"{typeof(Contracts.ConfigurationExtensions).Assembly.GetName().Name}.xml";
            o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, apiContractsXmlFile));

            var applicationXmlFile = $"{typeof(Application.ConfigurationExtensions).Assembly.GetName().Name}.xml";
            o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, applicationXmlFile));
        });
    }

    public static void UseSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger(o =>
        {
            o.RouteTemplate = "{documentName}/swagger.json";
            o.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.Servers = new List<OpenApiServer>
            {
                new() { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}/api/directory" }
            });
        });
    }
}
