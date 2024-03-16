using EndPointFinder.Repository.Configuration;
using EndPointFinder.Repository.Interfaces;
using System.Text.Json;

namespace EndPointFinder.Repository.Implementation;

public class HelperMethods : IHelperMethods
{
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
}
