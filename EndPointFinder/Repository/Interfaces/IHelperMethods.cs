using EndPointFinder.Repository.Configuration;

namespace EndPointFinder.Repository.Interfaces;

public interface IHelperMethods
{
    Task<List<string>> WordTrimmerFromTxt(string textPath);

    void IncrementCompletedTasks(ref int completedTasks, int totalTasks);

    int CountWordsInFile(string filePath);

    List<int> PerfectDividerNumberFinder(int numberForDivide);

    Task<Config> LoadConfig();

    void WriteToFile(string content);

    void WriteApiKeyToFile(string content);

    string GetValidUrl();

    bool IsValidUrl(string url);
}
