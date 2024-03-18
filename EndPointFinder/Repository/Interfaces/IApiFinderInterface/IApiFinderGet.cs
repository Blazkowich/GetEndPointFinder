using EndPointFinder.Models.ApiScanerModels;
using EndPointFinder.Repository.Helpers.ExecutionMethods;

namespace EndPointFinder.Repository.Interfaces.IApiFinderInterface;

public interface IApiFinderGet
{
    Task<ExecutionResult<IEnumerable<ApiScanerRootModels>>> GetAllApis();

    Task<ExecutionResult<IEnumerable<ApiModels>>> GetApiCollectionById(string Id, bool ignoreMedia);

    Task<ExecutionResult<IEnumerable<KeyModels>>> GetKeyCollectionById(string id);
}
