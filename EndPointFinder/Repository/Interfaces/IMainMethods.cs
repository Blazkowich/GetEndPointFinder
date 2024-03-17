using EndPointFinder.Models.ApiScanerModels;

namespace EndPointFinder.Repository.Interfaces;

public interface IMainMethods
{
    Task<List<string>> ScanWebSiteForEnpoints(string url);
    Task<ApiScanerRootModels> ScanWebSiteForApis(string url);
}
