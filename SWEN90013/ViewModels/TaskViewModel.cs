using System;
using System.Collections.Generic;
using SWEN90013.Enums;
using SWEN90013.ViewModels.TaskChecklist;
using SWEN90013.Models;

using Xamarin.Forms;

namespace SWEN90013.ViewModels
{
    public class TaskViewModel
    {
        private TaskDetailsValues _taskDetails = new TaskDetailsValues();
        public TaskViewModel()
        {
        }

        public TaskViewModel(TaskClass taskClass)
        {
            this.TaskInfo = new TaskInformation();
            this.TaskInfo.Barcode = taskClass.taskInformation.barcode;
            this.TaskInfo.Contractor = taskClass.taskInformation.contractor;
            this.TaskInfo.IsMoving2012Standard = taskClass.taskInformation.isMoving2012Standard;
            this.TaskInfo.LastServicedBy = taskClass.taskInformation.lastServicedBy;
            this.TaskInfo.Location = taskClass.taskInformation.location;
            this.TaskInfo.MaintainedStandard = taskClass.taskInformation.maintainedStandard;
            this.TaskInfo.OPNumber = taskClass.taskInformation.opNumber;
            this.TaskInfo.TaskTypeDescription = taskClass.taskInformation.taskTypeDescription;

            this.LastService = taskClass.lastService;
            this.LastResult = taskClass.lastResult;
            this.ThisResult = taskClass.thisResult;
            this.TaskType = taskClass.taskType;
            this.ServiceVisitItemNumber = taskClass.serviceVisitItemNumber;
            this.TaskName = taskClass.taskName;
            this.SiteEquipmentLineNumber = taskClass.siteEquipmentLineNumber;
            this.ServiceVisitId = taskClass.serviceVisitID;
            this.SiteId = taskClass.siteID;

            this.Checklists = new List<CheckItemViewModel>();
            if(taskClass.checkLists != null)
            {
                for(int i = 1; i <= taskClass.checkLists.Count; i ++)
                {
                    var task = new CheckItemViewModel(taskClass.checkLists[i - 1]);
                    task.StepNumber = i;
                    this.Checklists.Add(task);
                }
            }

            this.Comments = new List<CommentViewModel>();
            if (taskClass.defectReport != null)
            {
                foreach (DefectReport defect in taskClass.defectReport)
                {
                    this.Comments.Add(new CommentViewModel(defect));
                }
            }
        }

        // based on new API
        public TaskInformation TaskInfo { get; set; }
        public DateTime? LastService { get; set; }
        public TaskResultStatus? LastResult { get; set; }
        public TaskResultStatus ThisResult { get; set; }
        public String TaskType { get; set; }
        public int ServiceVisitItemNumber { get; set; }
        public String TaskName { get; set; }
        public int SiteEquipmentLineNumber { get; set; }
        public int ServiceVisitId { get; set; }
        public int SiteId { get; set; }

        public String LastResultIconUrl
        {
            get { return LastResult.HasValue? LastResult.Value.GetResultIconUrl() : "result-failed.png"; }
        }
		public String LastResultText
		{
			get { return LastResult.HasValue ? LastResult.Value.GetDescription() : "N/A"; }
		}
		public String LastServiceText {
			get { return LastService.HasValue ? LastService.Value.ToShortDateString() : "N/A"; }
		}
		public String ThisResultIconUrl
        {
            get { return ThisResult.GetResultIconUrl(); }
        }
        public Color ThisResultBgColor
        {
            get { return ThisResult.GetBgColor(); }
        }

        // based on old API
        public string TaskNumber { get; set; }
        public string EquipmentName { get; set; }
        public string EquipmentLocation { get; set; }
        public DateTime LastServiceDate { get; set; }
        public string ReferenceNumber { get; set; }
        public string Barcode { get; set; }

        public string LastTestResult { get; set; }

        public string CurrentTestResult { get; set; }
        public bool IsSynced { get; set; }

        public string RelevantOPNumber {get;set;}
        public string LastServicedBy { get; set; }

        private int _taskContractorIndex { get; set; }
        private int _taskLocationIndex { get; set; }
        private int _taskOwnerStandardIndex { get; set; }
        private int _taskMaintainStandardIndex { get; set; }
        private TaskResultStatus _status;

        public TaskResultStatus Status
        {
            get
            {
                if (ThisResult == TaskResultStatus.Passed)
                {
                    _status = TaskResultStatus.Passed;
                }
                else { _status = TaskResultStatus.Failed;  }

                return _status;
            }
            set
            {
                _status = value;
            }
        }

        public string StatusColor
        {
            get { return Status.GetColor(); }
        }

        private List<CheckItemViewModel> _checklists { get; set; }
        private List<CommentViewModel> _comments { get; set; }

        public IList<string> TaskContractorList
        {
            get => _taskDetails.TaskContractorList;
        }
        public IList<string> TaskLocationList
        {
            get => _taskDetails.TaskLocationList;
        }
        public IList<string> TaskOwnerStandardList
        {
            get => _taskDetails.TaskOwnerMovingTo2012StandardList;
        }
        public IList<string> TaskMaintainStandardList
        {
            get => _taskDetails.TaskMaintainedByWhatStandardList;
        }

        public int TaskContractorIndex
        {
            get => _taskContractorIndex;
            set
            {
                _taskContractorIndex = value;
            }
        }

        public int TaskLocationIndex
        {
            get => _taskLocationIndex;
            set
            {
                _taskLocationIndex = value;
            }
        }

        public int TaskOwnerStandardIndex
        {
            get => _taskOwnerStandardIndex;
            set
            {
                _taskOwnerStandardIndex = value;
            }
        }

        public int TaskMaintainStandardIndex
        {
            get => _taskMaintainStandardIndex;
            set
            {
                _taskMaintainStandardIndex = value;
            }
        }

        public List<CheckItemViewModel> Checklists
        {
            get { return _checklists; }
            set
            {
                _checklists = value;
            }
        }

        public List<CommentViewModel> Comments
        {
            get { return _comments; }
            set
            {
                _comments = value;
            }
        }
    }
}
