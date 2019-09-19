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
        #region PROPERTIES
        public string ChromeDriverRelativePath = (Directory.GetCurrentDirectory().Split("LIHunter"))[0] + @"\LIHunter\chromedriver_win32";
        public string UserName { get; set; }
        public string Password { get; set; }
        public string BaseURL { get { return @"https://www.linkedin.com/jobs/search?keywords="; } }
        public string BaseSearchParams { get; set; }
        public string AdvancedSearchParams { get; set; }
        public string FULLURL { get; set; }
        public IWebDriver Driver { get; set; }
        public List<Job> JobResults { get; set; } = new List<Job>();
        public bool OnlyGetEasyAppy { get; set; } = true;
        #endregion


        #region CONSTRUCTORS
        public LIService(string keywords, string city, string state)
        {
            setBaseSearchParams(keywords, city, state);
            FULLURL = BaseURL + BaseSearchParams;
        }

        public LIService(string keywords, string city, string state, string[] jobtitles, string[] experiences, string timesposted, bool onlygeteasy) : this(keywords, city, state)
        {
            setAdvancedSearchParams(jobtitles, experiences, timesposted);
            OnlyGetEasyAppy = onlygeteasy;
            FULLURL += AdvancedSearchParams;
        }
        #endregion


        #region METHODS 
        /// <summary>
        ///     This method takes in a string url and searches LinkedIn with it. It then scraps all of the jobs it finds and casts them
        ///     to Job objects and returns them in a list.
        /// </summary>
        /// <param name="url"></param>
        public List<Job> searchLI(string url)
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--incognito");
            Driver = new ChromeDriver(ChromeDriverRelativePath, options);
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

            foreach (IWebElement elm in JobCards)
            {
                if (OnlyGetEasyAppy)
                {
                    try
                    {
                        var easyApply = elm.FindElement(By.ClassName("job-result-card__easy-apply-label"));
                        if (easyApply != null)
                        {
                            string link = elm.FindElement(By.TagName("a")).GetAttribute("href");
                            string[] splitInfo = elm.Text.Split("\r\n");
                            if (splitInfo.Length >= 5)
                            {
                                string dateposted = splitInfo[4].Replace("Easy Apply", "");
                                Job holderJob = new Job(splitInfo[1], splitInfo[0], splitInfo[2], link, dateposted, splitInfo[3], true);
                                JobResults.Add(holderJob);
                            }
                        }
                    }
                    catch (OpenQA.Selenium.NoSuchElementException ex) { }
                } else
                {
                    string link = elm.FindElement(By.TagName("a")).GetAttribute("href");
                    string[] splitInfo = elm.Text.Split("\r\n");
                    if (splitInfo.Length >= 5)
                    {
                        Job holderJob = new Job(splitInfo[1], splitInfo[0], splitInfo[2], link, splitInfo[4], splitInfo[3], false);
                        JobResults.Add(holderJob);
                    }
                }
            }
            return JobResults;
        }


        /// <summary>
        ///     This method adds the required params to the search params for LinkedIn.
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        public void setBaseSearchParams(string keywords, string city, string state)
        {
            string keywordsWithoutSpaces = keywords.Replace(" ", "%20");
            BaseSearchParams = keywordsWithoutSpaces + "&location=" + city + "%2C%20" + state + "%2C%20United%20States&";
        }


        /// <summary>
        ///     This method adds the not required search params to the LinkedIn search url.
        /// </summary>
        /// <param name="jobtitles"></param>
        /// <param name="experiences"></param>
        /// <param name="timesposted"></param>
        public void setAdvancedSearchParams(string[] jobtitles, string[] experiences, string timesposted)
        {
            int counter = 0;
            foreach (string job in jobtitles)
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

            if (timesposted.Contains("day")) AdvancedSearchParams += "&f_TP=1";
            else if (timesposted.Contains("month")) AdvancedSearchParams += "&f_TP=1%2C2";
            else if (timesposted.Contains("week")) AdvancedSearchParams += "&f_TP=1%2C2%2C3%2C4";
        }


        public void closeWindow()
        {
            Driver.Close();
        }
        #endregion
    }
}