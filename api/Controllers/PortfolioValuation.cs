using Microsoft.AspNetCore.Mvc;

public interface IPortfolioValuation
{
    Task<ActionResult<PortfolioValuation>> GetCurrentValuation(string portfolioName);
}
public class PortfolioValuation
{
    public required string PortfolioName { get; set; } // Name of the portfolio
    public required string CurrencyCode { get; set; } // Currency code (e.g., USD, EUR)
    public decimal CurrentMarketValue { get; set; } // Current market value of the portfolio
    public decimal CostBasis { get; set; } // Total cost basis of the portfolio
    public decimal UnrealizedGains { get; set; } // Unrealized gains or losses
    public decimal RealizedGains { get; set; } // Realized gains or losses
}