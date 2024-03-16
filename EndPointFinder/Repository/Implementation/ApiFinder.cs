using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using DevToolsSessionDomains = OpenQA.Selenium.DevTools.V122.DevToolsSessionDomains;
using Network = OpenQA.Selenium.DevTools.V122.Network;
using System.Text.RegularExpressions;
using EndPointFinder.Repository.Interfaces;

namespace EndPointFinder.Repository.Implementation;

public class ApiFinder : IApiFinder
{
    private readonly IHelperMethods _helperMethods = new HelperMethods();

    public async Task<HashSet<string>> ScanAndFind(string urlToTest)
    {
        ChromeOptions chromeOptions = new();
        chromeOptions.AddArguments("--headless");
        ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService(@"C:\Users\oilur\source\repos\EndPointFinder\EndPointFinder\Repository\Config\", "chromedriver.exe");
        var results = new HashSet<string>();

        using IWebDriver driver = new ChromeDriver(chromeDriverService, chromeOptions);
        try
        {
            IDevTools devTools = driver as IDevTools;

            IDevToolsSession session = devTools.GetDevToolsSession();
            DevToolsSessionDomains devToolsSession = session.GetVersionSpecificDomains<DevToolsSessionDomains>();

            await devToolsSession.Network.Enable(new Network.EnableCommandSettings());

            devToolsSession.Network.RequestWillBeSent += (sender, e) =>
            {
                if (Regex.IsMatch(e.Request.Url, @"\bapi\b"))
                {
                    string requestInfo = $"Api - Request URL: {e.Request.Url}, Initiator URL: {e.Initiator.Url}";
                    if (!results.Contains(requestInfo))
                    {
                        _helperMethods.WriteToFile(requestInfo);
                    }

                    results.Add(requestInfo);
                }
            };

            devToolsSession.Network.RequestWillBeSent += (sender, e) =>
            {
                if (Regex.IsMatch(e.Request.Url, @"\b(apiKey|key)\b"))
                {
                    string requestInfo = $"Key - Request URL: {e.Request.Url}, Initiator URL: {e.Initiator.Url}";

                    if (!results.Contains(requestInfo))
                    {
                        _helperMethods.WriteApiKeyToFile(requestInfo);
                    }

                    results.Add(requestInfo);
                }
            };

            await NetworkInterceptionTest(urlToTest, devToolsSession, driver);
            await SetAdditionalHeadersTest(urlToTest, devToolsSession, driver);
            await SetUserAgentTest(urlToTest, devToolsSession, driver);

            return results;
        }
        catch (Exception ex)
        {
            return [$"An error occurred: {ex.Message}"];
        }
    }

    public async Task NetworkInterceptionTest(string urlToTest, DevToolsSessionDomains devToolsSession, IWebDriver driver)
    {
        await devToolsSession.Network.SetBlockedURLs(new Network.SetBlockedURLsCommandSettings()
        {
            Urls = ["*://*/*.css", "*://*/*.jpg", "*://*/*.png"]
        });

        driver.Navigate().GoToUrl(urlToTest);
    }

    public async Task SetAdditionalHeadersTest(string urlToTest, DevToolsSessionDomains devToolsSession, IWebDriver driver)
    {
        var extraHeader = new Network.Headers();
        extraHeader.Add("headerName", "executeHacked");
        await devToolsSession.Network.SetExtraHTTPHeaders(new Network.SetExtraHTTPHeadersCommandSettings()
        {
            Headers = extraHeader
        });

        driver.Navigate().GoToUrl(urlToTest);
    }

    public async Task SetUserAgentTest(string urlToTest, DevToolsSessionDomains devToolsSession, IWebDriver driver)
    {
        await devToolsSession.Network.SetUserAgentOverride(new Network.SetUserAgentOverrideCommandSettings()
        {
            UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko)"
        });

        driver.Navigate().GoToUrl(urlToTest);
    }
}
