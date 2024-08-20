using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using TechTalk.SpecFlow;

namespace RL.AutomatedTests.Steps.Plan;

[Binding]
public class PlanSteps
{
    private readonly ScenarioContext _context;
    private readonly string _urlBase = "http://localhost:3001";
    private readonly TimeSpan _waitDurration = new(0, 0, 20);

    public PlanSteps(ScenarioContext context)
    {
        _context = context;
    }

    [Given("I'm on the start page")]
    public async Task ImOnTheStartPage()
    {
        var driver = _context.Get<IWebDriver>("driver");
        driver.Navigate().GoToUrl(_urlBase);
        var wait = new WebDriverWait(driver, _waitDurration);
        wait.Until(ExpectedConditions.UrlContains(_urlBase));
    }

    [When("I click on start")]
    public async Task IClickOnStart()
    {
        var driver = _context.Get<IWebDriver>("driver");
        driver.FindElement(By.Id("start")).Click();
        var wait = new WebDriverWait(driver, _waitDurration);
        wait.Until(ExpectedConditions.UrlMatches(_urlBase + "/plan"));
    }

    [Then("I'm on the plan page")]
    public async Task ImOnThePlanPage()
    {
        var driver = _context.Get<IWebDriver>("driver");
        var wait = new WebDriverWait(driver, _waitDurration);
        wait.Until(ExpectedConditions.UrlMatches(@"/plan/(\d+)"));
        Thread.Sleep(10000);
        driver.Url.Should().MatchRegex(@"/plan/(\d+)");
    }

    [When(@"I add a procedure to the plan")]
    public void WhenIAddAProcedureToThePlan()
    {
        var driver = _context.Get<IWebDriver>("driver");
        driver.FindElement(By.XPath("//label[contains(text(),'Front Seat Control Switch')]")).Click();
        var wait = new WebDriverWait(driver, _waitDurration);
        wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(),'Front Seat Control Switch')]")));
    }
    [When(@"Assign user to plan procedure")]
    public void WhenAssignUserToPlanProcedure()
    {
        var driver = _context.Get<IWebDriver>("driver");
        var wait = new WebDriverWait(driver, _waitDurration);
        var dropdown = wait.Until(drv => drv.FindElement(By.CssSelector(".css-13cymwt-control")));
        dropdown.Click();
        IWebElement listbox = driver.FindElement(By.Id("react-select-3-listbox"));
        var options = listbox.FindElements(By.TagName("div"));
        foreach (var option in options)
        {
            if (option.Text.Contains("Tony Bidner")) 
            {
                option.Click();
                break;
            }
        }
    }

    [When(@"refresh the page")]
    public void WhenRefreshThePage()
    {
        var driver = _context.Get<IWebDriver>("driver");
        driver.Navigate().Refresh();
    }

    [Then(@"verify the user attached to plan procedure")]
    public void ThenVerifyTheUserAttachedToPlanProcedure()
    {
        var driver = _context.Get<IWebDriver>("driver");

        var wait = new WebDriverWait(driver, _waitDurration);

        IWebElement dropdownOption = driver.FindElement(By.XPath("//div[text()='Tony Bidner']"));

        Assert.IsTrue(dropdownOption.Displayed);
    }

}