using System;
namespace SWEN90013.Models
{
    public class EquipmentInformation
    {
        public EquipmentInformation()
        {
        }

        public String EquipmentTypeID { get; set; }
        public String Description { get; set; }
        public int PassiveFlag { get; set; }
    }
}
