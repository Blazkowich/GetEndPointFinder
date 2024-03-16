using EndPointFinder.Repository.Interfaces;

namespace EndPointFinder.Repository.Implementation;

public class MainMethods : IMainMethods
{
    private readonly IHelperMethods _helperMethods = new HelperMethods();
    private readonly IEndpointFinder _endpointFinder = new EndpointFinder();
    private readonly IApiFinder _apiFinder = new ApiFinder();

    public async Task<List<string>> ScanWebSiteForEnpoints(string url)
    {
        var configData = await _helperMethods.LoadConfig();
        var endpoints = await _helperMethods.WordTrimmerFromTxt(configData.TextPath);
        var results = new List<string>();

        Task<string> task1 = _endpointFinder.GetEndpointsWithoutApi(url, endpoints, configData.PerfectlyDivisorNum);
        Task<string> task2 = _endpointFinder.GetEndpointsWithApi(url, endpoints, configData.PerfectlyDivisorNum);
        Task<string> task3 = _endpointFinder.GetEndpointsWithS(url, endpoints, configData.PerfectlyDivisorNum);
        Task<string> task4 = _endpointFinder.GetEndpointsWithApiAndS(url, endpoints, configData.PerfectlyDivisorNum);

        await Task.WhenAll(task1, task2, task3, task4);

        results.Add(task1.Result);
        results.Add(task2.Result);
        results.Add(task3.Result);
        results.Add(task4.Result);

        _helperMethods.WriteEndpointsToFile(task1.Result);
        _helperMethods.WriteEndpointsToFile(task2.Result);
        _helperMethods.WriteEndpointsToFile(task3.Result);
        _helperMethods.WriteEndpointsToFile(task4.Result);

        return results;
    }

    public async Task<List<string>> ScanWebSiteForApis(string url)
    {
        try
        {
            return await _apiFinder.ScanAndFind(url);
        }
        catch (Exception ex)
        {
            return new List<string> { ex.Message };
        }
    }
}
