using EndPointFinder.Models.ApiScanerModels;
using EndPointFinder.Models.EndpointScanerModels;
using EndPointFinder.Repository.Interfaces;

namespace EndPointFinder.Repository.Implementation;

public class MainMethods : IMainMethods
{
    private readonly IEndpointFinder _endpointFinder;
    private readonly IApiFinder _apiFinder;

    public MainMethods(IEndpointFinder endpointFinder, IApiFinder apiFinder)
    {
        _endpointFinder = endpointFinder;
        _apiFinder = apiFinder;
    }

    public async Task<IEnumerable<EndpointScanerRootModels>> GetAllEndpoints()
    {
        return await _endpointFinder.GetAllEndpoints();
    }

    public async Task<EndpointScanerRootModels> ScanWebSiteForEnpoints(string url)
    {
        return await _endpointFinder.MergedEndpointScanner(url);
    }

    public async Task<IEnumerable<ApiScanerRootModels>> GetAllApis()
    {
        return await _apiFinder.GetAllApis();
    }

    public async Task<ApiScanerRootModels> ScanWebSiteForApis(string url)
    {
        return await _apiFinder.ScanAndFind(url);
    }
}
