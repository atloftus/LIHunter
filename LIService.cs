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
        public string BaseURL { get {return @"https://www.linkedin.com/jobs/search?keywords=";}}
        public string BaseSearchParams { get; set; }
        public string AdvancedSearchParams { get; set; }
        public string FULLURL { get; set; }
        public IWebDriver Driver { get; set; }
        public List<Job> JobResults { get; set; } = new List<Job>();


        public LIService(string keywords, string city, string state)
        {
            //TODO: Replace the static string with dynamic values 
            /*
            setBaseSearchParams("Software Engineer", "Chicago", "Illinois");
            setAdvancedSearchParams(new string[1] {"contract" }, new string[1] { "entry" }, "day");
            */
            setBaseSearchParams("Software Engineer", "Chicago", "Illinois");
            FULLURL = BaseURL + BaseSearchParams;
            searchLI(FULLURL);
        }

        public LIService(string keywords, string city, string state, string[] jobtitles, string[] experiences, string timesposted)
        {
            setBaseSearchParams("Software Engineer", "Chicago", "Illinois");
            setAdvancedSearchParams(new string[1] {"contract" }, new string[1] { "entry" }, "day");
            FULLURL = BaseURL + BaseSearchParams + AdvancedSearchParams;
            searchLI(FULLURL);
        }


        public void searchLI(string url)
        {
            Driver = new ChromeDriver(ChromeDriverRelativePath);
            Driver.Navigate().GoToUrl(url);

            IWebElement element;
            long scrollHeight = 0;

            do
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
                var newScrollHeight = (long)js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight); return document.body.scrollHeight;");
                if (newScrollHeight == scrollHeight) break;
                else scrollHeight = newScrollHeight;

                try
                {
                    element = Driver.FindElement(By.XPath("/html/body/main/section[1]/button"));
                    Thread.Sleep(1000);
                    element.Click();
                    Thread.Sleep(1000);
                }
                catch (OpenQA.Selenium.NoSuchElementException ex) { break; }
            } while (element != null);

            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> JobCards = Driver.FindElements(By.XPath("/html/body/main/section[1]/ul/li"));

            //Cast all results to model object instances
            foreach (IWebElement elm in JobCards)
            {
                string link = elm.FindElement(By.TagName("a")).GetAttribute("href");
                string[] splitInfo = elm.Text.Split("\r\n");
                Job holderJob = new Job(splitInfo[1], splitInfo[0], splitInfo[2], link, splitInfo[4], splitInfo[3]);
                JobResults.Add(holderJob);
            }
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
