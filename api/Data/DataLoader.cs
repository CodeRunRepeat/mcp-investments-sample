using System.Text.Json;

public static class DataLoader
{
    /// <summary>
    /// Loads JSON data from a file and deserializes it into a list of nullable objects of type T.
    /// Includes error handling for missing or empty files.
    /// </summary>
    public static IEnumerable<T?> LoadFromJsonFile<T>(string filePath) where T : class
    {
        try
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file '{filePath}' does not exist.");
            }

            string jsonData = File.ReadAllText(filePath);

            if (string.IsNullOrWhiteSpace(jsonData))
            {
                throw new InvalidDataException($"The file '{filePath}' is empty or contains invalid data.");
            }

            return JsonSerializer.Deserialize<IEnumerable<T?>>(jsonData) ?? [];
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return [];
        }
        catch (InvalidDataException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return [];
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error parsing JSON: {ex.Message}");
            return [];
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            return [];
        }
    }

    /// <summary>
    /// Loads a single nullable object from a JSON file.
    /// Includes error handling for missing or empty files.
    /// </summary>
    public static T? LoadSingleFromJsonFile<T>(string filePath) where T : class
    {
        try
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file '{filePath}' does not exist.");
            }

            string jsonData = File.ReadAllText(filePath);

            if (string.IsNullOrWhiteSpace(jsonData))
            {
                throw new InvalidDataException($"The file '{filePath}' is empty or contains invalid data.");
            }

            return JsonSerializer.Deserialize<T?>(jsonData);
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
        catch (InvalidDataException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error parsing JSON: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            return null;
        }
    }
}
