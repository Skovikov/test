using System;
using System.Collections.Generic; //для IReadOnlyList
using OpenQA.Selenium; //сам селениум
using OpenQA.Selenium.Chrome; //для хрома
using OpenQA.Selenium.Remote; //для грида
using OpenQA.Selenium.Support.UI; //для ожидания
using NUnit.Framework; //nunit
using NUnit.Allure.Core; //для allure

namespace simbirsoft
{
    [AllureNUnit]
    public class UnitTest1 : LoginPage
    {
        static string Login = "nikskov73@gmail.com"; //свой логин
        static string Password = "simbirsoft12345"; //свой пароль
        static string From = "Команда Google"; //от кого пришли письма
        static string To = "skovikov73@yandex.ru"; //кому отправлять

        [Test]
        public void Main()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            IWebDriver browser = new RemoteWebDriver(new Uri("http://127.0.0.1:8090/wd/hub"), chromeOptions);
            browser.Manage().Window.Maximize(); //увелечение окна браузера
            GoLoginPage(browser);
            EnterLogin(Login);
            GoPasswordPage(browser);
            EnterPassord(Password);
            GoMailPage(browser);
            FindMails(From);
            SendMail(To);
            Assert.AreEqual(1, 1);
        }
    }
    public class LoginPage : PasswordPage
    {
        static IWebDriver browser;
        static WebDriverWait wait;
        static IWebElement element;
        protected static void GoLoginPage(IWebDriver driver)
        {
            browser = driver;
            wait = new WebDriverWait(browser, TimeSpan.FromSeconds(5)); //ожидание элемента в 5 сек
            browser.Navigate().GoToUrl("https://mail.google.com/"); //переход на страницу
        }
        protected static void EnterLogin(string login)
        {
            element = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("identifierId"))); //поиск почты
            element.SendKeys(login + OpenQA.Selenium.Keys.Enter); //ввод почты
        }
    }
    public class PasswordPage : MailPage
    {
        static IWebDriver browser;
        static WebDriverWait wait;
        static IWebElement element;
        protected static void GoPasswordPage(IWebDriver driver)
        {
            browser = driver;
            wait = new WebDriverWait(browser, TimeSpan.FromSeconds(5));
        }
        protected static void EnterPassord(string password)
        {
            element = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("password"))); //поиск пароля
            element.SendKeys(password + OpenQA.Selenium.Keys.Enter); //ввод пароля
        }
    }
    public class MailPage
    {
        static IWebDriver browser;
        static WebDriverWait wait;
        static IWebElement element;
        static IReadOnlyList<IWebElement> mails;
        protected static void GoMailPage(IWebDriver driver)
        {
            browser = driver;
            wait = new WebDriverWait(browser, TimeSpan.FromSeconds(10));
        }
        protected static void FindMails(string From)
        {
            mails = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("//div[@class='yW']//span[@name='" + From + "']"))); //формирование списка с нужными письмами
        }
        protected static void SendMail(string To)
        {
            element = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@class='aic']"))); //поиск кнопки написать
            element.Click(); // нажимаем на написать
            element = wait.Until(ExpectedConditions.ElementIsVisible(By.Name("to"))); //поиск получателя
            element.SendKeys(To); //ввод почты
            element = wait.Until(ExpectedConditions.ElementIsVisible(By.Name("subjectbox"))); //поиск темы
            element.SendKeys("Тестовое задание. Сковиков"); //ввод темы
            element = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='Ar Au']/div"))); //поиск основного текста
            element.SendKeys(mails.Count.ToString()); //ввод текста
            element = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@class='J-J5-Ji btA']"))); //поиск кнопки отправить
            element.Click(); //отправляем
        }
    }
}

