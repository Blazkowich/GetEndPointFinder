namespace EndPointFinder.Repository.Interfaces;

public interface IMainMethods
{
    Task ScanWebSiteForEnpoints(string url);

    void ScanWebSiteForApis(string url);
}
