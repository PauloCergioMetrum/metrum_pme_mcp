using Pme_MCP_Metrum.Api.Tools.Alarms;
using Pme_MCP_Metrum.Api.Tools.Devices;
using Pme_MCP_Metrum.Api.Tools.Sources;
using Pme_MCP_Metrum.Application.Alarms;
using Pme_MCP_Metrum.Application.Devices;
using Pme_MCP_Metrum.Application.Devices.UseCases;
using Pme_MCP_Metrum.Application.Sources;
using Pme_MCP_Metrum.Application.Sources.UseCases;
using Pme_MCP_Metrum.Infrastructure.Persistence;
using Pme_MCP_Metrum.Infrastructure.Repositories.Alarms;
using Pme_MCP_Metrum.Infrastructure.Repositories.Devices;
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
    builder.Logging.AddFile("logs/devices_tool_errors.txt");
    builder.Logging.SetMinimumLevel(LogLevel.Information);

    // Repositórios e UseCases
    builder.Services.AddSingleton<SqlConnectionFactory>();
    builder.Services.AddSingleton<SqlConnectionFactoryNetwork>();

    //IMPORTANTE ID

    builder.Services.AddScoped<ISourceRepository, SourceRepository>();
    builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
    builder.Services.AddScoped<IAlarmRepository, AlarmRepository>();

    //IMPORTANTE ID
    builder.Services.AddScoped<ListSources>();
    builder.Services.AddScoped<ListDevices>();
    builder.Services.AddScoped<ListAlarms>();

    //IMPORTANTE 
    // MCP Server com as tools
    builder.Services.AddMcpServer()
        .WithHttpTransport()
        .WithToolsFromAssembly(typeof(SourcesTools).Assembly)
        .WithToolsFromAssembly(typeof(DevicesTools).Assembly)
        .WithToolsFromAssembly(typeof(AlarmsTools).Assembly);

    var app = builder.Build();
    app.MapMcp("/mcp");
    app.MapGet("/_health", () => Results.Ok(new { ok = true, ts = DateTimeOffset.UtcNow }));
    app.Run();
    return;
}

// Modo STDIO (Claude Desktop ou CLI)
var host = Host.CreateApplicationBuilder(args);

host.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

host.Logging.ClearProviders();
host.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Trace);
host.Logging.AddFile("logs/devices_tool_errors.txt");
host.Logging.SetMinimumLevel(LogLevel.Information);

// Repositórios e UseCases
host.Services.AddSingleton<SqlConnectionFactory>();
host.Services.AddSingleton<SqlConnectionFactoryNetwork>();

host.Services.AddScoped<ISourceRepository, SourceRepository>();
host.Services.AddScoped<IDeviceRepository, DeviceRepository>();
host.Services.AddScoped<IAlarmRepository, AlarmRepository>();


//IMPORTANTE 
host.Services.AddScoped<ListSources>();
host.Services.AddScoped<ListDevices>();
host.Services.AddScoped<ListAlarms>();

// MCP Server com as tools
host.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly(typeof(SourcesTools).Assembly)
    .WithToolsFromAssembly(typeof(DevicesTools).Assembly)
    .WithToolsFromAssembly(typeof(AlarmsTools).Assembly);

await host.Build().RunAsync();
