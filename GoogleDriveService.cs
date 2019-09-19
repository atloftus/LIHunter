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
        public string Sheet { get; set; } = "Sheet1";
        public string Range { get; set; } = "Sheet1!A3:F";
        public string SpreadsheetID { get; set; } = "1Q70wUYzkFZcPbrF0ttrzffIlrEzlBfYH58pKx4x0nbY";
        SheetsService Service { get; set; }
        public List<Job> Jobs { get; set; }
        #endregion


        #region CONSTRUCTORS
        public GoogleDriveService()
        {
            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                this.Credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }

            this.Service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = this.Credential,
                ApplicationName = ApplicationName,
            });
        }


        public GoogleDriveService(List<Job> jobs) : this()
        {
            Jobs = jobs;
        }
        #endregion


        #region METHODS
        public string CreateGoogleSheetsJobEntries(List<Job> jobs)
        {
            //TODO: Figure out how to write Job objects to google sheets
            var oblist = new List<object>() { "Hello!", "This", "was", "insertd", "via", "C#" };

            var valueRange = new ValueRange();
            valueRange.Values = new List<IList<object>> { oblist };

            var appendRequest = Service.Spreadsheets.Values.Append(valueRange, SpreadsheetID, Range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var appendReponse = appendRequest.Execute();

            //TODO: Change this to the response status from teh googel sheets update
            return "";
        }


        public void ReadGoogleSheetsJobEntries()
        {
            SpreadsheetsResource.ValuesResource.GetRequest request = Service.Spreadsheets.Values.Get(SpreadsheetID, Range);

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
