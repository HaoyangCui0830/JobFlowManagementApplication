using System;
using SQLite;
using SWEN90013.Enums;
using SWEN90013.Models;

namespace SWEN90013.Data
{
    public class CheckItemDBInfo
    {

        public CheckItemDBInfo()
        {
        }

        public CheckItemDBInfo(int serviceVisitItemNumber, CheckItem checkItem)
        {
            this.serviceVisitItemNumber = serviceVisitItemNumber;
            this.id = checkItem.Id;
            this.description = checkItem.Description;
            this.comment = checkItem.Comment;
            this.siteEquipmentLineNumber = checkItem.SiteEquipmentLineNumber;
            this.photoURL = checkItem.PhotoURL;
            this.status = checkItem.Status.ToString();
            this.Primarykey = this.serviceVisitItemNumber.ToString() + this.id.ToString();
        }

        [PrimaryKey]
        public String Primarykey { get; set; }
        public int serviceVisitItemNumber { get; set; }
        public int id { get; set; }
        public String description { get; set; }
        public String comment { get; set; }
        public int siteEquipmentLineNumber { get; set; }
        public String photoURL { get; set; }
        public String status { get; set; }
        public Boolean SendTotalResultFlag { get; set; }
        public String totalTestResult { get; set; }
        public int ServiceVisitId { get; set; }


        public CheckItem GetCheckItem()
        {
            CheckItem checkItem = new CheckItem();
            checkItem.Id = this.id;
            checkItem.Description = this.description;
            checkItem.Comment = this.comment;
            checkItem.SiteEquipmentLineNumber = this.siteEquipmentLineNumber;
            checkItem.PhotoURL = this.photoURL;
            checkItem.Status = (ChecklistStatus)Enum.Parse(typeof(ChecklistStatus), this.status);
            return checkItem;
        }
    }
}
