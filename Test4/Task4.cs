namespace testing.Test4;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

[TestFixture]
public class Test4
{
    private IWebDriver driver;
    private WebDriverWait wait;
    private readonly int numberOfBooksToAdd = 5;
    private List<string> bookTitles;
    private List<string> bookUrls;

    [SetUp]
    public void SetUp()
    {
        var options = new ChromeOptions();
        options.AddArguments("--start-maximized");
        driver = new ChromeDriver(options);
        driver.Navigate().GoToUrl("https://demowebshop.tricentis.com/");
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        
        driver.FindElement(By.XPath("//a[@href='/books']")).Click();
        wait.Until(d => d.FindElement(By.XPath("//div[@class='product-grid']")).Displayed);
        
        var books = driver.FindElements(By.XPath("//h2[@class='product-title']/a"));
        bookTitles = books.Select(x => x.Text).Take(numberOfBooksToAdd).ToList();
        bookUrls = books.Select(el => el.GetAttribute("href")).Take(numberOfBooksToAdd).ToList();
    }

    private void AddBooksToCompareList()
    {
        for (int i = 0; i < numberOfBooksToAdd; i++)
        {
            driver.Navigate().GoToUrl(bookUrls[i]);
            wait.Until(d => d.FindElement(By.XPath("//input[@value='Add to compare list']")).Displayed);
            driver.FindElement(By.XPath("//input[@value='Add to compare list']")).Click();
            wait.Until(d => d.FindElement(By.XPath("//tr[@class='product-name']")).Displayed);
        }
    }

    private void VerifyFirstBookRemoved()
    {
        bool isBookPresent = driver.FindElements(By.XPath($"//tr[@class='product-name']//td//a[text()='{bookTitles[0]}']")).Count > 0;
        Assert.That(isBookPresent, Is.False, "The first book is still visible in the compare list.");
    }

    [Test]
    public void TestCompareList()
    {
        AddBooksToCompareList();
        VerifyFirstBookRemoved();
    }
    
    [TearDown]
    public void TearDown()
    {
        driver.Quit();
    }
}

