using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;


namespace LIHunter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to LIHunter!");

            //TODO: Add a user decision tree for basic info 
            string keywords = "Software Engineering";
            string city = "Chicago";
            string state = "Illinois";

            //TODO: Add logic for adding advanced info
            //TODO: Add a user decision tree for advanced info 
            string[] jobtitles = new string[1] { "fulltime" };
            string[] experiences = new string[1] { "entry" };
            string dateposted = "week";
            bool onlygeteasy = true;

            Console.WriteLine("Initializing LinkedIn Service.");
            LIService linkedInService = new LIService(keywords, city, state, jobtitles, experiences, dateposted, onlygeteasy);
            Console.WriteLine("Successfully initialized LinkedIn Service.");

            Console.WriteLine($"Searching for {0} jobs in {1}...", keywords, city);
            List<Job> searchResults = linkedInService.searchLI(linkedInService.FULLURL);
            Console.WriteLine("Completed LinkedIn Search for {0} jobs in {1}!", keywords, city);
            linkedInService.closeWindow();

            Console.WriteLine("Initializing Google Drive Service.");
            GoogleDriveService googleDriveService = new GoogleDriveService(searchResults);
            Console.WriteLine("Successfully initialized Google Drive Service.");

            Console.WriteLine("Writing search results to your google sheet...");
            string updateResponse = googleDriveService.CreateGoogleSheetsJobEntries(googleDriveService.Jobs);
            Console.WriteLine("Completed writing results to google sheets with a status of...");

            Console.WriteLine("Thank you for using LIHunter");
            Console.WriteLine("-----SESSION STATS-----");
            //TODO: Add session stats here
        }

    }
}