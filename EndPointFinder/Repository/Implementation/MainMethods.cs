using EndPointFinder.Models.ApiScanerModels;
using EndPointFinder.Models.EndpointScanerModels;
using EndPointFinder.Repository.Interfaces;

namespace EndPointFinder.Repository.Implementation;

public class MainMethods : IMainMethods
{
    private readonly IHelperMethods _helperMethods = new HelperMethods();
    private readonly IEndpointFinder _endpointFinder = new EndpointFinder();
    private readonly IApiFinder _apiFinder = new ApiFinder();

    public async Task<EndpointScanerRootModels> ScanWebSiteForEnpoints(string url)
    {
        try
        {
            var configData = await _helperMethods.LoadConfig();
            var endpoints = await _helperMethods.WordTrimmerFromTxt(configData.TextPath);

            var results = new EndpointScanerRootModels
            {
                Endpoints = new HashSet<EndpointModels>(),
                Messages = new List<string>(),
            };

            Task<EndpointScanerRootModels> task1 = _endpointFinder.GetEndpointsWithoutApi(url, endpoints, configData.PerfectlyDivisorNum);
            Task<EndpointScanerRootModels> task2 = _endpointFinder.GetEndpointsWithApi(url, endpoints, configData.PerfectlyDivisorNum);
            Task<EndpointScanerRootModels> task3 = _endpointFinder.GetEndpointsWithS(url, endpoints, configData.PerfectlyDivisorNum);
            Task<EndpointScanerRootModels> task4 = _endpointFinder.GetEndpointsWithApiAndS(url, endpoints, configData.PerfectlyDivisorNum);

            await Task.WhenAll(task1, task2, task3, task4);

            results.Endpoints.UnionWith(task1.Result.Endpoints);
            results.Endpoints.UnionWith(task2.Result.Endpoints);
            results.Endpoints.UnionWith(task3.Result.Endpoints);
            results.Endpoints.UnionWith(task4.Result.Endpoints);

            results.Messages.AddRange(task1.Result.Messages);
            results.Messages.AddRange(task2.Result.Messages);
            results.Messages.AddRange(task3.Result.Messages);
            results.Messages.AddRange(task4.Result.Messages);

            return results;
        }
        catch (Exception ex)
        {
            return new EndpointScanerRootModels { Messages = new List<string> { $"An error occurred: {ex.Message}" } };
        }
    }

    public async Task<ApiScanerRootModels> ScanWebSiteForApis(string url)
    {
        return await _apiFinder.ScanAndFind(url);
    }
}
