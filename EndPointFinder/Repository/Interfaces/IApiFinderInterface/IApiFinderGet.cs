using EndPointFinder.Models.ApiScanerModels;

namespace EndPointFinder.Repository.Interfaces.IApiFinderInterface;

public interface IApiFinderGet
{
    Task<IEnumerable<ApiScanerRootModels>> GetAllApis();

    Task<IEnumerable<ApiModels>> GetFilteredApisWithoutMedia(string id);

    Task<IEnumerable<ApiModels>> GetApiCollectionById(string Id);
}
