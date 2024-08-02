using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Runtime.Intrinsics.X86;
using TechTalk.SpecFlow;

namespace RL.AutomatedTests.Steps.Plan;

[Binding]
public class PlanSteps
{
    private readonly ScenarioContext _context;
    private readonly string _urlBase = "http://localhost:3001";
    private readonly TimeSpan _waitDurration = new(0, 0, 20);

    private readonly string firstProcedure = "//label[normalize-space(text())='Front Seat Control Switch - Removal and Installation - Front Seats']";

    private readonly string selectedUser = "//div[contains(@class,'control')]//*[text()]";

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
        var wait = new WebDriverWait(driver, _waitDurration);
        wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[text()=\"Start\"]"))).Click();
        wait.Until(ExpectedConditions.UrlMatches(_urlBase + "/plan"));
    }

    [Then("I'm on the plan page")]
    public async Task ImOnThePlanPage()
    {
        var driver = _context.Get<IWebDriver>("driver");
        var wait = new WebDriverWait(driver, _waitDurration);
        wait.Until(ExpectedConditions.UrlMatches(@"/plan/(\d+)"));
        driver.Url.Should().MatchRegex(@"/plan/(\d+)");
    }

    [When("I Select Procedures To Plan")]
    public async Task ISelectProceduresToPlan()
    {
        var driver = _context.Get<IWebDriver>("driver");
        var wait = new WebDriverWait(driver, _waitDurration);
        wait.Until(ExpectedConditions.UrlMatches(@"/plan/(\d+)"));
        driver.Url.Should().MatchRegex(@"/plan/(\d+)");
    }

    [Then("I See Plan Procedure")]
    public async Task ISeePlanProcedure()
    {
        var driver = _context.Get<IWebDriver>("driver");
        var wait = new WebDriverWait(driver, _waitDurration);
        wait.Until(ExpectedConditions.UrlMatches(@"/plan/(\d+)"));
        driver.Url.Should().MatchRegex(@"/plan/(\d+)");
        wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(firstProcedure)));
    }

    [Then("I Assign Users To Plan Procedure")]
    public void IAssignUsersToPlanProcedure()
    {
        var driver = _context.Get<IWebDriver>("driver");
        var wait = new WebDriverWait(driver, _waitDurration);

        wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(firstProcedure))).Click();
        var usersInput = "/parent::div//div/input";
        var procedureDiv = firstProcedure.Replace("label", "div");
        wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(procedureDiv+usersInput))).Click();
        wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(procedureDiv+usersInput))).SendKeys("1");
        wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(procedureDiv+usersInput))).SendKeys(Keys.Enter);
        wait.Until(d => d.FindElement(
            (By.XPath(selectedUser))).Text!= "Select User to Assign");
        // select and add users
        // make sure the selected user is added
        var user=wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(selectedUser))).Text;
        user.Should().NotBeNullOrEmpty();
        user.Should().NotBeNullOrWhiteSpace();
        _context.Add("user", user);
    }

    [When("I Refresh")]
    public void IRefresh()
    {
        var driver = _context.Get<IWebDriver>("driver");
        var wait = new WebDriverWait(driver, _waitDurration);
        driver.Navigate().Refresh();
        wait.Until(ExpectedConditions.UrlMatches(@"/plan/(\d+)"));
        wait.Until(d=>!string.IsNullOrWhiteSpace(d.FindElement(
            (By.XPath(selectedUser))).Text));
    }

    [Then("The Selected Users Should Be Retained")]
    public void TheSelectedUsersShouldBeRetained()
    {
        var driver = _context.Get<IWebDriver>("driver");
        var wait = new WebDriverWait(driver, _waitDurration);
        wait.Until(ExpectedConditions.UrlMatches(@"/plan/(\d+)"));
       var actualText= wait.Until(ExpectedConditions.ElementIsVisible
            (By.XPath(selectedUser))).Text;
        actualText.Should().Be((string)_context["user"]);
        actualText.Should().NotBe("Select User to Assign");
    }
}