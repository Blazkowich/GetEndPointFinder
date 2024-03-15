﻿using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace EndPointFinder
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var url = @"https://www.megatechnica.ge/";
            var textPath = @"C:\Users\oilur\source\repos\EndPointFinder\EndPointFinder\Words\Dataset.txt";

            var endpoints = new List<string>();
            int perfectlyDivisorNum = 5;

            using (StreamReader reader = new StreamReader(textPath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] words = line.Split(',');
                    foreach (var word in words)
                    {
                        string trimmedWord = word.Trim(' ', '"');
                        if (!string.IsNullOrWhiteSpace(trimmedWord))
                        {
                            endpoints.Add(trimmedWord);
                        }
                    }
                }
            }

            Task<string> task1 = GetEndpointsWithoutApi(url, endpoints, perfectlyDivisorNum);
            Task<string> task2 = GetEndpointsWithApi(url, endpoints, perfectlyDivisorNum);
            Task<string> task3 = GetEndpointsWithS(url, endpoints, perfectlyDivisorNum);
            Task<string> task4 = GetEndpointsWithApiAndS(url, endpoints, perfectlyDivisorNum);

            await Task.WhenAll(task1, task2, task3, task4);

            Console.WriteLine($"Endpoints Without Anything: {task1.Result}");
            Console.WriteLine($"Endpoints With Api: {task2.Result}");
            Console.WriteLine($"Endpoints With S: {task3.Result}");
            Console.WriteLine($"Endpoints With Api And S: {task4.Result}");
        }

        public static async Task<string> GetEndpointsWithApiAndS(string url, List<string> endpoints, int perfectlyDivisorNum)
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

                        var response = await httpClient.GetAsync(link);

                        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                        {
                            successfulEndpoints.AppendLine(link);
                        }

                        lock (Console.Out)
                        {
                            Interlocked.Increment(ref completedTasks);
                            float progressPercentage = (float)completedTasks / totalTasks * 100;
                            Console.Write($"\rLoading... {progressPercentage:F2}%   ");
                        }


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

        public static async Task<string> GetEndpointsWithApi(string url, List<string> endpoints, int perfectlyDivisorNum)
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
                            successfulEndpoints.AppendLine(url + "api/" + endpoint);
                        }

                        lock (Console.Out)
                        {
                            Interlocked.Increment(ref completedTasks);
                            float progressPercentage = (float)completedTasks / totalTasks * 100;
                            Console.Write($"\rLoading... {progressPercentage:F2}%   ");
                        }

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

        public static async Task<string> GetEndpointsWithoutApi(string url, List<string> endpoints, int perfectlyDivisorNum)
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
                            successfulEndpoints.AppendLine(url + endpoint);
                        }

                        lock (Console.Out)
                        {
                            Interlocked.Increment(ref completedTasks);
                            float progressPercentage = (float)completedTasks / totalTasks * 100;
                            Console.Write($"\rLoading... {progressPercentage:F2}%   ");
                        }

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

        public static async Task<string> GetEndpointsWithS(string url, List<string> endpoints, int perfectlyDivisorNum)
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
                        var response = await httpClient.GetAsync(url + "api/" + endpoint + "s");
                        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                        {
                            successfulEndpoints.AppendLine(url + "api/" + endpoint + "s");
                        }

                        lock (Console.Out)
                        {
                            Interlocked.Increment(ref completedTasks);
                            float progressPercentage = (float)completedTasks / totalTasks * 100;
                            Console.Write($"\rLoading... {progressPercentage:F2}%   ");
                        }

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
}
