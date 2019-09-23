using System;
using System.Collections.Generic;
using System.Text;

namespace LIHunter
{
    /// <summary>
    /// 
    /// </summary>
    public class LIQuery
    {
        #region PROPERTIES
        public string KeyWords { get; set; } = "Software Engineering";
        public string City { get; set; } = "Chicago";
        public string State { get; set; } = "Illinois";
        public bool OnlyGetEasy { get; set; } = true;
        #endregion



        #region CONSTRUCTORS
        /// <summary>
        /// 
        /// </summary>
        public LIQuery() { }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        public LIQuery(string keywords, string city, string state)
        {
            KeyWords = keywords;
            City = city;
            State = state;
        }
        #endregion
    }



    /// <summary>
    /// 
    /// </summary>
    public class LIQueryAdvanced : LIQuery
    {
        #region PROPERTIES
        public string[] JobTitles { get; set; } = new string[1] { "fulltime" };
        public string[] Experiences { get; set; } = new string[1] { "entry" };
        public string DatePosted { get; set; } = "week";
        #endregion



        #region CONSTRUCTORS
        /// <summary>
        /// 
        /// </summary>
        public LIQueryAdvanced() { }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="jobtitles"></param>
        /// <param name="experiences"></param>
        /// <param name="dateposted"></param>
        /// <param name="onlygeteasy"></param>
        public LIQueryAdvanced(string keywords, string city, string state, string[] jobtitles, string[] experiences, string dateposted, bool onlygeteasy) : base(keywords, city, state)
        {
            JobTitles = jobtitles;
            Experiences = experiences;
            DatePosted = dateposted;
            OnlyGetEasy = onlygeteasy;
        }
        #endregion
    }
}
