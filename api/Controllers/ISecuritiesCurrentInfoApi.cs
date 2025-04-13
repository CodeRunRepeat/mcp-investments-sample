using Microsoft.AspNetCore.Mvc;

public interface ISecuritiesCurrentInfoApi
{
    ActionResult<SecurityInfo> GetCurrentSecurityInfo(string tickerSymbol);
    ActionResult<IEnumerable<SecurityInfo>> GetSecuritiesBySector(string sectorName); // Retrieve securities within a specific sector
}

public class SecurityInfo
{
    public required string TickerSymbol { get; set; }
    public required string SecurityName { get; set; }
    public required string Sector { get; set; }
    public decimal CurrentPrice { get; set; }
    public decimal PriceChangePercent { get; set; }
    public long TradingVolume { get; set; }
    public DateTime LastUpdated { get; set; }
}
