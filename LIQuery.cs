using System;
using System.Collections.Generic;
using System.Text;

namespace LIHunter
{
    public class LIQuery
    {
        public string KeyWords { get; set; }  = "Software Engineering";
        public string City { get; set; } = "Chicago";
        public string State { get; set; } = "Illinois";
        public bool OnlyGetEasy { get; set; } = true;



        public LIQuery(){ }


        public LIQuery(string keywords, string city, string state)
        {
            KeyWords = keywords;
            City = city;
            State = state;
        }
    }


    public class LIQueryAdvanced : LIQuery
    {
        public string[] JobTitles { get; set; } = new string[1] { "fulltime" };
        public string[] Experiences { get; set; } = new string[1] { "entry" };
        public string DatePosted { get; set; } = "week";



        public LIQueryAdvanced() { }


        public LIQueryAdvanced(string keywords, string city, string state, string[] jobtitles, string[] experiences, string dateposted, bool onlygeteasy) : base(keywords, city, state)
        {
            JobTitles = jobtitles;
            Experiences = experiences;
            DatePosted = dateposted;
            OnlyGetEasy = onlygeteasy;
        }
    }
}
