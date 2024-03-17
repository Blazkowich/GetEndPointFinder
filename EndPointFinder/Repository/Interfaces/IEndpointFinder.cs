using EndPointFinder.Models.EndpointScanerModels;

namespace EndPointFinder.Repository.Interfaces;

public interface IEndpointFinder
{
    Task<EndpointScanerRootModels> GetEndpointsWithApiAndS(string url, List<string> endpoints, int perfectlyDivisorNum);

    Task<EndpointScanerRootModels> GetEndpointsWithApi(string url, List<string> endpoints, int perfectlyDivisorNum);

    Task<EndpointScanerRootModels> GetEndpointsWithoutApi(string url, List<string> endpoints, int perfectlyDivisorNum);

    Task<EndpointScanerRootModels> GetEndpointsWithS(string url, List<string> endpoints, int perfectlyDivisorNum);
}
