using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndPointFinder.ArchivedCode
{
    internal class ArchivedCode
    {
        // Setting are optimized on 2Nabiji
        //public static async Task DownloadEndpointResults(string url)
        //{
        //    try
        //    {
        //        using var httpClient = new HttpClient();
        //        HttpResponseMessage response = await httpClient.GetAsync(url);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            string jsonString = await response.Content.ReadAsStringAsync();

        //            Root root = JsonConvert.DeserializeObject<Root>(jsonString);

        //            var filePath = Path.Combine(@"C:\Users\oilur\source\repos\EndPointFinder\EndPointFinder\OutputData", $"order_endpoint_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt");

        //            foreach (var order in root.Data.Orders)
        //            {
        //                string serializedOrder = JsonConvert.SerializeObject(order, Formatting.Indented);

        //                await File.AppendAllTextAsync(filePath, serializedOrder + Environment.NewLine);
        //            }

        //            Console.WriteLine("Orders written to file successfully.");

        //        }

        //        else
        //        {
        //            Console.WriteLine($"Failed to download data. Status code: {response.StatusCode}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //    }
        //}

        //public static async Task WriteUserAndActionPerformerDetails(string url)
        //{
        //    try
        //    {
        //        using var httpClient = new HttpClient();
        //        HttpResponseMessage response = await httpClient.GetAsync(url);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            string jsonString = await response.Content.ReadAsStringAsync();

        //            Root root = JsonConvert.DeserializeObject<Root>(jsonString);

        //            var filePath = Path.Combine(@"C:\Users\oilur\source\repos\EndPointFinder\EndPointFinder\Data", $"user_and_action_performer_details_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt");

        //            foreach (var order in root.Data.Orders)
        //            {
        //                var user = order.user;
        //                var selectedLoc = order.selectedAddress;
        //                string userDetail = $"User - \n First Name: {user.firstName},\n Last Name: {user.lastName},\n Email: {user.email},\n Mobile Number: {user.phoneNumber}, \n Location: {selectedLoc} \n\t";

        //                var actionPerformer = order.actionPerformer;
        //                if (actionPerformer != null)
        //                {
        //                    string actionPerformerDetail = $"Action Performer - \n First Name: {actionPerformer.firstName},\n Last Name: {actionPerformer.lastName} \n\t";
        //                    await File.AppendAllTextAsync(filePath, actionPerformerDetail + Environment.NewLine);
        //                }

        //                await File.AppendAllTextAsync(filePath, userDetail + Environment.NewLine);
        //            }

        //            Console.WriteLine("User and Action Performer details written to file successfully.");
        //        }
        //        else
        //        {
        //            Console.WriteLine($"Failed to download data. Status code: {response.StatusCode}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //    }
        //}

        //static int CountWordsInFile(string filePath)
        //{
        //    string[] lines = File.ReadAllLines(filePath);

        //    int wordCount = 0;
        //    foreach (string line in lines)
        //    {
        //        string[] words = line.Split(',');

        //        wordCount += words.Length;
        //    }

        //    return wordCount;
        //}


        ///This Should Be in Main
        //var urlForDownload = @"https://catalog-api.orinabiji.ge/catalog/api/orders";
        //await DownloadEndpointResults(urlForDownload);
        //await WriteUserAndActionPerformerDetails(urlForDownload);

        //Console.WriteLine($"The number of words in the file is: {CountWordsInFile(textPath)}");
    }
}
