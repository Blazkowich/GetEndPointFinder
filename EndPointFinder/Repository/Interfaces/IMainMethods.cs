using EndPointFinder.Models.ApiScanerModels;
using EndPointFinder.Models.EndpointScanerModels;

namespace EndPointFinder.Repository.Interfaces;

public interface IMainMethods
{
    Task<EndpointScanerRootModels> ScanWebSiteForEnpoints(string url);
    Task<ApiScanerRootModels> ScanWebSiteForApis(string url);
}
