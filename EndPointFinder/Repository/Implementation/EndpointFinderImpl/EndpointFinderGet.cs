using EndPointFinder.Models.EndpointScanerModels;
using MongoDB.Driver;
using EndPointFinder.Repository.Interfaces.IEndpointFinderInterface;

namespace EndPointFinder.Repository.Implementation.EndpointFinderImpl;

public class EndpointFinderGet : IEndpointFinderGet
{
    private readonly IMongoCollection<EndpointScanerRootModels> _endpointscan;

    public EndpointFinderGet(IMongoCollection<EndpointScanerRootModels> endpointscan)
    {
        _endpointscan = endpointscan;
    }

    public async Task<IEnumerable<EndpointScanerRootModels>> GetAllEndpoints()
    {
        return await _endpointscan.Find(e => true).ToListAsync();
    }
}
