using EndPointFinder.Repository.Interfaces;

namespace EndPointFinder.Repository.Implementation;

public class MainMethods : IMainMethods
{
    private readonly IHelperMethods _helperMethods = new HelperMethods();
    private readonly IEndpointFinder _endpointFinder = new EndpointFinder();
    private readonly IApiFinder _apiFinder = new ApiFinder();

    public async Task ScanWebSiteForEnpoints(string url)
    {
        var configData = await _helperMethods.LoadConfig();
        var endpoints = await _helperMethods.WordTrimmerFromTxt(configData.TextPath);

        Task<string> task1 = _endpointFinder.GetEndpointsWithoutApi(url, endpoints, configData.PerfectlyDivisorNum);
        Task<string> task2 = _endpointFinder.GetEndpointsWithApi(url, endpoints, configData.PerfectlyDivisorNum);
        Task<string> task3 = _endpointFinder.GetEndpointsWithS(url, endpoints, configData.PerfectlyDivisorNum);
        Task<string> task4 = _endpointFinder.GetEndpointsWithApiAndS(url, endpoints, configData.PerfectlyDivisorNum);

        await Task.WhenAll(task1, task2, task3, task4);

        Console.WriteLine($"Endpoints Without Anything: {task1.Result}");
        Console.WriteLine($"Endpoints With Api: {task2.Result}");
        Console.WriteLine($"Endpoints With S: {task3.Result}");
        Console.WriteLine($"Endpoints With Api And S: {task4.Result}");
    }

    public async Task ScanWebSiteForApis(string url)
    {
        try
        {
            await _apiFinder.ScanAndFind(url);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
