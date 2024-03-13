using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace EndPointFinder
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var url = @"https://catalog-api.orinabiji.ge/catalog/";
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

            Console.WriteLine(await GetSuccessfullEndpoint(url, endpoints));
            Console.WriteLine(await GetSuccessfullEndpointWithoutApi(url, endpoints));
            Console.WriteLine(await GetSuccessfullEndpointWithApiAndS(url, endpoints));
            Console.WriteLine(await GetSuccessfullEndpointWithS(url, endpoints));
        }

        public static async Task<string> GetSuccessfullEndpoint(string url, List<string> endpoints)
        {
            try
            {
                var successfulEndpoints = new StringBuilder();

                using var httpClient = new HttpClient();
                foreach (var endpoint in endpoints)
                {
                    var response = await httpClient.GetAsync(url + "api/" + endpoint);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        successfulEndpoints.AppendLine(url + endpoint);
                    }
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
        public static async Task<string> GetSuccessfullEndpointWithoutApi(string url, List<string> endpoints)
        {
            try
            {
                var successfulEndpoints = new StringBuilder();

                using var httpClient = new HttpClient();
                foreach (var endpoint in endpoints)
                {
                    var responsewithoutApi = await httpClient.GetAsync(url + endpoint);

                    if (responsewithoutApi.StatusCode == HttpStatusCode.OK)
                    {
                        successfulEndpoints.AppendLine(url + endpoint);
                    }
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
        public static async Task<string> GetSuccessfullEndpointWithApiAndS(string url, List<string> endpoints)
        {
            try
            {
                var successfulEndpoints = new StringBuilder();

                using var httpClient = new HttpClient();
                foreach (var endpoint in endpoints)
                {
                    var responsewithS = await httpClient.GetAsync(url + "api/" + endpoint + "s");

                    if (responsewithS.StatusCode == HttpStatusCode.OK)
                    {
                        successfulEndpoints.AppendLine(url +"api/" + endpoint + "s");
                    }
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
        public static async Task<string> GetSuccessfullEndpointWithS(string url, List<string> endpoints)
        {
            try
            {
                var successfulEndpoints = new StringBuilder();

                using var httpClient = new HttpClient();
                foreach (var endpoint in endpoints)
                {
                    var responsewithSWithoutApi = await httpClient.GetAsync(url + endpoint + "s");

                    if (responsewithSWithoutApi.StatusCode == HttpStatusCode.OK)
                    {
                        successfulEndpoints.AppendLine(url + endpoint);
                    }
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
