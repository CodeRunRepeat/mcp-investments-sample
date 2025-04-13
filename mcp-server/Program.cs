using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;


// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.File($"{Path.GetTempPath()}Logs/app-.log", rollingInterval: RollingInterval.Day) // Log to file with daily rolling
    .CreateLogger();

var builder = Host.CreateEmptyApplicationBuilder(settings: null);

builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

builder.Services.AddSerilog();

var app = builder.Build();

SecuritiesTools.Configure(app.Services.GetRequiredService<Microsoft.Extensions.Logging.ILogger<SecuritiesController>>());

await app.RunAsync();