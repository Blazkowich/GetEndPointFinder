using EndPointFinder.Models.EndpointScanerModels;
using MongoDB.Driver;
using EndPointFinder.Repository.Interfaces.IEndpointFinderInterface;
using EndPointFinder.Repository.Helpers.ExecutionMethods;
using AutoMapper;

namespace EndPointFinder.Repository.Implementation.EndpointFinderImpl;

public class EndpointFinderGet : IEndpointFinderGet
{
    private readonly IMongoCollection<EndpointScanerRootModels> _endpointscan;
    private readonly IMapper _mapper;

    public EndpointFinderGet(IMongoCollection<EndpointScanerRootModels> endpointscan, IMapper mapper)
    {
        _endpointscan = endpointscan;
        _mapper = mapper;
    }

    public async Task<ExecutionResult<IEnumerable<EndpointScanerRootModels>>> GetAllEndpoints()
    {
        var result = await _endpointscan.Find(e => true).ToListAsync();

        if (result.Count == 0)
        {
            return new ExecutionResult<IEnumerable<EndpointScanerRootModels>> 
            {
                ResultType = ExecutionResultType.NotFound,
                Message = "No Endpoint Collection Was Found",
            };
        }

        return new ExecutionResult<IEnumerable<EndpointScanerRootModels>>
        {
            ResultType = ExecutionResultType.Ok,
            Value = _mapper.Map<IEnumerable<EndpointScanerRootModels>>(result),
        };
    }
}
