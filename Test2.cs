using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace testing;

public static class Test2
{
    public static void Run()
    {
        var driver = new ChromeDriver();

        driver.Navigate().GoToUrl("https://demowebshop.tricentis.com/");
        driver.FindElement(By.XPath("//a[@href='/gift-cards']")).Click();

        var products = driver.FindElements(By.XPath("//div[contains(@class, 'item-box')]"));
        foreach (var webElement in products)
        {
            var price = webElement.FindElement(By.XPath(".//div[contains(@class, 'price')]")).Text;
            if (price != null && double.Parse(price) > 99.0)
            {
                webElement.FindElement(By.XPath(".//input[contains(@class, 'product-box-add-to-cart-button')]")).Click();
            }
        }

        Thread.Sleep(2000);
        driver.Quit();
    }
}