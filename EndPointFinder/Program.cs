using EndPointFinder.Repository.UnitOfWork;

namespace EndPointFinder;

public class Program
{
    private static readonly IUnitOfWork _unitOfWork = new UnitOfWork();

    static async Task Main(string[] args)
    {
        var url = @"https://catalog-api.orinabiji.ge/catalog/";

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
}
