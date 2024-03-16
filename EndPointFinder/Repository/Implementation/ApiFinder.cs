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

    public async Task ScanAndFind(string urlToTest)
    {
        ChromeOptions chromeOptions = new();
        chromeOptions.AddArguments("--headless");
        ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService(@"C:\Users\oilur\source\repos\EndPointFinder\ApiFinder\", "chromedriver.exe");

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
                    string requestInfo = $"{DateTime.Now} - Request URL: {e.Request.Url}, Initiator URL: {e.Initiator.Url}";
                    _helperMethods.WriteToFile(requestInfo);
                }
            };

            devToolsSession.Network.RequestWillBeSent += (sender, e) =>
            {
                if (Regex.IsMatch(e.Request.Url, @"\b(apiKey|key)\b"))
                {
                    string requestInfo = $"{DateTime.Now} - Request URL: {e.Request.Url}, Initiator URL: {e.Initiator.Url}";
                    _helperMethods.WriteApiKeyToFile(requestInfo);
                }
            };

            NetworkInterceptionTest(urlToTest, devToolsSession, driver);
            SetAdditionalHeadersTest(urlToTest, devToolsSession, driver);
            SetUserAgentTest(urlToTest, devToolsSession, driver);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public void NetworkInterceptionTest(string urlToTest, DevToolsSessionDomains devToolsSession, IWebDriver driver)
    {
        devToolsSession.Network.SetBlockedURLs(new Network.SetBlockedURLsCommandSettings()
        {
            Urls = ["*://*/*.css", "*://*/*.jpg", "*://*/*.png"]
        });

        driver.Navigate().GoToUrl(urlToTest);
    }

    public void SetAdditionalHeadersTest(string urlToTest, DevToolsSessionDomains devToolsSession, IWebDriver driver)
    {
        var extraHeader = new Network.Headers();
        extraHeader.Add("headerName", "executeHacked");
        devToolsSession.Network.SetExtraHTTPHeaders(new Network.SetExtraHTTPHeadersCommandSettings()
        {
            Headers = extraHeader
        });

        driver.Navigate().GoToUrl(urlToTest);
    }

    public void SetUserAgentTest(string urlToTest, DevToolsSessionDomains devToolsSession, IWebDriver driver)
    {
        devToolsSession.Network.SetUserAgentOverride(new Network.SetUserAgentOverrideCommandSettings()
        {
            UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 12_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko)"
        });

        driver.Navigate().GoToUrl(urlToTest);
    }
}
