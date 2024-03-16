using EndPointFinder.Repository.Interfaces;
using System.Net;
using System.Text;

namespace EndPointFinder.Repository.Implementation;

public class EndpointFinder : IEndpointFinder
{
    private readonly IHelperMethods _helperMethods = new HelperMethods();

    public async Task<string> GetEndpointsWithApiAndS(string url, List<string> endpoints, int perfectlyDivisorNum)
    {
        try
        {
            var successfulEndpoints = new StringBuilder();

            using var httpClient = new HttpClient();

            var batches = endpoints.Select((endpoint, index) => new { endpoint, index })
                                   .GroupBy(x => x.index / perfectlyDivisorNum)
                                   .Select(group => group.Select(x => x.endpoint).ToList())
                                   .ToList();

            int totalTasks = batches.Sum(batch => batch.Count);
            int completedTasks = 0;

            foreach (var batch in batches)
            {
                var tasks = batch.Select(async endpoint =>
                {
                    var link = url + "api/" + endpoint + "s/";

                    var response = await httpClient.GetAsync("api/" + endpoint + "s/");

                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                    {
                        successfulEndpoints.Append("With Api And S :");
                        successfulEndpoints.AppendLine(url + endpoint + "s");
                        successfulEndpoints.AppendLine();
                    }

                    _helperMethods.IncrementCompletedTasks(ref completedTasks, totalTasks);

                }).ToArray();
                await Task.WhenAll(tasks);
            }

            var result = new StringBuilder();
            result.AppendLine("Successful Endpoints With Api And S:");
            result.AppendLine(successfulEndpoints.ToString());

            return result.ToString();
        }
        catch (Exception ex)
        {
            return $"An error occurred: {ex.Message}";
        }
    }

    public async Task<string> GetEndpointsWithApi(string url, List<string> endpoints, int perfectlyDivisorNum)
    {
        try
        {
            var successfulEndpoints = new StringBuilder();

            using var httpClient = new HttpClient();

            var batches = endpoints.Select((endpoint, index) => new { endpoint, index })
                .GroupBy(x => x.index / perfectlyDivisorNum)
                .Select(group => group.Select(x => x.endpoint).ToList())
                .ToList();

            int totalTasks = batches.Sum(batch => batch.Count);
            int completedTasks = 0;

            foreach (var batch in batches)
            {
                var tasks = batch.Select(async endpoint =>
                {
                    var response = await httpClient.GetAsync(url + "api/" + endpoint);
                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                    {
                        successfulEndpoints.Append("With Api :");
                        successfulEndpoints.AppendLine(url + "api/" + endpoint);
                        successfulEndpoints.AppendLine();
                    }

                    _helperMethods.IncrementCompletedTasks(ref completedTasks, totalTasks);

                }).ToArray();

                await Task.WhenAll(tasks);
            }

            var result = new StringBuilder();
            result.AppendLine("Successful Endpoints With Api:");
            result.AppendLine(successfulEndpoints.ToString());

            return result.ToString();
        }
        catch (Exception ex)
        {
            return $"An error occurred: {ex.Message}";
        }
    }

    public async Task<string> GetEndpointsWithoutApi(string url, List<string> endpoints, int perfectlyDivisorNum)
    {
        try
        {
            var successfulEndpoints = new StringBuilder();

            using var httpClient = new HttpClient();

            var batches = endpoints.Select((endpoint, index) => new { endpoint, index })
                   .GroupBy(x => x.index / perfectlyDivisorNum)
                   .Select(group => group.Select(x => x.endpoint).ToList())
                   .ToList();

            int totalTasks = batches.Sum(batch => batch.Count);
            int completedTasks = 0;

            foreach (var batch in batches)
            {
                var tasks = batch.Select(async endpoint =>
                {
                    var response = await httpClient.GetAsync(url + endpoint);
                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                    {
                        successfulEndpoints.Append("Clean Endpoints :");
                        successfulEndpoints.AppendLine(url + endpoint);
                        successfulEndpoints.AppendLine();
                    }

                    _helperMethods.IncrementCompletedTasks(ref completedTasks, totalTasks);

                }).ToArray();

                await Task.WhenAll(tasks);
            }

            var result = new StringBuilder();
            result.AppendLine("Successful Endpoints Without Api:");
            result.AppendLine(successfulEndpoints.ToString());

            return result.ToString();
        }
        catch (Exception ex)
        {
            return $"An error occurred: {ex.Message}";
        }
    }

    public async Task<string> GetEndpointsWithS(string url, List<string> endpoints, int perfectlyDivisorNum)
    {
        try
        {
            var successfulEndpoints = new StringBuilder();

            using var httpClient = new HttpClient();
            var batches = endpoints.Select((endpoint, index) => new { endpoint, index })
                .GroupBy(x => x.index / perfectlyDivisorNum)
                .Select(group => group.Select(x => x.endpoint).ToList())
                .ToList();

            int totalTasks = batches.Sum(batch => batch.Count);
            int completedTasks = 0;

            foreach (var batch in batches)
            {
                var tasks = batch.Select(async endpoint =>
                {
                    var response = await httpClient.GetAsync(url + endpoint + "s");
                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                    {
                        successfulEndpoints.Append("With S :");
                        successfulEndpoints.AppendLine(url + endpoint + "s");
                        successfulEndpoints.AppendLine();
                    }

                    _helperMethods.IncrementCompletedTasks(ref completedTasks, totalTasks);

                }).ToArray();

                await Task.WhenAll(tasks);
            }

            var result = new StringBuilder();
            result.AppendLine("Successful Endpoints With S:");
            result.AppendLine(successfulEndpoints.ToString());

            return result.ToString();
        }
        catch (Exception ex)
        {
            return $"An error occurred: {ex.Message}";
        }
    }
}
