using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EndPointFinder
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var url = @"https://localhost:7153/";
            var endpoints = new List<string> { "games", "genres", "platforms" };

            Console.WriteLine(await GetSuccessfullEndpoint(url, endpoints));
        }

        public static async Task<string> GetSuccessfullEndpoint(string url, List<string> endpoints)
        {
            try
            {
                var successfulEndpoints = new StringBuilder();

                using var httpClient = new HttpClient();
                foreach (var endpoint in endpoints)
                {
                    var response = await httpClient.GetAsync(url + endpoint);

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
    }
}
