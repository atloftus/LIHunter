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
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Google Sheets API .NET Quickstart";

        static void Main(string[] args)
        {

            /*
            UserCredential credential;

            using (var stream =
                new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }
            */
             string[] Scopes = { SheetsService.Scope.Spreadsheets };
             string ApplicationName = "Current Legislators";
            string SpreadsheetId = "1P_0tngt7o02xgXr9T-wSaVXz-_JIJZekGTGRLoWLnA4";
             string sheet = "legislators-current";
            SheetsService service;

            GoogleCredential credential;
            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(Scopes);
            }

            // Create Google Sheets API service.
            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            CreateEntry(credential);

            //STUFF FOR READING FROM A SHEET
            /*
            // Define request parameters.
            //String spreadsheetId = "1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms";
            String spreadsheetId = "1Q70wUYzkFZcPbrF0ttrzffIlrEzlBfYH58pKx4x0nbY";
            String range = "Sheet1!A2:E";
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
            */
        }


        public static void CreateEntry(GoogleCredential credential)
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
    }
}




/*
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
*/
