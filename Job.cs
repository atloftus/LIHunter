using System;
using System.Collections.Generic;
using System.Text;

namespace LIHunter
{
    public class Job
    {
        #region PROPERTIES
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public string Location { get; set; }
        public string DatePosted { get; set; }
        public string DateApplied { get; set; }
        public string DateAddedToSheet { get => DateTime.Now.ToString(); }
        public string Link { get; set; }
        public string Details { get; set; }
        public bool IsEasyApply { get; set; }
        public string RefID { get; set; }
        #endregion


        #region CONSTRUCTORS
        public Job(string company, string position, string location, string refid)
        {
            CompanyName = company;
            Position = position;
            Location = location;
            RefID = refid;
        }


        public Job(string company, string position, string location, string refid, string link, string dateposted, string details) : this(company, position, location, refid)
        {
            Link = link;
            DatePosted = dateposted;
            Details = details;
        }


        public Job(string company, string position, string location, string refid, string link, string dateposted, string details, bool iseasyapply) : this(company, position, location, refid, link, dateposted, details)
        {
            IsEasyApply = iseasyapply;
        }
        #endregion
    }
}
