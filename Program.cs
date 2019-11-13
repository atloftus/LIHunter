using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using Hunters;
using Killers;
using System.Threading.Tasks;

namespace LIHunter
{
    /// <summary>
    ///     This class is the main porgram class for this .Net Core Desktop application thats primary purpose is to hold
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
            Console.Write("Welcome to the Application Manger!");
            Console.Write("Would you like to hunt jobs(y/n)? ");
            string huntInput = Console.ReadLine();
            Console.Write("Would you like to kill jobs(y/n)? ");
            string killInput = Console.ReadLine();

            if ((huntInput.Contains('y')) && (killInput.Contains('y'))) runAll();
            else if (huntInput.Contains('y')) huntJobs();
            else if (killInput.Contains('y')) killJobs();
        }



        /// <summary>
        /// 
        /// </summary>
        public static void runAll()
        {
            huntJobs();
            killJobs();
        }


        /// <summary>
        /// 
        /// </summary>
        public static void killJobs()
        {
            Console.Write("Would you like to kill Indeed(y/n)? ");
            string inInput = Console.ReadLine();
            Console.Write("Would you like to kill LinkedIn(y/n)? ");
            string liInput = Console.ReadLine();

            if ((inInput.Contains('y')) && (liInput.Contains('y'))) runBothKills();
            else if (inInput.Contains('y')) runIndeedKill();
            else if (liInput.Contains('y')) runLinkedInKill();
        }


        /// <summary>
        /// 
        /// </summary>
        public static void runBothKills()
        {
            runLinkedInKill();
            runIndeedKill();
        }


        /// <summary>
        /// 
        /// </summary>
        public static void runLinkedInKill()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("Welcome to LIKiller! Initializing LinkedIn Service...");
            Killers.LIKiller liKiller = new LIKiller();
            Console.WriteLine("Successfully initialized LinkedIn Killer.");

            Console.WriteLine("Applying to jobs...");
            int[] results = liKiller.applyToJobs();
            Console.WriteLine("Number of SUCCESSful applications on LinkedIn: " + results[0]);
            Console.WriteLine("Number of FAILed applications on LinkedIn: " + results[1]);
            Console.WriteLine("Completed all LinkedIn Searchs!");

            Console.WriteLine("Initializing Google Drive Service...");
            Killers.GoogleDriveService googleDriveService = new Killers.GoogleDriveService();
            Console.WriteLine("Successfully initialized Google Drive Service.");

            Console.WriteLine("Writing application results to your google sheet...");
            //TODO: Need to add calls to udpdate present entries
            string updateResponse = googleDriveService.CreateGoogleSheetsLIJobEntries(googleDriveService.Jobs);
            Console.WriteLine("Completed writing results to google sheets.");
            stopwatch.Stop();

            Console.WriteLine("Thank you for using LIKiller!");
            Console.WriteLine("-----SESSION STATS-----");
            Console.WriteLine("Number of SUCCESSful applications on LinkedIn: " + results[0]);
            Console.WriteLine("Number of FAILed applications on LinkedIn: " + results[1]);
            Console.WriteLine("Application Run Time: {0:mm\\:ss}", stopwatch.Elapsed);
            Console.WriteLine("-----------------------");
        }


        /// <summary>
        /// 
        /// </summary>
        public static void runIndeedKill()
        {
            //TODO: Finish this method
        }



        public static void huntJobs()
        {
            Console.Write("Would you like to update Indeed(y/n)? ");
            string inInput = Console.ReadLine();
            Console.Write("Would you like to update LinkedIn(y/n)? ");
            string liInput = Console.ReadLine();

            if ((inInput.Contains('y')) && (liInput.Contains('y'))) runBothHunts();
            else if (inInput.Contains('y')) runIndeedHunt();
            else if (liInput.Contains('y')) runLinkedInHunt();
        }


        /// <summary>
        /// 
        /// </summary>
        public static void runLinkedInHunt()
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
        public static void runIndeedHunt()
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
        public static void runBothHunts()
        {
            runLinkedInHunt();
            runIndeedHunt();
        }
    }
}