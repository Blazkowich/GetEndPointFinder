using EndPointFinder.Models.EndpointScanerModels;

namespace EndPointFinder.Repository.Interfaces.IEndpointFinderInterface;

public interface IEndpointFinderGet
{
    Task<IEnumerable<EndpointScanerRootModels>> GetAllEndpoints();
}
