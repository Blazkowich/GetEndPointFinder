using OpenQA.Selenium;
using DevToolsSessionDomains = OpenQA.Selenium.DevTools.V122.DevToolsSessionDomains;

namespace EndPointFinder.Repository.Interfaces;

public interface IApiFinder
{
    Task<List<string>> ScanAndFind(string urlToTest);

    Task NetworkInterceptionTest(string urlToTest, DevToolsSessionDomains devToolsSession, IWebDriver driver);

    Task SetAdditionalHeadersTest(string urlToTest, DevToolsSessionDomains devToolsSession, IWebDriver driver);

    Task SetUserAgentTest(string urlToTest, DevToolsSessionDomains devToolsSession, IWebDriver driver);
}
