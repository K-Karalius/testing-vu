using OpenQA.Selenium.Chrome;

namespace testing;

public static class Test1
{
    public static void Run()
    {
        var driver = new ChromeDriver();

        driver.Navigate().GoToUrl("https://www.google.com");

        Thread.Sleep(2000);

        driver.Quit();
    }
}