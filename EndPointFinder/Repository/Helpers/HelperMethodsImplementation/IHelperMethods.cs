using EndPointFinder.Data.Config;
using EndPointFinder.Models.UrlModel;

namespace EndPointFinder.Repository.Helpers.HelperMethodsImplementation;

public interface IHelperMethods
{
    Task<List<string>> WordTrimmerFromTxt(string textPath);

    Task<List<string>> WordTrimmerFromJson(string dictionaryPath);

    void IncrementCompletedTasks(ref int completedTasks, int totalTasks);

    int CountWordsInFile(string filePath);

    Task<int> PerfectDividerNumberFinder(int numberForDivide);

    Task<Config> LoadConfig();

    void WriteToFile(string content);

    void WriteEndpointsToFile(string content);

    void WriteApiKeyToFile(string content);

    Task<UrlModel> GetValidUrl(string inputUrl);

    Task<bool> IsValidUrl(string url);
}
