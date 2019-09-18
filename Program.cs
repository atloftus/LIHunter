using System;

namespace LIHunter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initializing LI Service");
            LIService linkedInService = new LIService();
            Console.WriteLine("Navigated to Google");
        }


        public static string createBaseSearchString(string keywords, string city, string state)
        {
            string keywordsWithoutSpaces = keywords.Replace(" ", "%20");
            string requiredParams = keywordsWithoutSpaces + "&location=" + city +  "%2C%20" + state + "%2C%20United%20States&";//[&f_JT=(F for full time and F%2CC for contract)][&f_E=(2 for entry and 2%2C3 entry and associate][&f_TP=1(nothing for day, %2C2 for week, %2C2%2C3%2C4 for month)]

            return "";
        }
    }
}
