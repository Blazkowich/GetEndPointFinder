using AutoMapper;
using EndPointFinder.Models.ApiScanerModels;
using EndPointFinder.Repository.Helpers.ExecutionMethods;
using EndPointFinder.Repository.Interfaces.IApiFinderInterface;
using MongoDB.Driver;

namespace EndPointFinder.Repository.Implementation.ApiFinderImpl;

public class ApiFinderGet : IApiFinderGet
{
    private readonly IMongoCollection<ApiScanerRootModels> _apiscan;
    private readonly IMapper _mapper;

    public ApiFinderGet(IMongoCollection<ApiScanerRootModels> apiscan, IMapper mapper)
    {
        _apiscan = apiscan;
        _mapper = mapper;
    }

    public async Task<ExecutionResult<IEnumerable<ApiScanerRootModels>>> GetAllApis()
    {
        var result = await _apiscan.Find(e => true).ToListAsync();

        if (result.Count == 0)
        {
            return new ExecutionResult<IEnumerable<ApiScanerRootModels>>
            {
                ResultType = ExecutionResultType.NotFound,
                Message = "No Api Collection Was Found"
            };
        }

        return new ExecutionResult<IEnumerable<ApiScanerRootModels>>
        {
            ResultType = ExecutionResultType.Ok,
            Value = _mapper.Map<IEnumerable<ApiScanerRootModels>>(result),
        };
    }

    public async Task<ExecutionResult<IEnumerable<ApiModels>>> GetFilteredApisWithoutMedia(string id)
    {
        FilterDefinition<ApiScanerRootModels> filter = Builders<ApiScanerRootModels>.Filter.Eq(x => x.Id, id);

        List<ApiScanerRootModels> matchingDocuments = await _apiscan.Find(filter).ToListAsync();

        List<ApiModels> result = matchingDocuments
            .SelectMany(doc => doc.Apis)
            .Where(api => !api.RequestUrl.EndsWith(".webp"))
            .GroupBy(api => api.RequestUrl)
            .Select(group => group.First())
            .ToList();

        if (result == null)
        {
            return new ExecutionResult<IEnumerable<ApiModels>>
            {
                ResultType = ExecutionResultType.NotFound,
                Message = $"No Api Collection Was Found On {id} ID.",
            };
        }

        return new ExecutionResult<IEnumerable<ApiModels>>
        {
            ResultType = ExecutionResultType.Ok,
            Value = _mapper.Map<IEnumerable<ApiModels>>(result),
        };
    }

    public async Task<ExecutionResult<IEnumerable<ApiModels>>> GetApiCollectionById(string id)
    {
        FilterDefinition<ApiScanerRootModels> filter = Builders<ApiScanerRootModels>.Filter.Eq(x => x.Id, id);

        List<ApiScanerRootModels> matchingDocuments = await _apiscan.Find(filter).ToListAsync();

        List<ApiModels> result = matchingDocuments
            .SelectMany(doc => doc.Apis)
            .ToList();

        if (result == null)
        {
            return new ExecutionResult<IEnumerable<ApiModels>>
            {
                ResultType = ExecutionResultType.NotFound,
                Message = $"No Api Collection Was Found On {id} ID.",
            };
        }

        return new ExecutionResult<IEnumerable<ApiModels>>
        {
            ResultType = ExecutionResultType.Ok,
            Value = _mapper.Map<IEnumerable<ApiModels>>(result),
        };
    }

    public async Task<ExecutionResult<IEnumerable<KeyModels>>> GetKeyCollectionById(string id)
    {
        FilterDefinition<ApiScanerRootModels> filter = Builders<ApiScanerRootModels>.Filter.Eq(x => x.Id, id);

        List<ApiScanerRootModels> matchingDocuments = await _apiscan.Find(filter).ToListAsync();

        List<KeyModels> result = matchingDocuments
            .SelectMany(doc => doc.Keys)
            .ToList();

        if (result == null)
        {
            return new ExecutionResult<IEnumerable<KeyModels>>
            {
                ResultType = ExecutionResultType.NotFound,
                Message = $"No Key Collection Was Found On {id} ID.",
            };
        }

        return new ExecutionResult<IEnumerable<KeyModels>>
        {
            ResultType = ExecutionResultType.Ok,
            Value = _mapper.Map<IEnumerable<KeyModels>>(result),
        };
    }
}
