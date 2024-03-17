using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using DevToolsSessionDomains = OpenQA.Selenium.DevTools.V122.DevToolsSessionDomains;
using Network = OpenQA.Selenium.DevTools.V122.Network;
using System.Text.RegularExpressions;
using EndPointFinder.Repository.Interfaces;
using EndPointFinder.Models.ApiScanerModels;
using MongoDB.Driver;

namespace EndPointFinder.Repository.Implementation;

public class ApiFinder : IApiFinder
{
    private readonly IMongoCollection<ApiScanerRootModels> _apiscan;
    private readonly IHelperMethods _helperMethods;

    public ApiFinder(IMongoCollection<ApiScanerRootModels> apiscan, IHelperMethods helperMethods)
    {
        _apiscan = apiscan;
        _helperMethods = helperMethods;
    }

    public async Task<IEnumerable<ApiScanerRootModels>> GetAllApis()
    {
        return await _apiscan.Find(e => true).ToListAsync();
    }

    public async Task<ApiScanerRootModels> ScanAndFind(string urlToTest)
    {
        ChromeOptions chromeOptions = new();
        chromeOptions.AddArguments("--headless");
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string filePath = Path.Combine(baseDirectory, "..", "..", "..", "..", "EndpointFinder", "Repository", "Config");
        ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService(filePath, "chromedriver.exe");

        object uniqueResultsLock = new object();

        var uniqueResults = new HashSet<string>();

        var results = new ApiScanerRootModels
        {
            Apis = new HashSet<ApiModels>(),
            Keys = new HashSet<KeyModels>()
        };

        using IWebDriver driver = new ChromeDriver(chromeDriverService, chromeOptions);

        IDevTools devTools = driver as IDevTools;

        IDevToolsSession session = devTools.GetDevToolsSession();
        DevToolsSessionDomains devToolsSession = session.GetVersionSpecificDomains<DevToolsSessionDomains>();

        await devToolsSession.Network.Enable(new Network.EnableCommandSettings());

        devToolsSession.Network.RequestWillBeSent += (sender, e) =>
        {
            if (Regex.IsMatch(e.Request.Url, @"\bapi\b"))
            {
                string requestInfo = $"Api - Request URL: {e.Request.Url}, Initiator URL: {e.Initiator.Url}";

                lock (uniqueResultsLock)
                {
                    if (!uniqueResults.Contains(requestInfo))
                    {
                        var apiModel = new ApiModels
                        {
                            RequestUrl = e.Request.Url,
                            InitiatorUrl = e.Initiator.Url
                        };

                        results.Apis.Add(apiModel);
                    }
                }

                uniqueResults.Add(requestInfo);
            }
        };

        devToolsSession.Network.RequestWillBeSent += (sender, e) =>
        {
            if (Regex.IsMatch(e.Request.Url, @"\b(apiKey|key)\b"))
            {
                string requestInfo = $"Key - Request URL: {e.Request.Url}, Initiator URL: {e.Initiator.Url}";

                lock (uniqueResultsLock)
                {
                    if (!uniqueResults.Contains(requestInfo))
                    {
                        var keyModel = new KeyModels
                        {
                            RequestUrl = e.Request.Url,
                            InitiatorUrl = e.Initiator.Url
                        };

                        results.Keys.Add(keyModel);
                    }
                }

                uniqueResults.Add(requestInfo);
            }
        };

        await NetworkInterceptionTest(urlToTest, devToolsSession, driver);
        await SetAdditionalHeadersTest(urlToTest, devToolsSession, driver);
        await SetUserAgentTest(urlToTest, devToolsSession, driver);
        
        await _apiscan.InsertOneAsync(results);
        return results;
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
