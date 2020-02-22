using System;
using System.Collections.Generic;
using System.Text;

namespace SWEN90013.Models
{
    public class TaskInformation
    {
        #region constructor
        public TaskInformation ()
        {

        }
        #endregion constructor

        #region properties
        public String contractor { get; set; }
        public String taskTypeDescription { get; set; }
        public String location { get; set; }
        public int barcode { get; set; }
        public int opNumber { get; set; }
        public bool isMoving2012Standard { get; set; }
        public String maintainedStandard { get; set; }
        public String lastServicedBy { get; set; }
        #endregion

        #region methods
        public String Contractor { get { return contractor; } set { contractor = value; } }
        public String TaskTypeDescription { get { return taskTypeDescription; } set { taskTypeDescription = value; } }
        public String Location { get { return location; } set { location = value; } }
        public int Barcode { get { return barcode; } set { barcode = value; } }
        public int OPNumber { get { return opNumber; } set { opNumber = value; } }
        public bool IsMoving2012Standard { get { return isMoving2012Standard; } set { isMoving2012Standard = value; } }
        public String MaintainedStandard { get { return maintainedStandard; } set { maintainedStandard = value; } }
        public String LastServicedBy { get { return lastServicedBy; } set { lastServicedBy = value; } }
        #endregion
    }
}
