using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace LIHunter
{
    class Program
    {
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "LIHunter";



        static void Main(string[] args)
        {
            //Old Implementation
            /*
            Console.WriteLine("Initializing LI Service");
            LIService linkedInService = new LIService();
            Console.WriteLine("Navigated to Google");
            */


            GoogleCredential credential;

            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }



            CreateGoogleSheetsJobEntries(credential);
            ReadGoogleSheetsJobEntries(credential);
        }



        public static void CreateGoogleSheetsJobEntries(GoogleCredential credential)
        {
            string sheet = "Sheet1";
            var range = $"{sheet}!A3:F";
            var valueRange = new ValueRange();

            var oblist = new List<object>() { "Hello!", "This", "was", "insertd", "via", "C#" };
            valueRange.Values = new List<IList<object>> { oblist };

            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            String spreadsheetId = "1Q70wUYzkFZcPbrF0ttrzffIlrEzlBfYH58pKx4x0nbY";
            var appendRequest = service.Spreadsheets.Values.Append(valueRange, spreadsheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var appendReponse = appendRequest.Execute();
            //Helpful link: https://www.twilio.com/blog/2017/03/google-spreadsheets-and-net-core.html?utm_source=youtube&utm_medium=video&utm_campaign=google-sheets-dotnet
        }


        public static void ReadGoogleSheetsJobEntries(GoogleCredential credential)
        {
            string sheet = "Sheet1";
            var range = $"{sheet}!A3:F";
            String spreadsheetId = "1Q70wUYzkFZcPbrF0ttrzffIlrEzlBfYH58pKx4x0nbY";

            //SheetsService service;


            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });



            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);

            // Prints the names and majors of students in a sample spreadsheet:
            // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
            //https://docs.google.com/spreadsheets/d/1Q70wUYzkFZcPbrF0ttrzffIlrEzlBfYH58pKx4x0nbY/edit#gid=0
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                Console.WriteLine("Name, Major");
                foreach (var row in values)
                {
                    // Print columns A and E, which correspond to indices 0 and 4.
                    Console.WriteLine("{0}, {1}", row[0], row[4]);
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }
            Console.Read();
        }
    }
}