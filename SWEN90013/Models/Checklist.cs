using System;
using System.Collections.Generic;
using SWEN90013.Enums;

namespace SWEN90013.Models
{
    public class Checklist
    {
        #region constructor
        public Checklist()
        {
        }
        #endregion
        #region properties
        public int id;
        public String description;
        public String comment;
        public int siteEquipmentLineNumber;
        public String photoURL;
        public TaskResultStatus status;
        public List<UnitEntryValue> unitEntryValue;
        #endregion
    }
}
