using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class SecuritiesController : ControllerBase
{
    private readonly ILogger<SecuritiesController> _logger;
    public SecuritiesController(ILogger<SecuritiesController> logger)
    {
        _logger = logger;
    }
    private readonly string _dataFolder = Path.Combine(AppContext.BaseDirectory, "Data");

    // ISecuritiesCurrentInfoApi implementation
    [HttpGet("current/{tickerSymbol}")]
    public async Task<ActionResult<SecurityInfo>> GetCurrentSecurityInfo(string tickerSymbol)
    {
        _logger.LogInformation($"Fetching current security info for ticker symbol: {tickerSymbol}");

        var securities = await LoadJsonDataAsync<List<SecurityInfo>>("securityInfo.json");
        var security = securities.FirstOrDefault(s => s.TickerSymbol.Equals(tickerSymbol, StringComparison.OrdinalIgnoreCase));
        if (security == null)
        {
            return NotFound($"Security with ticker symbol '{tickerSymbol}' not found.");
        }

        _logger.LogInformation($"Found security: {security.SecurityName} ({security.TickerSymbol}) in sector {security.Sector}");
        return security; // Returns 200 OK with the SecurityInfo object
    }

    [HttpGet("sector/{sectorName}")]
    public async Task<ActionResult<IEnumerable<SecurityInfo>>> GetSecuritiesBySector(string sectorName)
    {
        var securities = await LoadJsonDataAsync<List<SecurityInfo>>("securityInfo.json");
        var sectorSecurities = securities.Where(s => s.Sector.Equals(sectorName, StringComparison.OrdinalIgnoreCase)).ToList();
        if (!sectorSecurities.Any())
        {
            return NotFound($"No securities found for sector '{sectorName}'.");
        }
        return sectorSecurities; // Returns 200 OK with the list of securities
    }

    // ISecuritiesHistoricalDataApi implementation
    [HttpGet("historical/{tickerSymbol}")]
    public async Task<ActionResult<HistoricalData>> GetHistoricalData(string tickerSymbol, DateTime startDate, DateTime endDate)
    {
        var historicalData = await LoadJsonDataAsync<List<HistoricalData>>("historicalData.json");
        var data = historicalData.FirstOrDefault(h => h.TickerSymbol.Equals(tickerSymbol, StringComparison.OrdinalIgnoreCase));
        if (data == null)
        {
            return NotFound($"Historical data for ticker symbol '{tickerSymbol}' not found.");
        }

        data.Records = data.Records.Where(r => r.Date >= startDate && r.Date <= endDate).ToList();
        if (!data.Records.Any())
        {
            return NotFound($"No historical records found for ticker symbol '{tickerSymbol}' in the specified date range.");
        }
        return data; // Returns 200 OK with the HistoricalData object
    }

    // ISectorInfoApi implementation
    [HttpGet("sector-performance/{sectorName}")]
    public async Task<ActionResult<SectorPerformance>> GetSectorPerformance(string sectorName)
    {
        var sectors = await LoadJsonDataAsync<List<SectorPerformance>>("sectorPerformance.json");
        var sector = sectors.FirstOrDefault(s => s.SectorName.Equals(sectorName, StringComparison.OrdinalIgnoreCase));
        if (sector == null)
        {
            return NotFound($"Sector performance for '{sectorName}' not found.");
        }
        return sector; // Returns 200 OK with the SectorPerformance object
    }

    // Helper method to load JSON data asynchronously
    private async Task<T> LoadJsonDataAsync<T>(string fileName)
    {
        _logger.LogInformation($"Loading data from file: {fileName}");
        var filePath = Path.Combine(_dataFolder, fileName);
        if (!System.IO.File.Exists(filePath))
        {
            throw new FileNotFoundException($"Data file '{fileName}' not found in '{_dataFolder}'.");
        }

        var jsonData = await System.IO.File.ReadAllTextAsync(filePath);
        _logger.LogInformation($"Loaded {jsonData.Length} characters from '{fileName}'.");
        return JsonSerializer.Deserialize<T>(jsonData) ?? throw new InvalidOperationException($"Failed to deserialize data from '{fileName}'.");
    }
}