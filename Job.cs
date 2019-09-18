using System;
using System.Collections.Generic;
using System.Text;

namespace LIHunter
{
    public class Job
    {
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public string Location { get; set; }
        public string DatePosted { get; set; }
        public string DateApplied { get; set; }
        public string Link { get; set; }
        public string Details { get; set; }

        /*
        public DateTime DatePosted { get; set; }
        public DateTime DateApplied { get; set; }
        */


        public Job(string company, string position, string location)
        {
            CompanyName = company;
            Position = position;
            Location = location;
        }

        public Job(string company, string position, string location, string link, string dateposted, string details)
        {
            CompanyName = company;
            Position = position;
            Location = location;
            Link = link;
            DatePosted = dateposted;
            Details = details;
        }
    }
}
