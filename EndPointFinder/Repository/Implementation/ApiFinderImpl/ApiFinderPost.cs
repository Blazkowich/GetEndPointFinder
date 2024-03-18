using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using DevToolsSessionDomains = OpenQA.Selenium.DevTools.V122.DevToolsSessionDomains;
using Network = OpenQA.Selenium.DevTools.V122.Network;
using System.Text.RegularExpressions;
using EndPointFinder.Models.ApiScanerModels;
using MongoDB.Driver;
using EndPointFinder.Repository.Interfaces.IApiFinderInterface;
using EndPointFinder.Repository.Helpers.ExecutionMethods;
using AutoMapper;

namespace EndPointFinder.Repository.Implementation.ApiFinder;

public class ApiFinderPost : IApiFinderPost
{
    private readonly IMongoCollection<ApiScanerRootModels> _apiscan;
    private readonly IMapper _mapper;

    public ApiFinderPost(IMongoCollection<ApiScanerRootModels> apiscan, IMapper mapper)
    {
        _apiscan = apiscan;
        _mapper = mapper;
    }

    public async Task<ExecutionResult<ApiScanerRootModels>> ScanAndFind(string urlToTest, bool ignoreMedia)
    {
        ChromeOptions chromeOptions = new();
        chromeOptions.AddArguments("--headless");
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string filePath = Path.Combine(baseDirectory, "..", "..", "..", "..", "EndpointFinder", "Data", "Config");
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
                        if (ignoreMedia && !e.Request.Url.EndsWith(".webp"))
                        {
                            var apiModelWithoutMedia = new ApiModels
                            {
                                RequestUrl = e.Request.Url,
                                InitiatorUrl = e.Initiator.Url
                            };

                            results.Apis.Add(apiModelWithoutMedia);
                        }

                        else if (!ignoreMedia)
                        {
                            var apiModel = new ApiModels
                            {
                                RequestUrl = e.Request.Url,
                                InitiatorUrl = e.Initiator.Url
                            };

                            results.Apis.Add(apiModel);
                        }
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
                        if (ignoreMedia && !e.Request.Url.EndsWith(".webp"))
                        {
                            var keyModelWithoutMedia = new KeyModels
                            {
                                RequestUrl = e.Request.Url,
                                InitiatorUrl = e.Initiator.Url
                            };

                            results.Keys.Add(keyModelWithoutMedia);
                        }

                        else if (!ignoreMedia)
                        {
                            var keyModel = new KeyModels
                            {
                                RequestUrl = e.Request.Url,
                                InitiatorUrl = e.Initiator.Url
                            };

                            results.Keys.Add(keyModel);
                        }
                    }
                }

                uniqueResults.Add(requestInfo);
            }
        };

        await NetworkInterceptionTest(urlToTest, devToolsSession, driver);
        await SetAdditionalHeadersTest(urlToTest, devToolsSession, driver);
        await SetUserAgentTest(urlToTest, devToolsSession, driver);

        if (results == null)
        {
            return new ExecutionResult<ApiScanerRootModels>
            {
                ResultType = ExecutionResultType.BadRequest,
                Message = $"Issue With Web Site {urlToTest}"
            };
        }

        await _apiscan.InsertOneAsync(results);

        return new ExecutionResult<ApiScanerRootModels>
        {
            ResultType = ExecutionResultType.Ok,
            Value = _mapper.Map<ApiScanerRootModels>(results),
        };
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
