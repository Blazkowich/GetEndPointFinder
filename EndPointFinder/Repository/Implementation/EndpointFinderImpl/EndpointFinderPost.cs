using AutoMapper;
using ConcurrentCollections;
using EndPointFinder.Data.Dataset;
using EndPointFinder.Models.EndpointScanerModels;
using EndPointFinder.Repository.Helpers.ExecutionMethods;
using EndPointFinder.Repository.Helpers.HelperMethodsImplementation;
using EndPointFinder.Repository.Interfaces.IEndpointFinderInterface;
using MongoDB.Driver;
using System.Diagnostics;
using System.Net;

namespace EndPointFinder.Repository.Implementation.EndpointFinderImpl;

public class EndpointFinderPost : IEndpointFinderPost
{
    private readonly IMongoCollection<EndpointScanerRootModels> _endpointscan;
    private readonly IHelperMethods _helperMethods;
    private readonly IMapper _mapper;

    public EndpointFinderPost(IMongoCollection<EndpointScanerRootModels> endpointscan, IHelperMethods helperMethods, IMapper mapper)
    {
        _endpointscan = endpointscan;
        _helperMethods = helperMethods;
        _mapper = mapper;
    }

    public async Task<ExecutionResult<EndpointScanerRootModels>> MergedEndpointScanner(string url)
    {
        try
        {
            var configData = await _helperMethods.LoadConfig();
            var endpoints = await _helperMethods.WordTrimmerFromJson(configData.TextPath);

            var results = new EndpointScanerRootModels
            {
                Endpoints = new HashSet<EndpointModels>(),
                Messages = new List<string>(),
            };

            Task<ExecutionResult<EndpointScanerRootModels>> task1 = ScanEndpointsWithoutApi(url);
            Task<ExecutionResult<EndpointScanerRootModels>> task2 = ScanEndpointsWithApi(url);
            Task<ExecutionResult<EndpointScanerRootModels>> task3 = ScanEndpointsWithS(url);
            Task<ExecutionResult<EndpointScanerRootModels>> task4 = ScanEndpointsWithApiAndS(url);

            await Task.WhenAll(task1, task2, task3, task4);

            results.Endpoints.UnionWith(task1.Result.Value.Endpoints);
            results.Endpoints.UnionWith(task2.Result.Value.Endpoints);
            results.Endpoints.UnionWith(task3.Result.Value.Endpoints);
            results.Endpoints.UnionWith(task4.Result.Value.Endpoints);

            results.Messages.AddRange(task1.Result.Value.Messages);
            results.Messages.AddRange(task2.Result.Value.Messages);
            results.Messages.AddRange(task3.Result.Value.Messages);
            results.Messages.AddRange(task4.Result.Value.Messages);

            if (results == null)
            {
                return new ExecutionResult<EndpointScanerRootModels>
                {
                    ResultType = ExecutionResultType.BadRequest,
                    Message = "Error Occured With Merged Scan Methods",
                };
            }

            return new ExecutionResult<EndpointScanerRootModels>
            {
                ResultType = ExecutionResultType.Ok,
                Value = _mapper.Map<EndpointScanerRootModels>(results),
            };
        }
        catch (Exception ex)
        {
            return new ExecutionResult<EndpointScanerRootModels>
            {
                ResultType = ExecutionResultType.BadRequest,
                Message = $"An error occurred: {ex.Message}",
            };
        }
    }

    public async Task<ExecutionResult<EndpointScanerRootModels>> ScanEndpointsWithApiAndS(string url)
    {
        try
        {
            var configData = await _helperMethods.LoadConfig();
            var endpoints = DictionaryHashMap.HashMap.SelectMany(kv => kv.Value).ToList();

            var successfulEndpoints = new ConcurrentHashSet<string>();

            var results = new EndpointScanerRootModels
            {
                Endpoints = new HashSet<EndpointModels>(),
                Messages = new List<string>(),
            };

            object uniqueResultsLock = new object();

            using var httpClient = new HttpClient();

            var batches = endpoints.Select((endpoint, index) => new { endpoint, index })
                                   .GroupBy(x => x.index / configData.PerfectlyDivisorNum)
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

            if (results == null)
            {
                return new ExecutionResult<EndpointScanerRootModels>
                {
                    ResultType = ExecutionResultType.BadRequest,
                    Message = "Error Occured With Api And S Scan Method",
                };
            }

            await _endpointscan.InsertOneAsync(results);

            return new ExecutionResult<EndpointScanerRootModels>
            {
                ResultType = ExecutionResultType.Ok,
                Value = _mapper.Map<EndpointScanerRootModels>(results),
            };
        }
        catch (Exception ex)
        {
            return new ExecutionResult<EndpointScanerRootModels>
            {
                ResultType = ExecutionResultType.BadRequest,
                Message = $"An error occurred: {ex.Message}",
            };
        }
    }

