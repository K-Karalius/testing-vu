using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace testing;

public static class Test2
{
    public static void RunTest21()
    {
        var driver = new ChromeDriver();
        driver.Manage().Window.Maximize();
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

        driver.Navigate().GoToUrl("https://demoqa.com/");
        
        driver.FindElement(By.XPath(
                "//div[@class='card mt-4 top-card']//div[@class='card-body']//h5[text()='Widgets']/ancestor::div[@class='card mt-4 top-card']"
            )
        ).Click();

        driver.FindElement(By.XPath(
                "//div[@class='element-list collapse show']//li[.//span[@class='text' and text()='Progress Bar']]"
            )
        ).Click();

        var progressBarContainer = driver.FindElement(By.XPath("//div[@id='progressBarContainer']"));
        var progressBar = progressBarContainer.FindElement(By.XPath(".//div[@class='progress-bar bg-info']"));
        var progressBarButtonStart = progressBarContainer.FindElement(By.XPath(".//button[@id='startStopButton']"));
        progressBarButtonStart.Click();
        
        wait.Until(d =>
        {
            string progressValue = progressBar.GetAttribute("aria-valuenow");
            return !string.IsNullOrEmpty(progressValue) && Convert.ToInt32(progressValue) == 100;
        });
        
        progressBarContainer.FindElement(By.XPath(".//button[@id='resetButton']")).Click();
        string progressValue = progressBar.GetAttribute("aria-valuenow");
        Console.WriteLine(progressValue);
      
        driver.Quit();
    }

    public static void RunTest22()
    {
        var driver = new ChromeDriver();
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        driver.Manage().Window.Maximize();
        
        driver.Navigate().GoToUrl("https://demoqa.com/");
        
        driver.FindElement(By.XPath(
                "//div[@class='card mt-4 top-card']//div[@class='card-body']//h5[text()='Elements']/ancestor::div[@class='card mt-4 top-card']"
            )
        ).Click();
        
        driver.FindElement(By.XPath(
                "//div[@class='element-list collapse show']//li[.//span[@class='text' and text()='Web Tables']]"
            )
        ).Click();

        var addButton = driver.FindElement(By.XPath("//button[@id='addNewRecordButton']"));
        var pageCount = driver.FindElement(By.XPath("//span[@class='-totalPages']"));

        int increment = 1;
        wait.Until(d => 
        {
            addButton.Click();
            driver.FindElement(By.XPath("//input[@id='firstName']")).SendKeys("Kristupas" + increment);
            driver.FindElement(By.XPath("//input[@id='lastName']")).SendKeys("Karalius" + increment);
            driver.FindElement(By.XPath("//input[@id='userEmail']")).SendKeys("kristupas" + increment + "@gmail.com");
            driver.FindElement(By.XPath("//input[@id='age']")).SendKeys((20 + increment).ToString());
            driver.FindElement(By.XPath("//input[@id='salary']")).SendKeys((20000 + increment).ToString());
            driver.FindElement(By.XPath("//input[@id='department']")).SendKeys("department" + increment);
            driver.FindElement(By.XPath("//button[@id='submit']")).Click();
            ++increment; 
            return pageCount.Text == "2";
        });
        
        driver.FindElement(By.XPath("//div[@class='-next']//button[@class='-btn']")).Click();

        var row = driver.FindElement(By.XPath("//div[@role='row']"));
        row.FindElement(By.XPath("//span[@title='Delete']")).Click();
        
        var pageInfo = driver.FindElement(By.XPath("//span[@class='-pageInfo']"));
        var currentPage = pageInfo.FindElement(By.XPath(".//input[@aria-label='jump to page']")).GetAttribute("value");
        var totalPages = pageInfo.FindElement(By.XPath(".//span[@class='-totalPages']")).Text;
        
        
        Console.WriteLine("Current page: " + currentPage);
        Console.WriteLine("Total pages: " + totalPages);
        
        driver.Quit();
    }
}