using System;
using System.Collections.Generic;
using SWEN90013.Enums;

namespace SWEN90013.Models
{
    public class TaskClass
    {
        #region constructor
        public TaskClass()
        {
        }
        #endregion constructor

        #region properties
        public TaskInformation taskInformation { get; set; }
        public DateTime? lastService { get; set; }
        public TaskResultStatus? lastResult { get; set; }
        public TaskResultStatus thisResult { get; set; }
        public String taskType { get; set; }
        public int serviceVisitItemNumber { get; set; }
        public String taskName { get; set; }
        public int siteEquipmentLineNumber { get; set; }
        public int serviceVisitID { get; set; }
        public int siteID { get; set; }
        public IList<DefectReport> defectReport { get; set; }
        public IList<CheckItem> checkLists { get; set; }
        #endregion
    }
}