    public async Task<ExecutionResult<EndpointScanerRootModels>> ScanEndpointsWithApi(string url)
    {
        try
        {
            var configData = await _helperMethods.LoadConfig();
            var endpoints = DictionaryHashMap.HashMap.SelectMany(kv => kv.Value).ToList();

            var successfulEndpoints = new ConcurrentHashSet<string>();

            var results = new EndpointScanerRootModels
            {
                Endpoints = new HashSet<EndpointModels>(),
                Messages = new List<string>(),
            };

            object uniqueResultsLock = new object();

            using var httpClient = new HttpClient();

            var batches = endpoints.Select((endpoint, index) => new { endpoint, index })
                .GroupBy(x => x.index / configData.PerfectlyDivisorNum)
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

            if (results == null)
            {
                return new ExecutionResult<EndpointScanerRootModels>
                {
                    ResultType = ExecutionResultType.BadRequest,
                    Message = "Error Occured With Api Scan Method",
                };
            }

            await _endpointscan.InsertOneAsync(results);

            return new ExecutionResult<EndpointScanerRootModels>
            {
                ResultType = ExecutionResultType.Ok,
                Value = _mapper.Map<EndpointScanerRootModels>(results),
            };
        }
        catch (Exception ex)
        {
            return new ExecutionResult<EndpointScanerRootModels>
            {
                ResultType = ExecutionResultType.BadRequest,
                Message = $"An error occurred: {ex.Message}",
            };
        }
    }

    public async Task<ExecutionResult<EndpointScanerRootModels>> ScanEndpointsWithoutApi(string url)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            var configData = await _helperMethods.LoadConfig();

            var endpoints = DictionaryHashMap.HashMap.SelectMany(kv => kv.Value).ToList();

            var successfulEndpoints = new ConcurrentHashSet<string>();

            var results = new EndpointScanerRootModels
            {
                Endpoints = new HashSet<EndpointModels>(),
                Messages = new List<string>(),
            };

            object uniqueResultsLock = new object();

            using var httpClient = new HttpClient();

            var batches = endpoints.Select((endpoint, index) => new { endpoint, index })
                   .GroupBy(x => x.index / configData.PerfectlyDivisorNum)
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

            if (results == null)
            {
                return new ExecutionResult<EndpointScanerRootModels>
                {
                    ResultType = ExecutionResultType.BadRequest,
                    Message = "Error Occured With Clean Scan Method",
                };
            }

            await _endpointscan.InsertOneAsync(results);

            stopwatch.Stop();

            // Log the elapsed time
            Console.WriteLine($"Method execution took: {stopwatch.Elapsed.TotalSeconds} seconds");


            return new ExecutionResult<EndpointScanerRootModels>
            {
                ResultType = ExecutionResultType.Ok,
                Value = _mapper.Map<EndpointScanerRootModels>(results),
            };
        }
        catch (Exception ex)
        {
            return new ExecutionResult<EndpointScanerRootModels>
            {
                ResultType = ExecutionResultType.BadRequest,
                Message = $"An error occurred: {ex.Message}",
            };
        }
    }

    public async Task<ExecutionResult<EndpointScanerRootModels>> ScanEndpointsWithS(string url)
    {
        try
        {
            var configData = await _helperMethods.LoadConfig();
            var endpoints = DictionaryHashMap.HashMap.SelectMany(kv => kv.Value).ToList();

            var successfulEndpoints = new ConcurrentHashSet<string>();

            var results = new EndpointScanerRootModels
            {
                Endpoints = new HashSet<EndpointModels>(),
                Messages = new List<string>(),
            };

            object uniqueResultsLock = new object();

            using var httpClient = new HttpClient();
            var batches = endpoints.Select((endpoint, index) => new { endpoint, index })
                .GroupBy(x => x.index / configData.PerfectlyDivisorNum)
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

            if (results == null)
            {
                return new ExecutionResult<EndpointScanerRootModels>
                {
                    ResultType = ExecutionResultType.BadRequest,
                    Message = "Error Occured With S Scan Method",
                };
            }

            await _endpointscan.InsertOneAsync(results);

            return new ExecutionResult<EndpointScanerRootModels>
            {
                ResultType = ExecutionResultType.Ok,
                Value = _mapper.Map<EndpointScanerRootModels>(results),
            };
        }
        catch (Exception ex)
        {
            return new ExecutionResult<EndpointScanerRootModels>
            {
                ResultType = ExecutionResultType.BadRequest,
                Message = $"An error occurred: {ex.Message}",
            };
        }
    }
}