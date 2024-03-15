namespace EndPointFinder.Helpers;

public class HelperMethods
{
    public static async Task<List<string>> WordTrimmerFromTxt(string textPath)
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

    public static void IncrementCompletedTasks(ref int completedTasks, int totalTasks)
    {
        lock (Console.Out)
        {
            Interlocked.Increment(ref completedTasks);
            float progressPercentage = (float)completedTasks / totalTasks * 100;
            Console.Write($"\rLoading... {progressPercentage:F2}%   ");
        }
    }
}
