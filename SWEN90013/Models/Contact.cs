using System;
namespace SWEN90013.Models
{
    public class Contact
    {
        #region constructors
        public Contact()
        {
        }
        #endregion

        #region properties
        public string ContactName { get; set; }
        public string ContactTelephone { get; set; }
        public string ContactMobile { get; set; }
        public string ContactEmail { get; set; }
        #endregion
    }
}
