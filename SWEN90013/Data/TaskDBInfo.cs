using System;
using SWEN90013.Enums;
using SWEN90013.Models;
using SQLite;

namespace SWEN90013.Data
{
    public class TaskDBInfo
    {
        #region constructor
        public TaskDBInfo(TaskClass task)
        {
            this.serviceVisitID = task.serviceVisitID;
            this.contractor = task.taskInformation.contractor;
            this.taskTypeDescription = task.taskInformation.taskTypeDescription;
            this.location = task.taskInformation.location;
            this.barcode = task.taskInformation.barcode;
            this.opNumber = task.taskInformation.opNumber;
            this.isMoving2012Standard = task.taskInformation.isMoving2012Standard;
            this.maintainedStandard = task.taskInformation.maintainedStandard;
            this.lastServicedBy = task.taskInformation.lastServicedBy;

            this.lastService = task.lastService;
            this.lastResult = task.lastResult.ToString();
            this.thisResult = task.thisResult.ToString();
            this.taskType = task.taskType;
            this.serviceVisitItemNumber = task.serviceVisitItemNumber;
            this.taskName = task.taskName;
            this.siteEquipmentLineNumber = task.siteEquipmentLineNumber;
            this.siteID = task.siteID;
            System.Random generator = new System.Random();
            this.ID = generator.Next(0,Int32.MaxValue);
        }

        public TaskDBInfo() { }
        #endregion constructor

        #region properties
        [PrimaryKey]
        public int ID { get; set; }

        public int serviceVisitID { get; set; }

        public string contractor { get; set; }
        public string taskTypeDescription { get; set; }
        public string location { get; set; }
        public int barcode { get; set; }
        public int opNumber { get; set; }
        public Boolean isMoving2012Standard { get; set; }
        public string maintainedStandard { get; set; }
        public string lastServicedBy { get; set; }

        public DateTime? lastService { get; set; }
        public string lastResult { get; set; }
        public string thisResult { get; set; }
        public string taskType { get; set; }
        public int serviceVisitItemNumber { get; set; }
        public string taskName { get; set; }
        public int siteEquipmentLineNumber { get; set; }
        public int siteID { get; set; }
        public Boolean AddNewTaskFlag { get; set; }
        public Boolean EditTaskFlag { get; set; }
        #endregion

        public TaskClass getTask()
        {
            TaskClass task = new TaskClass();
            task.serviceVisitID = this.serviceVisitID;
            task.lastService = this.lastService;
            task.thisResult = (TaskResultStatus)Enum.Parse(typeof(TaskResultStatus), this.thisResult);
            task.lastResult = (TaskResultStatus)Enum.Parse(typeof(TaskResultStatus), this.lastResult);
            task.taskType = this.taskType;
            task.serviceVisitItemNumber = this.serviceVisitItemNumber;
            task.taskName = this.taskName;
            task.siteEquipmentLineNumber = this.siteEquipmentLineNumber;
            task.siteID = this.siteID;
            TaskInformation taskInfo = new TaskInformation();
            taskInfo.contractor = this.contractor;
            taskInfo.taskTypeDescription = this.taskTypeDescription;
            taskInfo.location = this.location;
            taskInfo.barcode = this.barcode;
            taskInfo.opNumber = this.opNumber;
            taskInfo.isMoving2012Standard = this.isMoving2012Standard;
            taskInfo.maintainedStandard = this.maintainedStandard;
            taskInfo.lastServicedBy = this.lastServicedBy;
            task.taskInformation = taskInfo;
            return task;
        }

    }
}
