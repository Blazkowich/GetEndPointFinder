using System;
using System.IO;
using System.Net;

class Program
{
    static void Main(string[] args)
    {
        string url = @"https://regex101.com/";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

        try
        {
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                // Request URL
                Console.WriteLine($"Request URL: {url}");

                // Request Method
                Console.WriteLine($"Request Method: {request.Method}");

                // Status Code
                Console.WriteLine($"Status Code: {(int)response.StatusCode} {response.StatusCode}");

                // Remote Address
                Console.WriteLine($"Remote Address: {response.ResponseUri.Host}:{response.ResponseUri.Port}");
            }

        }
        catch (WebException ex)
        {
            if (ex.Response is HttpWebResponse errorResponse)
            {
                Console.WriteLine($"Error Response Status Code: {(int)errorResponse.StatusCode} {errorResponse.StatusCode}");
            }
            else
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        Console.ReadLine();
    }
}
