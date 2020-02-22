using System;
namespace SWEN90013.Models
{
    public class AddNewTaskInformation
    {
        public AddNewTaskInformation()
        {
        }

        public AddNewTaskInformation(TaskInformation taskInformation)
        {
            this.contractor = taskInformation.contractor;
            this.barcode = taskInformation.barcode;
            this.equipmentTypeID = taskInformation.taskTypeDescription;
            this.location = taskInformation.location;
            this.opNumber = taskInformation.opNumber;
            this.isMoving2012Standard = taskInformation.isMoving2012Standard;
            this.maintainedStandard = taskInformation.maintainedStandard;
            this.addedBy = taskInformation.lastServicedBy;
        }

        public String contractor { get; set; }
        public String equipmentTypeID { get; set; }
        public String location { get; set; }
        public int barcode { get; set; }
        public int opNumber { get; set; }
        public bool isMoving2012Standard { get; set; }
        public String maintainedStandard { get; set; }
        public String addedBy { get; set; }
    }
}
