using Pme_MCP_Metrum.Api.Tools.Alarms;
using Pme_MCP_Metrum.Api.Tools.Devices;
using Pme_MCP_Metrum.Api.Tools.PQEvents;
using Pme_MCP_Metrum.Api.Tools.SourceQuantityName;
using Pme_MCP_Metrum.Api.Tools.Sources;
using Pme_MCP_Metrum.Application.Alarms;
using Pme_MCP_Metrum.Application.DataLog2.UseCases;
using Pme_MCP_Metrum.Application.Devices;
using Pme_MCP_Metrum.Application.Devices.UseCases;
using Pme_MCP_Metrum.Application.PQEvents.UseCases;
using Pme_MCP_Metrum.Application.SourceQuantityName.UseCases;
using Pme_MCP_Metrum.Application.SourceQuantityNames;
using Pme_MCP_Metrum.Application.SourceQuantityNames.UseCases;
using Pme_MCP_Metrum.Application.Sources;
using Pme_MCP_Metrum.Application.Sources.UseCases;
using Pme_MCP_Metrum.Domain.Repositories;
using Pme_MCP_Metrum.Domain.Repositories.PQEvents;
using Pme_MCP_Metrum.Infrastructure.Persistence;
using Pme_MCP_Metrum.Infrastructure.Repositories.Alarms;
using Pme_MCP_Metrum.Infrastructure.Repositories.DataLogs2;
using Pme_MCP_Metrum.Infrastructure.Repositories.Devices;
using Pme_MCP_Metrum.Infrastructure.Repositories.PQEvents;
using Pme_MCP_Metrum.Infrastructure.Repositories.SourceQuantityNames;
using Pme_MCP_Metrum.Infrastructure.Repositories.Sources;
using static Pme_MCP_Metrum.Api.Tools.DataLog2.DataLog2Tools;

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
    builder.Services.AddScoped<IDataLog2Repository, DataLog2Repository>();
    builder.Services.AddScoped<IPQEventsRepository, PQEventRepository>();
    builder.Services.AddScoped<ISourceQuantityNameRepository, SourceQuantityNameRepository>();

    //IMPORTANTE ID
    builder.Services.AddScoped<ListSources>();
    builder.Services.AddScoped<ListDevices>();
    builder.Services.AddScoped<ListAlarms>();
    builder.Services.AddScoped<ListDataLog2>();
    builder.Services.AddScoped<ListPQEvents>();
    builder.Services.AddScoped<GetLatestPQEvent>();     
    builder.Services.AddScoped<CountPQEventsByDay>();
    builder.Services.AddScoped<ListSourceQuantityName>();

    builder.Services.AddScoped<SearchSources>();
    builder.Services.AddScoped<GetSourceById>();


    //IMPORTANTE 
    // MCP Server com as tools
    builder.Services.AddMcpServer()
        .WithHttpTransport()
        .WithToolsFromAssembly(typeof(SourcesTools).Assembly)
        .WithToolsFromAssembly(typeof(DevicesTools).Assembly)
        .WithToolsFromAssembly(typeof(AlarmsTools).Assembly)
       .WithToolsFromAssembly(typeof(DataLog2ListArgs).Assembly)
        .WithToolsFromAssembly(typeof(PQEventsTools).Assembly)
        .WithToolsFromAssembly(typeof(SourceQuantityNameTools).Assembly)
        .WithToolsFromAssembly(typeof(SourceQuantityNameTools).Assembly);


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
host.Services.AddScoped<IDataLog2Repository, DataLog2Repository>();
host.Services.AddScoped<IPQEventsRepository, PQEventRepository>();
host.Services.AddScoped<ISourceQuantityNameRepository, SourceQuantityNameRepository>();


//IMPORTANTE 
host.Services.AddScoped<ListSources>();
host.Services.AddScoped<ListDevices>();
host.Services.AddScoped<ListAlarms>();
host.Services.AddScoped<ListDataLog2>();
host.Services.AddScoped<ListPQEvents>();
host.Services.AddScoped<GetLatestPQEvent>();         
host.Services.AddScoped<CountPQEventsByDay>();
host.Services.AddScoped<ListSourceQuantityName>();
host.Services.AddScoped<SearchSources>();
host.Services.AddScoped<GetSourceById>();


// MCP Server com as tools
host.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly(typeof(SourcesTools).Assembly)
    .WithToolsFromAssembly(typeof(DevicesTools).Assembly)
    .WithToolsFromAssembly(typeof(AlarmsTools).Assembly)
    .WithToolsFromAssembly(typeof(DataLog2ListArgs).Assembly)
  .WithToolsFromAssembly(typeof(PQEventsTools).Assembly);


await host.Build().RunAsync();
