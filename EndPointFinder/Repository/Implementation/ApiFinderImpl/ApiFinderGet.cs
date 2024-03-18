using EndPointFinder.Models.ApiScanerModels;
using EndPointFinder.Repository.Interfaces.IApiFinderInterface;
using MongoDB.Driver;

namespace EndPointFinder.Repository.Implementation.ApiFinderImpl;

public class ApiFinderGet : IApiFinderGet
{
    private readonly IMongoCollection<ApiScanerRootModels> _apiscan;

    public ApiFinderGet(IMongoCollection<ApiScanerRootModels> apiscan)
    {
        _apiscan = apiscan;
    }

    public async Task<IEnumerable<ApiScanerRootModels>> GetAllApis()
    {
        return await _apiscan.Find(e => true).ToListAsync();
    }

    public async Task<IEnumerable<ApiModels>> GetFilteredApisWithoutMedia(string id)
    {
        FilterDefinition<ApiScanerRootModels> filter = Builders<ApiScanerRootModels>.Filter.Eq(x => x.Id, id);

        List<ApiScanerRootModels> matchingDocuments = await _apiscan.Find(filter).ToListAsync();

        List<ApiModels> result = matchingDocuments
            .SelectMany(doc => doc.Apis)
            .Where(api => !api.RequestUrl.EndsWith(".webp"))
            .GroupBy(api => api.RequestUrl)
            .Select(group => group.First())
            .ToList();

        return result;
    }

    public async Task<IEnumerable<ApiModels>> GetApiCollectionById(string Id)
    {
        FilterDefinition<ApiScanerRootModels> filter = Builders<ApiScanerRootModels>.Filter.Eq(x => x.Id, Id);

        List<ApiScanerRootModels> matchingDocuments = await _apiscan.Find(filter).ToListAsync();

        List<ApiModels> result = matchingDocuments
            .SelectMany(doc => doc.Apis)
            .ToList();

        return result;
    }
}
