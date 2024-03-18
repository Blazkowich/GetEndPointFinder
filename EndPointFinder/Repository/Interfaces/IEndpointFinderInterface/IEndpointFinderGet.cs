using EndPointFinder.Models.EndpointScanerModels;
using EndPointFinder.Repository.Helpers.ExecutionMethods;

namespace EndPointFinder.Repository.Interfaces.IEndpointFinderInterface;

public interface IEndpointFinderGet
{
    Task<ExecutionResult<IEnumerable<EndpointScanerRootModels>>> GetAllEndpoints();
}
