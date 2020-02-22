using System;
using SWEN90013.Enums;
using SWEN90013.Models;
using SQLite;

namespace SWEN90013.Data
{
    public class CheckListDBInfo
    {
        public CheckListDBInfo(int serviceVisitItemNumber, Checklist checklist)
        {
            this.serviceVisitItemNumber = serviceVisitItemNumber;
            this.id = checklist.id;
            this.description = checklist.description;
            this.comment = checklist.comment;
            this.siteEquipmentLineNumber = checklist.siteEquipmentLineNumber;
            this.photoURL = checklist.photoURL;
            this.status = checklist.status.ToString();
        }
        [PrimaryKey]
        public int serviceVisitItemNumber { get; set; }
        public int id { get; set; }
        public String description { get; set; }
        public String comment { get; set; }
        public int siteEquipmentLineNumber { get; set; }
        public String photoURL { get; set; }
        public String status { get; set; }
        

        public Checklist GetChecklist()
        {
            Checklist checklist = new Checklist();
            checklist.id = this.id;
            checklist.description = this.description;
            checklist.comment = this.comment;
            checklist.siteEquipmentLineNumber = this.siteEquipmentLineNumber;
            checklist.photoURL = this.photoURL;
            checklist.status = (TaskResultStatus)Enum.Parse(typeof(TaskResultStatus), this.status);
            return checklist;
        }
    }
}
