using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LIHunter
{
    public class LIService
    {
        public string ChromeDriverRelativePath = (Directory.GetCurrentDirectory().Split("LIHunter"))[0] + @"\LIHunter\chromedriver_win32";
        public string UserName { get; set; }
        public string Password { get; set; }

        public string BaseURL { get; set; }
        public string SearchURL { get; set; }

        public IWebDriver Driver { get; set; }

        public LIService()
        {
            //Construct search string

            //Initialize chromedriver
            //Driver = new ChromeDriver(@"C:\Users\AlexanderLoftus\Desktop\chromedriver_win32");
            Driver = new ChromeDriver(ChromeDriverRelativePath);
            
            Driver.Navigate().GoToUrl("http:www.google.com");
            IWebElement element = Driver.FindElement(By.Name("q"));
            element.SendKeys("executeautomation");

            //Navigate to LI
            //Get all results 
            //Scrape all results
            //Cast all results to model object instances
            //In a method return a collection of all the instances
        }
    }
}
