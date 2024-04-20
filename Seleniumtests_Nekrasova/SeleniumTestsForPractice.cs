using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Seleniumtests_Nekrasova;

public class SeleniumTestsForPractice
{
    public ChromeDriver driver;

    [SetUp]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox", "--start-maximized", "--disable-extensions");
        // Зайти в Chrome с помощью вебдрайвера
        driver = new ChromeDriver(options);
        
        // Неявное ожидание
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
        
        // Авторизация 
        Autorization();
    }
    
    [Test]
    public void Authorization()
    {
        var news = driver.FindElement(By.CssSelector("[data-tid='Title']"));
        // Проверяем, что находимся на нужной странице
        var currentUrl = driver.Url;
        currentUrl.Should().Be("https://staff-testing.testkontur.ru/news");
     }

    [Test]
    public void NavigationTests()
    {
         // Проверка, что есть боковое меню
         var sideMenu = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
         // Клик на "Сообщества"
         var community = driver.FindElements(By.CssSelector("[data-tid='Community']")).First(element => element.Displayed);
         community.Click();
         var communityTitle = driver.FindElement(By.CssSelector("[data-tid='Title']"));
         Assert.That(driver.Url == "https://staff-testing.testkontur.ru/communities", "На странице 'Сообщества' неправильный URL");
    }

    [Test]
    public void TestCreateEmptyCommunity()
    {
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/communities");

        // Ищем кнопку "СОЗДАТЬ". У элемента нет data-tid, поэтому ищем по классу
        var element = driver.FindElement(By.CssSelector("[class='sc-juXuNZ sc-ecQkzk WTxfS vPeNx']"));
        element.Click();
        
        // Создаем пустое сообщество. Нажать кнопку "Создать"
        var create = driver.FindElement(By.CssSelector("[data-tid='CreateButton']"));
        create.Click();
        
        // Ожидание, пока элемент появится
        WebDriverWait wait = new WebDriverWait(driver,TimeSpan.FromSeconds(5));
        wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='validationMessage']")));
        
        // Проверяем, сработала ли валидация
        var validationMessage = driver.FindElement(By.CssSelector("[data-tid='validationMessage']"));
        Assert.That(validationMessage.Text == "Поле обязательно для заполнения.", "Нет сообщения валидации");
    }

    [Test]
    public void TestOpenSearchFiles()
    {
        // Проверка, что есть боковое меню
        var sideMenu = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        // Клик на "Файлы"
        var files = driver.FindElements(By.CssSelector("[data-tid='Files']")).First(element => element.Displayed);
        files.Click();
        
        // Ожидание, пока откроется страница Файлы
        WebDriverWait wait = new WebDriverWait(driver,TimeSpan.FromSeconds(5));
        wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='Title']")));
        
        var filesTitle = driver.FindElement(By.CssSelector("[data-tid='Title']"));
        Assert.That(driver.Url == "https://staff-testing.testkontur.ru/files", "На странице 'Файлы' неправильный URL");
        
        // Клик по кнопке поиска (значок лупа). 
        var search = driver.FindElement(By.CssSelector("[data-tid='Search']"));
        search.Click(); 
        
        // Ожидание, пока откроется окно поиска
        wait = new WebDriverWait(driver,TimeSpan.FromSeconds(5));
        wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='Search']")));

        // Ищем текст подсказки в окне поиска файлов
        var searchElement = driver.FindElements(By.CssSelector("[data-tid='Search']"));
        Assert.That(searchElement.Count > 0, "Нет текста подсказки поиска");  
    }

    [Test]
    public void TestVersionPortalHasCloseButton()
    {
        // Клик по гиперссылке для открытия окна с описанием версий
        var currentVersion = driver.FindElement(By.CssSelector("[data-tid='Version']"));
        currentVersion.Click();   
        
        // Ожидание, пока откроется окно с версиями
        WebDriverWait wait = new WebDriverWait(driver,TimeSpan.FromSeconds(5));
        wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("[data-tid='modal-close']")));
        
        // Ищем кнопку закрытия
        var closeVersions = driver.FindElements(By.CssSelector("[data-tid='modal-close']"));
        Assert.That(closeVersions.Count > 0, "Кнопка закрытия не найдена");   
    }
    
    public void Autorization()
    {
        // Перейти по URL https://staff-testing.testkontur.ru
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
       
        // Ожидание, пока появится элемент UserName (он иногда сразу не находится)
        WebDriverWait wait = new WebDriverWait(driver,TimeSpan.FromSeconds(5));
        wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Username")));
        
        // Ввести логин и пароль
        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys("alljaz@yandex.ru");
        var password = driver.FindElement(By.Name("Password"));
        password.SendKeys("Rhs;jdybr2024");
        
        // Нажать кнопку "Войти"
        var enter = driver.FindElement(By.Name("button"));
        enter.Click();
        
        // Ждем загрузки главной страницы
        wait = new WebDriverWait(driver,TimeSpan.FromSeconds(5));
        wait.Until(ExpectedConditions.UrlContains("https://staff-testing.testkontur.ru/news"));
    }
    
    [TearDown]
    public void TearDown()
    {
        // Закрываем браузер и убиваем процесс драйвера
        driver.Close();
        driver.Quit();
    }
}