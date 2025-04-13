using Microsoft.AspNetCore.Mvc;

public interface ISecuritiesHistoricalDataApi
{
    ActionResult<HistoricalData> GetHistoricalData(string tickerSymbol, DateTime startDate, DateTime endDate);
}

public class HistoricalData
{
    public required string TickerSymbol { get; set; }
    public required IEnumerable<HistoricalRecord> Records { get; set; }
}

public class HistoricalRecord
{
    public DateTime Date { get; set; }
    public decimal OpenPrice { get; set; }
    public decimal ClosePrice { get; set; }
    public decimal HighPrice { get; set; }
    public decimal LowPrice { get; set; }
    public long TradingVolume { get; set; }
}
