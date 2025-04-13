using Microsoft.AspNetCore.Mvc;

public interface ISectorInfoApi
{
    ActionResult<SectorPerformance> GetSectorPerformance(string sectorName);
}

public class SectorPerformance
{
    public required string SectorName { get; set; }
    public decimal PriceChangePercent { get; set; }
    public decimal AverageTradingVolume { get; set; }
    public DateTime LastUpdated { get; set; }
}

