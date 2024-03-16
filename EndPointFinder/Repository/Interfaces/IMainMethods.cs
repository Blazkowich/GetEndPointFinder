namespace EndPointFinder.Repository.Interfaces;

public interface IMainMethods
{
    Task ScanWebSiteForEnpoints(string url);

    Task ScanWebSiteForApis(string url);
}
