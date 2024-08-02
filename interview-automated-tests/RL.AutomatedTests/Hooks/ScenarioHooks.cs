using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Reflection;
using TechTalk.SpecFlow;

namespace RL.AutomatedTests.Hooks;

[Binding]
public class ScenarioHooks
{
    [BeforeScenario(Order = 0)]
    public void SetupBrowser(ScenarioContext scenarioContext)
    {
        if (scenarioContext.ContainsKey("driver"))
            return;
        ChromeOptions chromeOptions = new ChromeOptions();
        chromeOptions.SetLoggingPreference(LogType.Browser, LogLevel.All);
        chromeOptions.AddArguments("--window-size=1920,1080");
        var driver = new ChromeDriver(chromeOptions);
        scenarioContext["driver"] = driver;
    }

    [AfterScenario(Order = 99999)]
    public void TearDownBrowser(ScenarioContext scenarioContext)
    {
        if (!scenarioContext.TryGetValue("driver", out IWebDriver browser))
            return;

        browser.Quit();
        browser.Dispose();
    }
}
