using EndPointFinder.Models.EndpointScanerModels;

namespace EndPointFinder.Repository.Interfaces;

public interface IEndpointFinder
{
    Task<EndpointScanerRootModels> MergedEndpointScanner(string url);

    Task<EndpointScanerRootModels> GetEndpointsWithApiAndS(string url);

    Task<EndpointScanerRootModels> GetEndpointsWithApi(string url);

    Task<EndpointScanerRootModels> GetEndpointsWithoutApi(string url);

    Task<EndpointScanerRootModels> GetEndpointsWithS(string url);

    Task<IEnumerable<EndpointScanerRootModels>> GetAllEndpoints();
}
