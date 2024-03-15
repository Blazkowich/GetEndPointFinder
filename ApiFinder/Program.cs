using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using DevToolsSessionDomains = OpenQA.Selenium.DevTools.V122.DevToolsSessionDomains;
using Network = OpenQA.Selenium.DevTools.V122.Network;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApiFinder
{
    class Program
    {
        static string urlToTest = @"https://2nabiji.ge/en/product/ghvino-thethri-alavi-kakhuri-mshrali-294#";
        static string logFilePath = $"network_requests_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.txt";
        static readonly object fileLock = new object(); // Lock object to synchronize file access

        static async Task Main(string[] args)
        {
            // Set ChromeDriver path
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--headless");
            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService(@"C:\Users\oilur\source\repos\EndPointFinder\ApiFinder\", "chromedriver.exe");

            // Set up Chrome driver
            using (IWebDriver driver = new ChromeDriver(chromeDriverService, chromeOptions))
            {
                try
                {
                    // Get DevTools
                    IDevTools devTools = driver as IDevTools;

                    // DevTools Session
                    IDevToolsSession session = devTools.GetDevToolsSession();
                    DevToolsSessionDomains devToolsSession = session.GetVersionSpecificDomains<DevToolsSessionDomains>();

                    // Initialize Network domain
                    await devToolsSession.Network.Enable(new Network.EnableCommandSettings());

                    // Subscribe to the RequestWillBeSent event
                    devToolsSession.Network.RequestWillBeSent += (sender, e) =>
                    {
                        if (Regex.IsMatch(e.Request.Url, @"\bapi\b"))
                        {
                            string requestInfo = $"{DateTime.Now} - Request URL: {e.Request.Url}, Initiator URL: {e.Initiator.Url}";
                            WriteToFile(requestInfo);
                        }
                    };

                    // Run tests
                    NetworkInterceptionTest(devToolsSession, driver);
                    SetAdditionalHeadersTest(devToolsSession, driver);
                    SetUserAgentTest(devToolsSession, driver);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        static void NetworkInterceptionTest(DevToolsSessionDomains devToolsSession, IWebDriver driver)
        {
            devToolsSession.Network.SetBlockedURLs(new Network.SetBlockedURLsCommandSettings()
            {
                Urls = new string[] { "*://*/*.css", "*://*/*.jpg", "*://*/*.png" }
            });

            driver.Navigate().GoToUrl(urlToTest);
        }

        static void SetAdditionalHeadersTest(DevToolsSessionDomains devToolsSession, IWebDriver driver)
        {
            var extraHeader = new Network.Headers();
            extraHeader.Add("headerName", "executeHacked");
            devToolsSession.Network.SetExtraHTTPHeaders(new Network.SetExtraHTTPHeadersCommandSettings()
            {
                Headers = extraHeader
            });

            driver.Navigate().GoToUrl(urlToTest);
        }

        static void SetUserAgentTest(DevToolsSessionDomains devToolsSession, IWebDriver driver)
        {
            devToolsSession.Network.SetUserAgentOverride(new Network.SetUserAgentOverrideCommandSettings()
            {
                UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko)"
            });

            driver.Navigate().GoToUrl(urlToTest);
        }

        static void WriteToFile(string content)
        {
            lock (fileLock)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(@"C:\Users\oilur\source\repos\EndPointFinder\ApiFinder\Records\" + logFilePath, true))
                    {
                        writer.WriteLine(content);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
                }
            }
        }
    }
}
