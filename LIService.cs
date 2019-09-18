using System;
using System.IO;
using System.Threading;
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
        public string BaseSearchParams { get; set; }
        public string AdvancedSearchParams { get; set; }
        public string FULLURL { get; set; }

        public IWebDriver Driver { get; set; }

        public LIService()
        {
            //Construct search string
            BaseURL = @"https://www.linkedin.com/jobs/search?keywords=";

            //TODO: Replace the static string with dynamic values 
            setBaseSearchParams("Software Engineer", "Chicago", "Illinois");
            setAdvancedSearchParams(new string[2] { "fulltime", "contract" }, new string[2] { "entry", "associate" }, "day");
            FULLURL = BaseURL + BaseSearchParams + AdvancedSearchParams;

            Driver = new ChromeDriver(ChromeDriverRelativePath);         
            Driver.Navigate().GoToUrl(FULLURL);
            //https://www.linkedin.com/jobs/search?keywords=Software%20Engineer&location=Chicago%2C%20Illinois%2C%20United%20States&&f_JT=F%2CC&f_E=2%2C3&f_TP=1


            IWebElement element;
            long scrollHeight = 0;

            do {
                //TODO:Scrape all relevant info here
                IJavaScriptExecutor js = (IJavaScriptExecutor) Driver;
                var newScrollHeight = (long)js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight); return document.body.scrollHeight;");
                if (newScrollHeight == scrollHeight) break;
                else scrollHeight = newScrollHeight;

                element = Driver.FindElement(By.XPath("/html/body/main/section[1]/button"));
                Thread.Sleep(1000);
                element.Click();
                Thread.Sleep(1000);
            } while (element != null);

            //Get all results 
            //Scrape all results
            //Cast all results to model object instances
            //In a method return a collection of all the instances
        }

        public void setBaseSearchParams(string keywords, string city, string state)
        {
            string keywordsWithoutSpaces = keywords.Replace(" ", "%20");
            BaseSearchParams = keywordsWithoutSpaces + "&location=" + city + "%2C%20" + state + "%2C%20United%20States&";//[&f_JT=(F for full time and F%2CC for contract)][&f_E=(2 for entry and 2%2C3 entry and associate][&f_TP=1(nothing for day, %2C2 for week, %2C2%2C3%2C4 for month)]
        }


        public void setAdvancedSearchParams(string[] jobtitles, string[] experiences, string timesposted)
        {
            int counter = 0;
            foreach(string job in jobtitles)
            {
                if (counter == 0) AdvancedSearchParams += "&f_JT=";
                else AdvancedSearchParams += "%2";

                if (job.Contains("full")) AdvancedSearchParams += "F";
                else if (job.Contains("contract")) AdvancedSearchParams += "CC";
                else if (job.Contains("part")) AdvancedSearchParams += "P";

                counter++;
            }

            counter = 0;

            foreach (string exp in experiences)
            {
                if (counter == 0) AdvancedSearchParams += "&f_E=";
                else AdvancedSearchParams += "%2";

                if (exp.Contains("entry")) AdvancedSearchParams += "2";
                else if (exp.Contains("associate")) AdvancedSearchParams += "C3";

                counter++;
            }

            if (timesposted.Contains("month")) AdvancedSearchParams += "&f_TP=1";
            else if (timesposted.Contains("week")) AdvancedSearchParams += "&f_TP=1";
            else AdvancedSearchParams += "&f_TP=1";
        }
    }
}
