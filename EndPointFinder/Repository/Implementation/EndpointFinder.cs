using EndPointFinder.Data.Context.Settings;
using EndPointFinder.Models.EndpointScanerModels;
using EndPointFinder.Repository.Interfaces;
using MongoDB.Driver;
using System.Net;
using Microsoft.Extensions.Options;

namespace EndPointFinder.Repository.Implementation;

public class EndpointFinder : IEndpointFinder
{
    private readonly IMongoCollection<EndpointScanerRootModels> _endpointscan;
    private readonly IHelperMethods _helperMethods;

    public EndpointFinder(IMongoCollection<EndpointScanerRootModels> endpointscan, IHelperMethods helperMethods)
    {
        _endpointscan = endpointscan;
        _helperMethods = helperMethods;
    }

    public async Task<EndpointScanerRootModels> MergedEndpointScanner(string url)
    {
        try
        {
            var configData = await _helperMethods.LoadConfig();
            var endpoints = await _helperMethods.WordTrimmerFromTxt(configData.TextPath);

            var results = new EndpointScanerRootModels
            {
                Endpoints = new HashSet<EndpointModels>(),
                Messages = new List<string>(),
            };

            Task<EndpointScanerRootModels> task1 = GetEndpointsWithoutApi(url, endpoints, configData.PerfectlyDivisorNum);
            Task<EndpointScanerRootModels> task2 = GetEndpointsWithApi(url, endpoints, configData.PerfectlyDivisorNum);
            Task<EndpointScanerRootModels> task3 = GetEndpointsWithS(url, endpoints, configData.PerfectlyDivisorNum);
            Task<EndpointScanerRootModels> task4 = GetEndpointsWithApiAndS(url, endpoints, configData.PerfectlyDivisorNum);

            await Task.WhenAll(task1, task2, task3, task4);

            results.Endpoints.UnionWith(task1.Result.Endpoints);
            results.Endpoints.UnionWith(task2.Result.Endpoints);
            results.Endpoints.UnionWith(task3.Result.Endpoints);
            results.Endpoints.UnionWith(task4.Result.Endpoints);

            results.Messages.AddRange(task1.Result.Messages);
            results.Messages.AddRange(task2.Result.Messages);
            results.Messages.AddRange(task3.Result.Messages);
            results.Messages.AddRange(task4.Result.Messages);

            await _endpointscan.InsertOneAsync(results);
            return results;
        }
        catch (Exception ex)
        {
            return new EndpointScanerRootModels { Messages = new List<string> { $"An error occurred: {ex.Message}" } };
        }
    }

