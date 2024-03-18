using EndPointFinder.Models.ApiScanerModels;
using EndPointFinder.Repository.Helpers.ExecutionMethods;

namespace EndPointFinder.Repository.Interfaces.IApiFinderInterface;

public interface IApiFinderGet
{
    Task<ExecutionResult<IEnumerable<ApiScanerRootModels>>> GetAllApis();

    Task<ExecutionResult<IEnumerable<ApiModels>>> GetFilteredApisWithoutMedia(string id);

    Task<ExecutionResult<IEnumerable<ApiModels>>> GetApiCollectionById(string Id);

    Task<ExecutionResult<IEnumerable<KeyModels>>> GetKeyCollectionById(string id);
}
