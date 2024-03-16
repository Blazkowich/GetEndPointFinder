using OpenQA.Selenium;
using DevToolsSessionDomains = OpenQA.Selenium.DevTools.V122.DevToolsSessionDomains;

namespace EndPointFinder.Repository.Interfaces;

public interface IApiFinder
{
    Task ScanAndFind(string urlToTest);

    void NetworkInterceptionTest(string urlToTest, DevToolsSessionDomains devToolsSession, IWebDriver driver);

    void SetAdditionalHeadersTest(string urlToTest, DevToolsSessionDomains devToolsSession, IWebDriver driver);

    void SetUserAgentTest(string urlToTest, DevToolsSessionDomains devToolsSession, IWebDriver driver);
}
