namespace EndPointFinder.Repository.Interfaces;

public interface IEndpointFinder
{
    Task<string> GetEndpointsWithApiAndS(string url, List<string> endpoints, int perfectlyDivisorNum);

    Task<string> GetEndpointsWithApi(string url, List<string> endpoints, int perfectlyDivisorNum);

    Task<string> GetEndpointsWithoutApi(string url, List<string> endpoints, int perfectlyDivisorNum);

    Task<string> GetEndpointsWithS(string url, List<string> endpoints, int perfectlyDivisorNum);
}
