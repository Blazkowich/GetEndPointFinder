using EndPointFinder.Models.EndpointScanerModels;

namespace EndPointFinder.Repository.Interfaces.IEndpointFinderInterface;

public interface IEndpointFinderPost
{
    Task<EndpointScanerRootModels> MergedEndpointScanner(string url);

    Task<EndpointScanerRootModels> ScanEndpointsWithApiAndS(string url);

    Task<EndpointScanerRootModels> ScanEndpointsWithApi(string url);

    Task<EndpointScanerRootModels> ScanEndpointsWithoutApi(string url);

    Task<EndpointScanerRootModels> ScanEndpointsWithS(string url);
}
