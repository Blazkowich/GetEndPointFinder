using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EndPointFinder
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var url = @"https://catalog-api.orinabiji.ge/catalog/";
            var urlForDownload = @"https://catalog-api.orinabiji.ge/catalog/api/orders";
            var textPath = @"C:\Users\oilur\source\repos\EndPointFinder\EndPointFinder\output.txt";
            var endpoints = new List<string>();

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

            //Console.WriteLine(await GetEndpointsWithApiAndS(url, endpoints));

            await DownloadEndpointResults(urlForDownload);
            await WriteUserAndActionPerformerDetails(urlForDownload);

        }
        public static async Task<string> GetEndpointsWithApiAndS(string url, List<string> endpoints)
        {
            try
            {
                var successfulEndpoints = new StringBuilder();

                using var httpClient = new HttpClient();

                var batches = endpoints.Select((endpoint, index) => new { endpoint, index })
                                       .GroupBy(x => x.index / 5)
                                       .Select(group => group.Select(x => x.endpoint).ToList())
                                       .ToList();

                foreach (var batch in batches)
                {
                    var tasks = batch.Select(async endpoint =>
                    {
                        var link = url + "api/" + endpoint + "s/";

                        var response = await httpClient.GetAsync(link);

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            successfulEndpoints.AppendLine(link);
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

        public static async Task DownloadEndpointResults(string url)
        {
            try
            {
                using var httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();

                    Root root = JsonConvert.DeserializeObject<Root>(jsonString);

                    var filePath = Path.Combine(@"C:\Users\oilur\source\repos\EndPointFinder\EndPointFinder\", "order_endpoint.txt");

                    foreach (var order in root.Data.Orders)
                    {
                        string serializedOrder = JsonConvert.SerializeObject(order, Formatting.Indented);

                        // Append to the file
                        await File.AppendAllTextAsync(filePath, serializedOrder + Environment.NewLine);
                    }

                    Console.WriteLine("Orders written to file successfully.");

                }

                else
                {
                    Console.WriteLine($"Failed to download data. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public static async Task WriteUserAndActionPerformerDetails(string url)
        {
            try
            {
                using var httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();

                    Root root = JsonConvert.DeserializeObject<Root>(jsonString);

                    var filePath = Path.Combine(@"C:\Users\oilur\source\repos\EndPointFinder\EndPointFinder\", "user_and_action_performer_details.txt");

                    foreach (var order in root.Data.Orders)
                    {
                        var user = order.user;
                        var selectedLoc = order.selectedAddress;
                        string userDetail = $"User - \n First Name: {user.firstName},\n Last Name: {user.lastName},\n Email: {user.email},\n Mobile Number: {user.phoneNumber}, \n Location: {selectedLoc} \n\t";

                        var actionPerformer = order.actionPerformer;
                        if (actionPerformer != null)
                        {
                            string actionPerformerDetail = $"Action Performer - \n First Name: {actionPerformer.firstName},\n Last Name: {actionPerformer.lastName} \n\t";
                            await File.AppendAllTextAsync(filePath, actionPerformerDetail + Environment.NewLine);
                        }

                        // Write user details to the file
                        await File.AppendAllTextAsync(filePath, userDetail + Environment.NewLine);
                    }

                    Console.WriteLine("User and Action Performer details written to file successfully.");
                }
                else
                {
                    Console.WriteLine($"Failed to download data. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }






        public static async Task<string> GetEndpointsWithApi(string url, List<string> endpoints)
        {
            try
            {
                var successfulEndpoints = new StringBuilder();

                using var httpClient = new HttpClient();

                var batches = endpoints.Select((endpoint, index) => new { endpoint, index })
                    .GroupBy(x => x.index / 10)
                    .Select(group => group.Select(x => x.endpoint).ToList())
                    .ToList();

                foreach (var batch in batches)
                {
                    var tasks = batch.Select(async endpoint =>
                    {
                        var response = await httpClient.GetAsync(url + "api/" + endpoint);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            successfulEndpoints.AppendLine(url + "api/" + endpoint);
                        }
                    }).ToArray();

                    await Task.WhenAll(tasks);
                }

                var result = new StringBuilder();
                result.AppendLine("Successful Endpoints:");
                result.AppendLine(successfulEndpoints.ToString());

                return result.ToString();
            }
            catch (Exception ex)
            {
                return $"An error occurred: {ex.Message}";
            }
        }

        public static async Task<string> GetEndpointsWithoutApi(string url, List<string> endpoints)
        {
            try
            {
                var successfulEndpoints = new StringBuilder();

                using var httpClient = new HttpClient();

                var batches = endpoints.Select((endpoint, index) => new { endpoint, index })
                       .GroupBy(x => x.index / 10)
                       .Select(group => group.Select(x => x.endpoint).ToList())
                       .ToList();

                foreach (var batch in batches)
                {
                    var tasks = batch.Select(async endpoint =>
                    {
                        var response = await httpClient.GetAsync(url + endpoint);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            successfulEndpoints.AppendLine(url + endpoint);
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

        public static async Task<string> GetEndpointsWithS(string url, List<string> endpoints)
        {
            try
            {
                var successfulEndpoints = new StringBuilder();

                using var httpClient = new HttpClient();
                var batches = endpoints.Select((endpoint, index) => new { endpoint, index })
                    .GroupBy(x => x.index / 10)
                    .Select(group => group.Select(x => x.endpoint).ToList())
                    .ToList();

                foreach (var batch in batches)
                {
                    var tasks = batch.Select(async endpoint =>
                    {
                        var response = await httpClient.GetAsync(url + "api/" + endpoint + "s");
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            successfulEndpoints.AppendLine(url + "api/" + endpoint + "s");
                        }
                    }).ToArray();

                    await Task.WhenAll(tasks);
                }

                var result = new StringBuilder();
                result.AppendLine("Successful Endpoints:");
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
