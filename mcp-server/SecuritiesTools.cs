using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

[McpServerToolType]
public static class SecuritiesTools
{
    private static SecuritiesController _securitiesController = default!;
    public static ILogger<SecuritiesController> Logger { get; private set; } = default!;
    public static void Configure(ILogger<SecuritiesController> logger)
    {
        _securitiesController = new SecuritiesController(logger, new DataLoader(logger));
        Logger = logger;
    }

    [McpServerTool, Description("Get the current information for a securities ticker symbol.")]
    public static async Task<string> GetCurrentSecurityInfo(
        [Description("The ticker symbol to get information for for.")] string tickerSymbol)
    {
        Logger.LogInformation($"Fetching current security info for ticker symbol: {tickerSymbol}");
        
        var securityInfo = await _securitiesController.GetCurrentSecurityInfo(tickerSymbol);

        if (securityInfo.Result is not null && securityInfo.Result is not OkObjectResult)
        {
            Logger.LogError($"Error fetching security info: {securityInfo.Result?.ToString()} ({securityInfo.Result?.GetType()})");
            return $"Error: {securityInfo.Result?.ToString()}";
        }

        var value = securityInfo.Value;
        var returnValue = value is not null
            ? FormatSecurityInfo(value)
            : "No data found for the specified ticker symbol.";

        Logger.LogInformation($"Found security: {returnValue}");
        return returnValue;
    }

    [McpServerTool, Description("Get the current information for a securities sector.")]
    public static async Task<string> GetSecuritiesBySector(
        [Description("The sector to get information for.")] string sectorName)
    {
        var securities = await _securitiesController.GetSecuritiesBySector(sectorName);

        if (securities.Result is not null && securities.Result is not OkObjectResult)
        {
            return $"Error: {securities.Result?.ToString()}";
        }

        return securities.Value is not null
            ? $"[ {string.Join("\n", securities.Value.Select(s => FormatSecurityInfo(s)))} ]"
            : "No data found for the specified sector.";
    }

    [McpServerTool, Description("Get the historical data for a securities ticker symbol.")]
    public static async Task<string> GetHistoricalData(
        [Description("The ticker symbol to get historical data for.")] string tickerSymbol,
        [Description("The start date for the historical data.")] DateTime startDate,
        [Description("The end date for the historical data.")] DateTime endDate)
    {
        var historicalData = await _securitiesController.GetHistoricalData(tickerSymbol, startDate, endDate);

        if (historicalData.Result is not null && historicalData.Result is not OkObjectResult okResult)
        {
            return $"Error: {historicalData.Result?.ToString()}";
        }

        return historicalData.Value is not null
            ? $"[ {string.Join("\n", historicalData.Value.Records.Select(r =>
                JsonSerializer.Serialize(r)))} ]"
            : "No historical data found for the specified ticker symbol.";
    }

    [McpServerTool, Description("Get the performance of a sector.")]
    public static async Task<string> GetSectorPerformance(
        [Description("The sector to get performance for.")] string sectorName)
    {
        var sectorPerformance = await _securitiesController.GetSectorPerformance(sectorName);

        if (sectorPerformance.Result is not null && sectorPerformance.Result is not OkObjectResult)
        {
            return $"Error: {sectorPerformance.Result?.ToString()}";
        }

        return sectorPerformance.Value is not null
            ? JsonSerializer.Serialize(sectorPerformance.Value)
            : "No performance data found for the specified sector.";
    }

    private static string FormatSecurityInfo(SecurityInfo value)
    {
        return JsonSerializer.Serialize(value);
    }
}