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

            //TODO: Add a user decision tree
            string keywords = "Software Engineering";
            string city = "Chicago";
            string state = "Illinois";

            Console.WriteLine("Initializing LinkedIn Service.");
            LIService linkedInService = new LIService(keywords, city, state);
            Console.WriteLine("Successfully initialized LinkedIn Service.");

            Console.WriteLine($"Searching for {0} jobs in {1}...", keywords, city);
            List<Job> searchResults = linkedInService.searchLI(linkedInService.FULLURL);
            Console.WriteLine("Completed LinkedIn Search for {0} jobs in {1}!", keywords, city);

            Console.WriteLine("Writing search results to your google sheet...");
            GoogleDriveService googleDriveService = new GoogleDriveService();
            Console.WriteLine("Completed writing results to google sheets with a status of...");

            Console.WriteLine("Thank you for using LIHunter");
            Console.WriteLine("-----SESSION STATS-----");
            //TODO: Add session stats here
        }

    }
}