    public async Task<EndpointScanerRootModels> GetEndpointsWithApiAndS(string url, List<string> endpoints, int perfectlyDivisorNum)
    {
        try
        {
            var successfulEndpoints = new HashSet<string>();

            var results = new EndpointScanerRootModels
            {
                Endpoints = new HashSet<EndpointModels>(),
                Messages = new List<string>(),
            };

            object uniqueResultsLock = new object();

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

                    var response = await httpClient.GetAsync(link);

                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                    {
                        lock (uniqueResultsLock)
                        {
                            if (!successfulEndpoints.Contains(link))
                            {
                                var endpointModel = new EndpointModels
                                {
                                    Type = "Api And S",
                                    Endpoint = link,
                                    Message = "Found",
                                    Amount = successfulEndpoints.Count + 1,
                                };

                                results.Endpoints.Add(endpointModel);
                                successfulEndpoints.Add(link);
                            }
                        }
                    }

                    _helperMethods.IncrementCompletedTasks(ref completedTasks, totalTasks);

                }).ToArray();
                await Task.WhenAll(tasks);
            }

            results.Messages.Add($"Found Url With Api and S : {successfulEndpoints.Count}");

            return results;
        }
        catch (Exception ex)
        {
            return new EndpointScanerRootModels { Messages = new List<string> { $"An error occurred: {ex.Message}" } };
        }
    }

    public async Task<EndpointScanerRootModels> GetEndpointsWithApi(string url, List<string> endpoints, int perfectlyDivisorNum)
    {
        try
        {
            var successfulEndpoints = new HashSet<string>();

            var results = new EndpointScanerRootModels
            {
                Endpoints = new HashSet<EndpointModels>(),
                Messages = new List<string>(),
            };

            object uniqueResultsLock = new object();

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
                    var link = url + "api/" + endpoint;

                    var response = await httpClient.GetAsync(link);
                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                    {
                        lock (uniqueResultsLock)
                        {
                            if (!successfulEndpoints.Contains(link))
                            {
                                var endpointModel = new EndpointModels
                                {
                                    Type = "Api",
                                    Endpoint = link,
                                    Message = "Found",
                                    Amount = successfulEndpoints.Count + 1,
                                };

                                results.Endpoints.Add(endpointModel);
                                successfulEndpoints.Add(link);
                            }
                        }
                    }

                    _helperMethods.IncrementCompletedTasks(ref completedTasks, totalTasks);

                }).ToArray();

                await Task.WhenAll(tasks);
            }

            results.Messages.Add($"Found Url with Api: {successfulEndpoints.Count}");

            return results;
        }
        catch (Exception ex)
        {
            return new EndpointScanerRootModels { Messages = new List<string> { $"An error occurred: {ex.Message}" } };
        }
    }

    public async Task<EndpointScanerRootModels> GetEndpointsWithoutApi(string url, List<string> endpoints, int perfectlyDivisorNum)
    {
        try
        {
            var successfulEndpoints = new HashSet<string>();

            var results = new EndpointScanerRootModels
            {
                Endpoints = new HashSet<EndpointModels>(),
                Messages = new List<string>(),
            };

            object uniqueResultsLock = new object();

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
                    var link = url + endpoint;

                    var response = await httpClient.GetAsync(link);
                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                    {
                        lock (uniqueResultsLock)
                        {
                            if (!successfulEndpoints.Contains(link))
                            {
                                var endpointModel = new EndpointModels
                                {
                                    Type = "Clean",
                                    Endpoint = link,
                                    Message = "Found",
                                    Amount = successfulEndpoints.Count + 1,
                                };

                                results.Endpoints.Add(endpointModel);
                                successfulEndpoints.Add(link);
                            }
                        }
                    }

                    _helperMethods.IncrementCompletedTasks(ref completedTasks, totalTasks);

                }).ToArray();

                await Task.WhenAll(tasks);
            }

            results.Messages.Add($"Found Clean Url: {successfulEndpoints.Count}");

            return results;
        }
        catch (Exception ex)
        {
            return new EndpointScanerRootModels { Messages = new List<string> { $"An error occurred: {ex.Message}" } };
        }
    }

    public async Task<EndpointScanerRootModels> GetEndpointsWithS(string url, List<string> endpoints, int perfectlyDivisorNum)
    {
        try
        {
            var successfulEndpoints = new HashSet<string>();

            var results = new EndpointScanerRootModels
            {
                Endpoints = new HashSet<EndpointModels>(),
                Messages = new List<string>(),
            };

            object uniqueResultsLock = new object();

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
                    var link = url + endpoint + "s";

                    var response = await httpClient.GetAsync(link);

                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                    {
                        lock (uniqueResultsLock)
                        {
                            if (!successfulEndpoints.Contains(link))
                            {
                                var endpointModel = new EndpointModels
                                {
                                    Type = "S",
                                    Endpoint = link,
                                    Message = "Found",
                                    Amount = successfulEndpoints.Count + 1,
                                };

                                results.Endpoints.Add(endpointModel);
                                successfulEndpoints.Add(link);
                            }
                        }
                    }

                    _helperMethods.IncrementCompletedTasks(ref completedTasks, totalTasks);

                }).ToArray();

                await Task.WhenAll(tasks);
            }

            results.Messages.Add($"Found Url With S : {successfulEndpoints.Count}");

            return results;
        }
        catch (Exception ex)
        {
            return new EndpointScanerRootModels { Messages = new List<string> { $"An error occurred: {ex.Message}" } };
        }
    }

    public async Task<IEnumerable<EndpointScanerRootModels>> GetAllEndpoints()
    {
        return await _endpointscan.Find(e => true).ToListAsync();
    }
}
