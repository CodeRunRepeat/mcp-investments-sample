using System.Text.Json;

public class DataLoader
{
    private readonly string _dataFolder = Path.Combine(AppContext.BaseDirectory, "Data");
    private readonly ILogger _logger;

    // Constructor to initialize the logger
    public DataLoader(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<T> LoadJsonDataAsync<T>(string fileName)
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
