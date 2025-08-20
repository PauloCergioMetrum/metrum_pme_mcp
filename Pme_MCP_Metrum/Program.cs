using Microsoft.Extensions.Logging;
using ModelContextProtocol.AspNetCore;
using ModelContextProtocol.Server;
using Pme_MCP_Metrum.Api.Tools.Sources;
using Pme_MCP_Metrum.Application.Sources;
using Pme_MCP_Metrum.Application.Sources.UseCases;
using Pme_MCP_Metrum.Infrastructure.Persistence;
using Pme_MCP_Metrum.Infrastructure.Repositories;
using Pme_MCP_Metrum.Infrastructure.Repositories.Sources;



var transport = (Environment.GetEnvironmentVariable("MCP_TRANSPORT") ?? "stdio")
    .Trim().ToLowerInvariant();


if (transport == "http")
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();

    builder.Logging.ClearProviders();
    builder.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Trace);


    builder.Services.AddSingleton<SqlConnectionFactory>();
    builder.Services.AddScoped<ISourceRepository, SourceRepository>();
    builder.Services.AddScoped<ListSources>();




    builder.Services.AddMcpServer()
        .WithHttpTransport()
        .WithToolsFromAssembly(typeof(SourcesTools).Assembly);

    var app = builder.Build();
    app.MapMcp("/mcp");
    app.MapGet("/_health", () => Results.Ok(new { ok = true, ts = DateTimeOffset.UtcNow }));
    app.Run();
    return;
}


var host = Host.CreateApplicationBuilder(args);

host.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

host.Logging.ClearProviders();
host.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Trace);


host.Services.AddSingleton<SqlConnectionFactory>();
host.Services.AddScoped<ISourceRepository, SourceRepository>();
host.Services.AddScoped<ListSources>();

host.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly(typeof(SourcesTools).Assembly);

await host.Build().RunAsync();
