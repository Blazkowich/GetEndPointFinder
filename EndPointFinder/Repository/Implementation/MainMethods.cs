using EndPointFinder.Repository.Interfaces;
using EndPointFinder.Repository.UnitOfWork;

namespace EndPointFinder.Repository.Implementation;

public class MainMethods : IMainMethods
{
    private readonly IUnitOfWork _unitOfWork;

    public MainMethods(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task ScanWebSiteForEnpoints(string url)
    {
        var configData = await _unitOfWork.HelperMethods.LoadConfig();
        var endpoints = await _unitOfWork.HelperMethods.WordTrimmerFromTxt(configData.TextPath);

        Task<string> task1 = _unitOfWork.EndpointFinder.GetEndpointsWithoutApi(url, endpoints, configData.PerfectlyDivisorNum);
        Task<string> task2 = _unitOfWork.EndpointFinder.GetEndpointsWithApi(url, endpoints, configData.PerfectlyDivisorNum);
        Task<string> task3 = _unitOfWork.EndpointFinder.GetEndpointsWithS(url, endpoints, configData.PerfectlyDivisorNum);
        Task<string> task4 = _unitOfWork.EndpointFinder.GetEndpointsWithApiAndS(url, endpoints, configData.PerfectlyDivisorNum);

        await Task.WhenAll(task1, task2, task3, task4);

        Console.WriteLine($"Endpoints Without Anything: {task1.Result}");
        Console.WriteLine($"Endpoints With Api: {task2.Result}");
        Console.WriteLine($"Endpoints With S: {task3.Result}");
        Console.WriteLine($"Endpoints With Api And S: {task4.Result}");
    }

    public async Task ScanWebSiteForApis(string url)
    {
        try
        {
            await _unitOfWork.ApiFinder.ScanAndFind(url);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
