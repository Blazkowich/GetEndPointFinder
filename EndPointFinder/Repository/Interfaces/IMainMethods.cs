namespace EndPointFinder.Repository.Interfaces;

public interface IMainMethods
{
    Task<List<string>> ScanWebSiteForEnpoints(string url);
    Task<HashSet<string>> ScanWebSiteForApis(string url);
}
