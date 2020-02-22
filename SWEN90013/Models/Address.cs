using System;
namespace SWEN90013.Models
{
    public class Address
    {
        #region constructors
        public Address()
        {
        }
        #endregion

        #region properties
        public string SiteAddressLine1 { get; set; }
        public string SiteAddressLine2 { get; set; }
        public string SiteAddressLine3 { get; set; }
        public string SiteAddressState { get; set; }
        public int SiteAddressPostCode { get; set; }
        #endregion
    }
}
