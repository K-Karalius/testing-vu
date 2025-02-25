using OpenQA.Selenium.Chrome;

namespace testing;

public static class Test0
{
    public static void Run()
    {
        var options = new ChromeOptions();
        options.AddArguments("--start-maximized");
        var driver = new ChromeDriver(options);
        
        driver.Navigate().GoToUrl("https://www.google.com");
        driver.Quit();
    }
}