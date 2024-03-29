﻿using EndPointFinder.Models.ApiScanerModels;
using EndPointFinder.Repository.Helpers.ExecutionMethods;
using OpenQA.Selenium;
using DevToolsSessionDomains = OpenQA.Selenium.DevTools.V122.DevToolsSessionDomains;

namespace EndPointFinder.Repository.Interfaces.IApiFinderInterface;

public interface IApiFinderPost
{
    Task<ExecutionResult<ApiScanerRootModels>> ScanAndFind(string urlToTest, bool ignoreMedia);

    Task NetworkInterceptionTest(string urlToTest, DevToolsSessionDomains devToolsSession, IWebDriver driver);

    Task SetAdditionalHeadersTest(string urlToTest, DevToolsSessionDomains devToolsSession, IWebDriver driver);

    Task SetUserAgentTest(string urlToTest, DevToolsSessionDomains devToolsSession, IWebDriver driver);
}
