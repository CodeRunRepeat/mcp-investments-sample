using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PortfolioValuationController : ControllerBase, IPortfolioValuation
{
    private readonly ILogger<PortfolioValuationController> _logger;
    private readonly DataLoader _dataLoader;

    public PortfolioValuationController(ILogger<PortfolioValuationController> logger, DataLoader dataLoader)
    {
        _logger = logger;
        _dataLoader = dataLoader;
    }

    [HttpGet("current")]
    public async Task<ActionResult<PortfolioValuation>> GetCurrentValuation(string portfolioName)
    {
        _logger.LogInformation("Fetching current portfolio valuation.");

        try
        {
            // Use DataLoader to load the JSON data
            var valuation = (await _dataLoader
            .LoadJsonDataAsync<PortfolioValuation[]>("portfolioValuation.json"))
            ?.FirstOrDefault(v => v.PortfolioName.Equals(portfolioName, StringComparison.OrdinalIgnoreCase));

            if (valuation == null)
            {
                _logger.LogWarning($"Portfolio {portfolioName} valuation data not found.");
                return NotFound("Portfolio valuation data not found.");
            }

            _logger.LogInformation("Successfully fetched portfolio valuation.");
            return valuation; // Returns 200 OK with the valuation data
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching portfolio valuation.");
            return StatusCode(500, "An error occurred while fetching portfolio valuation.");
        }
    }
}
