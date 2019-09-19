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
        static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static string ApplicationName = "LIHunter";
        GoogleCredential Credential { get; set; }
        public string Sheet { get; set; } = "LinkedIn";
        public string Range { get; set; } = "LinkedIn!A2:I";
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
            List<string> existingRfids = getExistingSheetJobRefIds();
            List<IList<object>> lineItems = new List<IList<object>>();
            List<object> lineHolder = new List<object>();
            foreach (Job job in jobs)
            {
                if (!(existingRfids.Contains(job.RefID)))
                {
                    lineHolder = new List<object>() { job.CompanyName, job.Location, job.Position, job.IsEasyApply, job.DatePosted, job.DateAddedToSheet, job.Details, job.Link, job.RefID };
                    lineItems.Add(lineHolder);
                }                 
            }

            var valueRange = new ValueRange();
            valueRange.Values = lineItems;
            var appendRequest = Service.Spreadsheets.Values.Append(valueRange, SpreadsheetID, Range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var updateReponse = appendRequest.Execute();
            return lineItems.Count.ToString();
        }


        public List<string> getExistingSheetJobRefIds()
        {
            List<string> existingRfids = new List<string>();
            SpreadsheetsResource.ValuesResource.GetRequest request = Service.Spreadsheets.Values.Get(SpreadsheetID, Range);
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                foreach (var row in values) existingRfids.Add(row[8].ToString());
            }
            return existingRfids;
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
