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
            string liInputFile = @"C:\Users\AlexanderLoftus\source\repos\LIHunter\LI_queries.txt";
            List<object> queries = createLIQueries(liInputFile);

            Console.WriteLine("Welcome to LIHunter!");

            Console.WriteLine("Initializing LinkedIn Service.");
            LIService linkedInService = new LIService(queries);
            Console.WriteLine("Successfully initialized LinkedIn Service.");
            
            Console.WriteLine($"Searching for jobs...");
            List<Job> searchResults = linkedInService.searchLI();
            Console.WriteLine("Completed all LinkedIn Searchs!");

            Console.WriteLine("Initializing Google Drive Service.");
            GoogleDriveService googleDriveService = new GoogleDriveService(searchResults);
            Console.WriteLine("Successfully initialized Google Drive Service.");

            Console.WriteLine("Writing search results to your google sheet...");
            string updateResponse = googleDriveService.CreateGoogleSheetsJobEntries(googleDriveService.Jobs);
            Console.WriteLine("Completed writing results to google sheets with a status of...");

            Console.WriteLine("Thank you for using LIHunter");
            Console.WriteLine("-----SESSION STATS-----");
            Console.WriteLine("Number of Jobs Added to Sheet: " + searchResults.Count);
            //TODO: Add more session stats here

        }


        public static List<object> createLIQueries(string filepath)
        {
            string[] lines = File.ReadAllLines(filepath);
            List<object> queries = new List<object>();

            foreach (string line in lines)
            {
                string[] splitInput = line.Split('.');

                if (splitInput.Length >= 3)
                {
                    if (splitInput.Length == 3)
                    {
                        LIQuery query = new LIQuery(splitInput[0], splitInput[1], splitInput[2]);
                        queries.Add(query);
                    }

                    if (splitInput.Length == 7)
                    {
                        bool onlygeteasy = true;
                        if ((splitInput[6].Contains('f')) || (splitInput[6].Contains('F'))) onlygeteasy = false;
                        string[] jobtitles = splitInput[3].Split('|');
                        string[] experiences = splitInput[4].Split('|');
                        LIQueryAdvanced advquery = new LIQueryAdvanced(splitInput[0], splitInput[1], splitInput[2], jobtitles, experiences, splitInput[5], onlygeteasy);
                        queries.Add(advquery);
                    }
                }
            }
            return queries;
        }




    }
}