using System;
namespace SWEN90013.Models
{
    public class Building
    {
        #region constructor
        public Building()
        {
        }
        #endregion

        #region properties
        public string BuildingEra { get; set; }
        public string BuildingSize { get; set; }
        public string BuildingParts { get; set; }
        public string BuildingClass { get; set; }
        public int KeysHeld { get; set; }
        #endregion
    }
}
