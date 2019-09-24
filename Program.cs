using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using Hunters;


namespace LIHunter
{
    /// <summary>
    ///     This class is the main porgram class for thsi .Net Core Desktop application thats promary purpose is to hold
    ///     the main method.
    /// </summary>
    class Program
    {
        /// <summary>
        ///     This is the main method for the .Net Core Application that initializes all fo the services and 
        ///     makes all of the appropriate calls.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.Write("Would you like to update Indeed(y/n)? ");
            string inInput = Console.ReadLine();
            Console.Write("Would you like to update LinkedIn(y/n)? ");
            string liInput = Console.ReadLine();

            if ((inInput.Contains('y')) && (liInput.Contains('y'))) runBoth();
            else if (inInput.Contains('y')) runIndeed();
            else if (liInput.Contains('y')) runLinkedIn();
                               
        }


        /// <summary>
        /// 
        /// </summary>
        public static void runLinkedIn()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("Welcome to LIHunter! Initializing LinkedIn Service...");
            Hunters.LIService linkedInService = new Hunters.LIService();
            Console.WriteLine("Successfully initialized LinkedIn Service.");

            Console.WriteLine($"Searching for jobs...");
            List<Hunters.Job> searchResults = linkedInService.searchLI();
            Console.WriteLine("Completed all LinkedIn Searchs!");

            Console.WriteLine("Initializing Google Drive Service...");
            Hunters.GoogleDriveService googleDriveService = new Hunters.GoogleDriveService(searchResults);
            Console.WriteLine("Successfully initialized Google Drive Service.");

            Console.WriteLine("Writing search results to your google sheet...");
            string updateResponse = googleDriveService.CreateGoogleSheetsLIJobEntries(googleDriveService.Jobs);
            Console.WriteLine("Completed writing results to google sheets.");
            stopwatch.Stop();

            Console.WriteLine("Thank you for using LIHunter!");
            Console.WriteLine("-----SESSION STATS-----");
            Console.WriteLine("Number of Jobs Found on LinkedIn: " + searchResults.Count);
            Console.WriteLine("Number of new Jobs added to the Google Sheet: " + updateResponse);
            Console.WriteLine("Application Run Time: {0:mm\\:ss}", stopwatch.Elapsed);
            Console.WriteLine("-----------------------");
        }


        /// <summary>
        /// 
        /// </summary>
        public static void runIndeed()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("Welcome to INHunter! Initializing Indeed Service...");
            Hunters.INService indeedService = new Hunters.INService();
            Console.WriteLine("Successfully initialized Indeed Service.");

            Console.WriteLine($"Searching for jobs...");
            List<Hunters.Job> searchResults = indeedService.searchIN();
            Console.WriteLine("Completed all Indeed Searchs!");

            Console.WriteLine("Initializing Google Drive Service...");
            Hunters.GoogleDriveService googleDriveService = new Hunters.GoogleDriveService(searchResults);
            Console.WriteLine("Successfully initialized Google Drive Service.");

            Console.WriteLine("Writing search results to your google sheet...");
            string updateResponse = googleDriveService.CreateGoogleSheetsINJobEntries(googleDriveService.Jobs);
            Console.WriteLine("Completed writing results to google sheets.");
            stopwatch.Stop();

            Console.WriteLine("Thank you for using INHunter!");
            Console.WriteLine("-----SESSION STATS-----");
            Console.WriteLine("Number of Jobs Found on Indeed: " + searchResults.Count);
            Console.WriteLine("Number of new Jobs added to the Google Sheet: " + updateResponse);
            Console.WriteLine("Application Run Time: {0:mm\\:ss}", stopwatch.Elapsed);
            Console.WriteLine("-----------------------");
        }


        /// <summary>
        /// 
        /// </summary>
        public static void runBoth()
        {
            runLinkedIn();
            runIndeed();
        }
    }
}