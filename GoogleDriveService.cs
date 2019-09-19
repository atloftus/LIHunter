using System;
using System.Text;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;


namespace LIHunter
{
    public class GoogleDriveService
    {
        #region PROPERTIES
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "LIHunter";
        GoogleCredential Credential { get; set; }
        string Sheet { get; set; }
        string Range { get; set; }
        string SpreadsheetID { get; set; }
        SheetsService Service { get; set; }
        #endregion


        #region CONSTRUCTORS
        public GoogleDriveService()
        {
            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                Credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }


            string sheet = "Sheet1";
            var range = $"{sheet}!A3:F";
            string spreadsheetId = "1Q70wUYzkFZcPbrF0ttrzffIlrEzlBfYH58pKx4x0nbY";

            SheetsService service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = Credential,
                ApplicationName = ApplicationName,
            });

            CreateGoogleSheetsJobEntries(Credential);
            ReadGoogleSheetsJobEntries(Credential);
        }



        public GoogleDriveService(List<Job> jobs)
        {
            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                Credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }


            string sheet = "Sheet1";
            var range = $"{sheet}!A3:F";
            string spreadsheetId = "1Q70wUYzkFZcPbrF0ttrzffIlrEzlBfYH58pKx4x0nbY";

            SheetsService service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = Credential,
                ApplicationName = ApplicationName,
            });

            //TODO: Need to be able to pass in Job objects to write to google sheets
            CreateGoogleSheetsJobEntries(Credential);
            //ReadGoogleSheetsJobEntries(Credential);
        }
        #endregion


        #region METHODS
        public static void CreateGoogleSheetsJobEntries(GoogleCredential credential)
        {
            string sheet = "Sheet1";
            var range = $"{sheet}!A3:F";
            string spreadsheetId = "1Q70wUYzkFZcPbrF0ttrzffIlrEzlBfYH58pKx4x0nbY";

            SheetsService service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            var valueRange = new ValueRange();
            var oblist = new List<object>() { "Hello!", "This", "was", "insertd", "via", "C#" };
            valueRange.Values = new List<IList<object>> { oblist };

            var appendRequest = service.Spreadsheets.Values.Append(valueRange, spreadsheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var appendReponse = appendRequest.Execute();
            //Helpful link: https://www.twilio.com/blog/2017/03/google-spreadsheets-and-net-core.html?utm_source=youtube&utm_medium=video&utm_campaign=google-sheets-dotnet
        }


        public static void ReadGoogleSheetsJobEntries(GoogleCredential credential)
        {
            string sheet = "Sheet1";
            var range = $"{sheet}!A3:F";
            string spreadsheetId = "1Q70wUYzkFZcPbrF0ttrzffIlrEzlBfYH58pKx4x0nbY";

            SheetsService service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);

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
        #endregion
    }
}
