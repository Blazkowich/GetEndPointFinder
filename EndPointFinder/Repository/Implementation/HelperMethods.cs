using EndPointFinder.Repository.Configuration;
using EndPointFinder.Repository.Interfaces;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace EndPointFinder.Repository.Implementation;

public class HelperMethods : IHelperMethods
{
    readonly object fileLock = new();
    private static Config _config;

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
        string json = await File.ReadAllTextAsync(@"C:\Users\oilur\source\repos\EndPointFinder\EndPointFinder\Repository\Config\Config.json");
        return JsonSerializer.Deserialize<Config>(json);
    }

    //public void WriteToFile(string content)
    //{
    //    lock (fileLock)
    //    {
    //        try
    //        {
    //            using StreamWriter writer = new StreamWriter(@"C:\Users\oilur\source\repos\EndPointFinder\EndPointFinder\Records\NetworkLogs\" + $"network_requests_{DateTime.Now:yyyy-MM-dd-HH-mm}.txt", true);
    //            writer.WriteLine(content);
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
    //        }
    //    }
    //}

    //public void WriteApiKeyToFile(string content)
    //{
    //    lock (fileLock)
    //    {
    //        try
    //        {
    //            using StreamWriter writer = new StreamWriter(@"C:\Users\oilur\source\repos\EndPointFinder\EndPointFinder\Records\ApiKeyLogs\" + $"ApiKey_requests_{DateTime.Now:yyyy-MM-dd-HH-mm}.txt", true);
    //            writer.WriteLine(content);
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
    //        }
    //    }
    //}
    public void WriteToFile(string content)
    {
        lock (fileLock)
        {
            try
            {
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string logsDirectory = Path.Combine(currentDirectory, "Records", "NetworkLogs");

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

    public void WriteApiKeyToFile(string content)
    {
        lock (fileLock)
        {
            try
            {
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string logsDirectory = Path.Combine(currentDirectory, "Records", "ApiKeyLogs");

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

    public string GetValidUrl()
    {
        string inputUrl;
        do
        {
            Console.WriteLine("Please enter the URL:");
            inputUrl = Console.ReadLine();

            if (!IsValidUrl(inputUrl))
            {
                Console.WriteLine("Incorrect URL format. Please try again.");
            }
            else if (!inputUrl.StartsWith("http://") && !inputUrl.StartsWith("https://"))
            {
                inputUrl = "https://" + inputUrl + "/";
            }
        } while (!IsValidUrl(inputUrl));

        return inputUrl;
    }

    public bool IsValidUrl(string url)
    {
        string pattern = @"^(http(s)?://)?([\w-]+\.)+[\w-]+(/[\w- ;,./?%&=]*)?$";
        return Regex.IsMatch(url, pattern);
    }
}
