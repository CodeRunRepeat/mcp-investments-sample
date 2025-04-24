using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

[McpServerToolType]
public static class PortfolioTools
{
    private static PortfolioValuationController _portfolioValuationController = default!;
    public static ILogger<PortfolioValuationController> Logger { get; private set; } = default!;

    public static void Configure(ILogger<PortfolioValuationController> logger)
    {
        _portfolioValuationController = new PortfolioValuationController(logger, new DataLoader(logger));
        Logger = logger;
    }

    [McpServerTool, Description("Get the current portfolio valuation.")]
    public static async Task<string> GetCurrentPortfolioValuation(
        [Description("The name of the portfolio to get valuation for")]string portfolioName)
    {
        Logger.LogInformation("Fetching current portfolio valuation.");

        var valuationResult = await _portfolioValuationController.GetCurrentValuation(portfolioName);

        if (valuationResult.Result is not null && valuationResult.Result is not OkObjectResult)
        {
            Logger.LogError($"Error fetching portfolio valuation: {valuationResult.Result?.ToString()} ({valuationResult.Result?.GetType()})");
            return $"Error: {valuationResult.Result?.ToString()}";
        }

        var value = valuationResult?.Value;
        var returnValue = value is not null
            ? JsonSerializer.Serialize(value)
            : "No portfolio valuation data found.";

        Logger.LogInformation($"Fetched portfolio valuation: {returnValue}");
        return returnValue;
    }
}