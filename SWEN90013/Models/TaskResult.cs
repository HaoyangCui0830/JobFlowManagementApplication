using System;
using System.Collections.Generic;
using SWEN90013.Enums;
using SWEN90013.ViewModels.TaskChecklist;

namespace SWEN90013.Models
{
    public class TaskResult
    {
        public TaskResult()
        {
        }

        public TaskResult(TaskResultStatus thisResult, DateTime date, String userCode, int id, List<CheckItemViewModel> checkItems)
        {
            ThisResult = thisResult;
            Date = date;
            UserCode = userCode;
            Id = id;
            Checklist = new List<CheckItem>();
            foreach(CheckItemViewModel checkItem in checkItems)
            {
                Checklist.Add(new CheckItem(checkItem));
            }
        }

        public TaskResult(TaskResultStatus thisResult, DateTime date, String userCode, int id, List<CheckItem> checkItems)
        {
            ThisResult = thisResult;
            Date = date;
            UserCode = userCode;
            Id = id;
            Checklist = new List<CheckItem>();
            foreach (CheckItem checkItem in checkItems)
            {
                Checklist.Add(checkItem);
            }
        }

        #region Properties
        public TaskResultStatus ThisResult {get;set;}
        public DateTime Date { get; set; }
        public String UserCode { get; set; }
        public int Id { get; set; }
        public List<CheckItem> Checklist { get; set; }
        #endregion
    }
}
