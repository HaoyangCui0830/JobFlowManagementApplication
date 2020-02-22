using System;
namespace SWEN90013.Models
{
    public class Locale
    {
        #region constructor
        public Locale()
        {

        }

        public Locale(string csvLine)
        {
            string[] values = csvLine.Split(',');
            PostCode = Convert.ToInt32(values[0]);
            Suburb = values[1];
        }
        #endregion

        #region properties
        public int PostCode { get; set; }
        public string Suburb { get; set; }
        #endregion
    }
}
