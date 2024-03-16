namespace EndPointFinder.Repository.Interfaces;

public interface IMainMethods
{
    Task<List<string>> ScanWebSiteForEnpoints(string url);
    Task<List<string>> ScanWebSiteForApis(string url);
}
