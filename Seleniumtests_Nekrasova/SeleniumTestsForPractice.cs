using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Seleniumtests_Nekrasova;

public class SeleniumTestsForPractice
{
    [Test]
    public void Authorization()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox", "--start-maximized", "--disable-extensions");
        
        var driver = new ChromeDriver(options);
        
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
        Thread.Sleep(3000);

        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys("alljaz@yandex.ru");

        var password = driver.FindElement(By.Name("Password"));
        password.SendKeys("Rhs;jdybr2024");
        
        var enter = driver.FindElement(By.Name("button"));
        enter.Click();
        
        

        var currentUrl = driver.Url;
        Assert.That(currentUrl == "https://staff-testing.testkontur.ru/news");
        
        driver.Quit();

    }
}