using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace testing;

public static class Test1
{
    public static void Run()
    {
        var driver = new ChromeDriver();
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
        driver.Manage().Window.Maximize();
        driver.Navigate().GoToUrl("https://demowebshop.tricentis.com/");
        
        AddGiftCards(driver, wait);
        AddJewelry(driver, wait);
        AddWishlistItemsToCart(driver);
        
        // confirm sub-total
        var totals = driver.FindElement(By.XPath("//div[@class='total-info']"));
        var tableRow = totals.FindElement(By.XPath(".//tr[.//span[text()='Sub-Total:']]"));
        var priceSpan = tableRow.FindElement(By.XPath(".//span[@class='product-price']"));
        Console.WriteLine($"Total Price: {priceSpan.Text}");
        driver.Quit();

    }

    private static void AddGiftCards(ChromeDriver driver, WebDriverWait wait)
    {
        var category = driver.FindElement(By.XPath("//div[@class='block block-category-navigation']"));
        category.FindElement(By.XPath("//a[@href='/gift-cards']")).Click();
        
        var productItem = driver.FindElement(By.XPath(
            "(//div[@class='product-item']//span[@class='price actual-price' and number(text()) > 99]/ancestor::div[@class='product-item'])[1]"
        ));
        productItem.FindElement(By.XPath(".//input[@class='button-2 product-box-add-to-cart-button']")).Click();

        WaitForItemDisplay(wait, "//input[@id='giftcard_4_RecipientName']");
        driver.FindElement(By.XPath("//input[@id='giftcard_4_RecipientName']")).SendKeys("John");
        driver.FindElement(By.XPath("//input[@id='giftcard_4_SenderName']")).SendKeys("John");
        driver.FindElement(By.XPath("//input[@id='addtocart_4_EnteredQuantity']")).Clear();
        driver.FindElement(By.XPath("//input[@id='addtocart_4_EnteredQuantity']")).SendKeys("5000");
        driver.FindElement(By.XPath("//input[@id='add-to-cart-button-4']")).Click();
        WaitForItemDisplay(wait, "//div[@id='bar-notification']");
        driver.FindElement(By.XPath("//input[@id='add-to-wishlist-button-4']")).Click();
    }

    private static void AddJewelry(ChromeDriver driver, WebDriverWait wait)
    {
        var category = driver.FindElement(By.XPath("//div[@class='block block-category-navigation']"));
        category.FindElement(By.XPath("//a[@href='/jewelry']")).Click();
        driver.FindElement(By.XPath("//a[@href='/create-it-yourself-jewelry']")).Click();
        
        var dropdownElement = driver.FindElement(By.XPath("//select[@id='product_attribute_71_9_15']"));
        var dropdown = new SelectElement(dropdownElement);
        dropdown.SelectByValue("47");
        
        driver.FindElement(By.XPath("//input[@id='product_attribute_71_10_16']")).SendKeys("80");
        driver.FindElement(By.XPath("//input[@id='product_attribute_71_11_17_50']")).Click();
        driver.FindElement(By.XPath("//input[@id='addtocart_71_EnteredQuantity']")).Clear();
        driver.FindElement(By.XPath("//input[@id='addtocart_71_EnteredQuantity']")).SendKeys("26");
        driver.FindElement(By.XPath("//input[@id='add-to-cart-button-71']")).Click();
        WaitForItemDisplay(wait, "//div[@id='bar-notification']");
        driver.FindElement(By.XPath("//input[@id='add-to-wishlist-button-71']")).Click();
    }

    private static void AddWishlistItemsToCart(ChromeDriver driver)
    {
        var headerDiv = driver.FindElement(By.XPath("//div[@class='header']"));
        headerDiv.FindElement(By.XPath(".//a[contains(@href, '/wishlist')]")).Click();
        
        var cartItemRow = driver.FindElement(By.XPath("//tr[@class='cart-item-row']"));
        var checkBoxes = cartItemRow.FindElements(By.XPath(".//input[@name='addtocart']"));
        foreach (var checkBox in checkBoxes)
        {
            checkBox.Click();
        }
        
        driver.FindElement(By.XPath("//input[@class='button-2 wishlist-add-to-cart-button']")).Click();
    }
    
    private static void WaitForItemDisplay(WebDriverWait wait, string path)
    {
        wait.Until(d => d.FindElement(By.XPath(path)).Displayed);
    }
}