using System;
using System.Collections.Generic;

namespace SWEN90013.Models
{
    public class Timeline
    {
        #region constructor
        public Timeline()
        {
        }
        #endregion constructor

        #region properties
        public string result { get; set; }
        public DateTime date { get; set; }
        public List<DefectReport> DefectReport { get; set; }
        #endregion
    }
}
