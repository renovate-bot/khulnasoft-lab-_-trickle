using System.Text.Json.Serialization;
using Trickle.Archival.Api;
using Trickle.Archival.Application;
using Trickle.SharedKernel.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureApplication();
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication(builder.Configuration);

var app = builder.Build();

app.UseApplication();
app.UseExceptionHandler(app.Environment.IsDevelopment() ? "/error-local-development" : "/error");
app.UseSwagger();
app.MapControllers();
app.Use(async (context, next) =>
{
    context.Request.PathBase = "/api/archival";
    await next.Invoke();
});

await app.TryRunWithLoggingAsync();
