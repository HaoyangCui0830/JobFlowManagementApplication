using System;
using System.Collections.Generic;
using SWEN90013.Enums;
using SWEN90013.ViewModels.TaskChecklist;

namespace SWEN90013.Models
{
    public class CheckItem
    {
        #region Constructor
        public CheckItem()
        {
        }

        public CheckItem(CheckItemViewModel checkItem)
        {
            Id = checkItem.Id;
            Description = checkItem.Description;
            if(checkItem.CommentExist)
            {
                Comment = checkItem.Comment.Description;
                PhotoURL = checkItem.Comment.ImageUrl;
            }
            SiteEquipmentLineNumber = checkItem.SiteEquipmentLineNumber;
            Status = checkItem.Status;
            UnitEntryValue = new List<UnitEntryValue>();
            if(checkItem.Fields != null)
            {
                foreach(CheckItemFieldViewModel field in checkItem.Fields)
                {
                    UnitEntryValue.Add(new UnitEntryValue(field));
                }
            }
        }
        #endregion

        #region Properties
        public int Id { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public int SiteEquipmentLineNumber { get; set; }
        public string PhotoURL { get; set; }
        public ChecklistStatus Status { get; set; }
        public List<UnitEntryValue> UnitEntryValue { get; set; }
        #endregion
    }
}
