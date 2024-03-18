using EndPointFinder.Data.Config;
using EndPointFinder.Models.UrlModel;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace EndPointFinder.Repository.Helpers.HelperMethodsImplementation;

public class HelperMethods : IHelperMethods
{
    readonly object fileLock = new();

    public HelperMethods()
    {
        LoadConfig().Wait();
    }

    public async Task<List<string>> WordTrimmerFromTxt(string textPath)
    {
        try
        {
            var endpoints = new List<string>();

            using (StreamReader reader = new StreamReader(textPath))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    string[] words = line.Split(',');
                    foreach (var word in words)
                    {
                        string trimmedWord = word.Trim(' ', '"');
                        if (!string.IsNullOrWhiteSpace(trimmedWord))
                        {
                            endpoints.Add(trimmedWord);
                        }
                    }
                }
            }

            return endpoints;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return [];
        }
    }

    public async Task<List<string>> WordTrimmerFromJson(string dictionaryPath)
    {
        try
        {
            var endpoints = new List<string>();

            string jsonContent = await File.ReadAllTextAsync(dictionaryPath);

            var wordDictionary = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonContent);

            foreach (var kvp in wordDictionary)
            {
                foreach (var word in kvp.Value)
                {
                    string trimmedWord = word.Trim();
                    if (!string.IsNullOrWhiteSpace(trimmedWord) && !endpoints.Contains(trimmedWord))
                    {
                        endpoints.Add(trimmedWord);
                    }
                }
            }

            return endpoints;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new List<string>();
        }
    }

    public void IncrementCompletedTasks(ref int completedTasks, int totalTasks)
    {
        lock (Console.Out)
        {
            Interlocked.Increment(ref completedTasks);
            float progressPercentage = (float)completedTasks / totalTasks * 100;
            Console.Write($"\rLoading... {progressPercentage:F2}%   ");
        }
    }

    public int CountWordsInFile(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);

        int wordCount = 0;
        foreach (string line in lines)
        {
            string[] words = line.Split(',');

            wordCount += words.Length;
        }

        return wordCount;
    }

    public List<int> PerfectDividerNumberFinder(int numberForDivide)
    {
        List<int> listOfPerfectDividers = [];

        for (int num = 5; num <= 10000; num++)
        {
            int dividedNumber = numberForDivide % num;

            if (dividedNumber == 0)
            {
                listOfPerfectDividers.Add(num);
            }
        }

        foreach (int perfectDivider in listOfPerfectDividers)
        {
            Console.WriteLine(perfectDivider);
        }
        return listOfPerfectDividers;
    }

    public async Task<Config> LoadConfig()
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        string jsonFilePath = Path.Combine(baseDirectory, "..", "..", "..", "..", "EndpointFinder", "Data", "Config", "Config.json");

        jsonFilePath = Path.GetFullPath(jsonFilePath);

        string json = await File.ReadAllTextAsync(jsonFilePath);

        return System.Text.Json.JsonSerializer.Deserialize<Config>(json);
    }

    // Unused
    public void WriteToFile(string content)
    {
        lock (fileLock)
        {
            try
            {
                string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                string currentDirectory = Path.Combine(userDirectory, "EndpointFinder");

                string logsDirectory = Path.Combine(currentDirectory, "Records", "ApiEndpoints");

                Directory.CreateDirectory(logsDirectory);

                string filePath = Path.Combine(logsDirectory, $"endpoints_{DateTime.Now:yyyy-MM-dd-HH-mm}.txt");

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
            }
        }
    }

    // Unused
    public void WriteEndpointsToFile(string content)
    {
        lock (fileLock)
        {
            try
            {
                string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                string currentDirectory = Path.Combine(userDirectory, "EndpointFinder");

                string logsDirectory = Path.Combine(currentDirectory, "Records", "EndpointLogs");

                Directory.CreateDirectory(logsDirectory);

                string filePath = Path.Combine(logsDirectory, $"network_requests_{DateTime.Now:yyyy-MM-dd-HH-mm}.txt");

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
            }
        }
    }

    // Unused
    public void WriteApiKeyToFile(string content)
    {
        lock (fileLock)
        {
            try
            {
                string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                string currentDirectory = Path.Combine(userDirectory, "EndpointFinder");

                string logsDirectory = Path.Combine(currentDirectory, "Records", "KeyLogs");

                Directory.CreateDirectory(logsDirectory);

                string filePath = Path.Combine(logsDirectory, $"ApiKey_requests_{DateTime.Now:yyyy-MM-dd-HH-mm}.txt");

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
            }
        }
    }

    public async Task<UrlModel> GetValidUrl(string inputUrl)
    {
        if (!await IsValidUrl(inputUrl))
        {
            return new UrlModel
            {
                Message = "Enter Correct Url.",
            };
        }
        else if (!inputUrl.StartsWith("http://") && !inputUrl.StartsWith("https://"))
        {
            inputUrl = "https://" + inputUrl + "/";
        }

        return new UrlModel
        {
            Url = inputUrl,
        };
    }

    public async Task<bool> IsValidUrl(string url)
    {
        var configData = await LoadConfig();
        var domains = await WordTrimmerFromTxt(configData.DomainPath);

        string pattern = @"^(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ;,./?%&=]*)?$";
        return Regex.IsMatch(url, pattern) && domains.Any(url.EndsWith);
    }
}
