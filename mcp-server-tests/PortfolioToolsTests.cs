using Microsoft.Extensions.Logging;
using Moq;

[TestClass]
public class PortfolioToolsTests
{
    private Mock<ILogger<PortfolioValuationController>> _mockLogger = new Mock<ILogger<PortfolioValuationController>>();

    [TestInitialize]
    public void Setup()
    {
        // Configure PortfolioTools with the mocked logger
        PortfolioTools.Configure(_mockLogger.Object);
    }

    [TestMethod]
    public async Task GetCurrentPortfolioValuation_ValidPortfolio_ReturnsFormattedInfo()
    {
        // Arrange
        var portfolioName = "Tech Growth Fund";

        // Act
        var result = await PortfolioTools.GetCurrentPortfolioValuation(portfolioName);

        // Assert
        Assert.IsNotNull(result, "The result should not be null.");
        Assert.IsTrue(result.Contains("Tech Growth Fund"), "The result should contain the portfolio name.");
    }

    [TestMethod]
    public async Task GetCurrentPortfolioValuation_InvalidPortfolio_ReturnsError()
    {
        // Arrange
        var portfolioName = "INVALID";

        // Act
        var result = await PortfolioTools.GetCurrentPortfolioValuation(portfolioName);

        // Assert
        Assert.IsNotNull(result, "The result should not be null.");
        Assert.IsTrue(result.StartsWith("Error:"), "The result should indicate an error.");
    }
}