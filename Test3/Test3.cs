using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace testing.Test3;

public class UserRegistration
{
    public static (string email, string password) CreateUser()
    {
        IWebDriver driver = new ChromeDriver();
        driver.Manage().Window.Maximize();
        driver.Navigate().GoToUrl("https://demowebshop.tricentis.com/");
        driver.FindElement(By.XPath("//div[@class='header-links']//a[@href='/login']")).Click();
        driver.FindElement(By.XPath("//div[@class='buttons']//input[@class='button-1 register-button']")).Click();
        
        string email = $"testuser{Guid.NewGuid().ToString().Substring(0, 5)}@mail.com";
        string password = $"Test{Guid.NewGuid().ToString().Substring(0, 5)}";
        
        driver.FindElement(By.XPath("//input[@id='gender-male']")).Click();
        driver.FindElement(By.XPath("//input[@id='FirstName']")).SendKeys("Test");
        driver.FindElement(By.XPath("//input[@id='LastName']")).SendKeys("User");
        driver.FindElement(By.XPath("//input[@id='Email']")).SendKeys(email);
        driver.FindElement(By.XPath("//input[@id='Password']")).SendKeys(password);
        driver.FindElement(By.XPath("//input[@id='ConfirmPassword']")).SendKeys(password);
        driver.FindElement(By.XPath("//input[@id='register-button']")).Click();
        
        driver.Quit();
        
        return (email, password);
    }
}

[TestFixture]
public class Test3
{
    private static string Email;
    private static string Password;
    private IWebDriver driver;
    private WebDriverWait wait;

    [OneTimeSetUp]
    public void SetUpClass()
    {
        (Email, Password) = UserRegistration.CreateUser();
    }

    [SetUp]
    public void SetUp()
    {
        var options = new ChromeOptions();
        options.AddArguments("--start-maximized");
        driver = new ChromeDriver(options);
        driver.Navigate().GoToUrl("https://demowebshop.tricentis.com/");
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
    }

    private void Login()
    {
        driver.FindElement(By.XPath("//div[@class='header-links']//a[@href='/login']")).Click();
        driver.FindElement(By.XPath("//input[@id='Email']")).SendKeys(Email);
        driver.FindElement(By.XPath("//input[@id='Password']")).SendKeys(Password);
        driver.FindElement(By.XPath("//input[@class='button-1 login-button']")).Click();
    }

    private void AddItemsToCart(string filename)
    {
        wait.Until(d => driver.FindElement(By.XPath(
            "//div[@class='block block-category-navigation']//a[@href='/digital-downloads']"
        )).Displayed);
        
        driver.FindElement(By.XPath(
            "//div[@class='block block-category-navigation']//a[@href='/digital-downloads']"
        )).Click();
        
        string filePath = Path.Combine(Directory.GetCurrentDirectory(),  "../../../Test3", filename);
        Assert.That(File.Exists(filePath), Is.True);
        
        var products = File.ReadAllLines(filePath);
        foreach (var product in products)
        {
            driver.FindElement(By.XPath($"//a[text()='{product.Trim()}']")).Click();
            driver.FindElement(By.XPath("//input[contains(@class, 'add-to-cart-button')]")).Click();
            driver.Navigate().Back();
        }
    }

    private void Checkout()
    {
        driver.FindElement(By.XPath("//div[@class='header-links']//a[@href='/cart']")).Click();
        driver.FindElement(By.XPath("//input[@id='termsofservice']")).Click();
        driver.FindElement(By.XPath("//button[@id='checkout']")).Click();
        if (driver.FindElements(By.XPath("//select[@id='billing-address-select']")).Count == 0)
        {
            var dropdownElement = driver.FindElement(By.XPath("//select[@id='BillingNewAddress_CountryId']"));
            new SelectElement(dropdownElement).SelectByValue("156");
            driver.FindElement(By.XPath("//input[@id='BillingNewAddress_City']")).SendKeys("City");
            driver.FindElement(By.XPath("//input[@id='BillingNewAddress_Address1']")).SendKeys("Street");
            driver.FindElement(By.XPath("//input[@id='BillingNewAddress_ZipPostalCode']")).SendKeys("99999");
            driver.FindElement(By.XPath("//input[@id='BillingNewAddress_PhoneNumber']")).SendKeys("868888888");
        }
        wait.Until(d => driver.FindElement(By.XPath(
            "//input[@class='button-1 new-address-next-step-button']"
        )).Displayed);
        driver.FindElement(By.XPath("//input[@class='button-1 new-address-next-step-button']")).Click();
        wait.Until(d => driver.FindElement(By.XPath(
            "//input[@class='button-1 payment-method-next-step-button']"
        )).Displayed);
        driver.FindElement(By.XPath("//input[@class='button-1 payment-method-next-step-button']")).Click();
        wait.Until(d => driver.FindElement(By.XPath(
            "//input[@class='button-1 payment-info-next-step-button']"
        )).Displayed);
        driver.FindElement(By.XPath("//input[@class='button-1 payment-info-next-step-button']")).Click();
        wait.Until(d => driver.FindElement(By.XPath(
            "//input[@class='button-1 confirm-order-next-step-button']"
        )).Displayed);
        driver.FindElement(By.XPath("//input[@class='button-1 confirm-order-next-step-button']")).Click();
        
        wait.Until(d => driver.FindElement(By.XPath(
            "//div[@class='section order-completed']//strong"
        )).Displayed);
        string orderMessage = driver.FindElement(By.XPath("//div[@class='section order-completed']//strong")).Text;
        Assert.That(orderMessage, Is.EqualTo("Your order has been successfully processed!"));
    }

    [Test]
    public void TestPurchase1()
    {
        Login();
        AddItemsToCart("data1.txt");
        Checkout();
    }
    
    [Test]
    public void TestPurchase2()
    {
        Login();
        AddItemsToCart("data2.txt");
        Checkout();
    }
    
    [TearDown]
    public void TearDown()
    {
        driver.Quit();
    }
}

