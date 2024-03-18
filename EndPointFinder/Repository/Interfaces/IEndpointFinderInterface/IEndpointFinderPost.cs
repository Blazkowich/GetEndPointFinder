using EndPointFinder.Models.EndpointScanerModels;
using EndPointFinder.Repository.Helpers.ExecutionMethods;

namespace EndPointFinder.Repository.Interfaces.IEndpointFinderInterface;

public interface IEndpointFinderPost
{
    Task<ExecutionResult<EndpointScanerRootModels>> MergedEndpointScanner(string url);

    Task<ExecutionResult<EndpointScanerRootModels>> ScanEndpointsWithApiAndS(string url);

    Task<ExecutionResult<EndpointScanerRootModels>> ScanEndpointsWithApi(string url);

    Task<ExecutionResult<EndpointScanerRootModels>> ScanEndpointsWithoutApi(string url);

    Task<ExecutionResult<EndpointScanerRootModels>> ScanEndpointsWithS(string url);
}
