using Microsoft.Extensions.Logging;
using Moq;

[TestClass]
public class SecuritiesToolsTests
{
    private Mock<ILogger<SecuritiesController>> _mockLogger = new Mock<ILogger<SecuritiesController>>();
    
    [TestInitialize]
    public void Setup()
    {
       // Configure SecuritiesTools with the mocked logger
        SecuritiesTools.Configure(_mockLogger.Object);
    }

    [TestMethod]
    public async Task GetCurrentSecurityInfo_ValidTicker_ReturnsFormattedInfo()
    {
        // Arrange
        var tickerSymbol = "AAPL";

        // Act
        var result = await SecuritiesTools.GetCurrentSecurityInfo(tickerSymbol);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("AAPL"), "The result should contain the ticker symbol.");
    }

    [TestMethod]
    public async Task GetCurrentSecurityInfo_InvalidTicker_ReturnsError()
    {
        // Arrange
        var tickerSymbol = "INVALID";

        // Act
        var result = await SecuritiesTools.GetCurrentSecurityInfo(tickerSymbol);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.StartsWith("Error:"), "The result should indicate an error.");
    }
}