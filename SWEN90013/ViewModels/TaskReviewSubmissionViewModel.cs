using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Plugin.SecureStorage;
using SWEN90013.Enums;
using SWEN90013.Models;
using SWEN90013.Views;
using Xamarin.Forms;

namespace SWEN90013.ViewModels
{
    public class TaskReviewSubmissionViewModel: INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }
        public Command SubmitCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public string Notes { get; set; }
        public bool CustomerInspection { get; set; }
        public bool TechnicianInspection { get; set; }
        public string ChosenStatus { get; set; }
        public List<string> DropDownStatus { get; set; }
        public string CurrentDate { get; set; }
        public int ServiceVisitID;

        private List<TaskViewModel> _tasksList;
        
        //private TaskResultStatus _status;
        //private String _colorResult;

        // constructor
        public TaskReviewSubmissionViewModel(List<TaskViewModel> fullUndoneTasksoTasks, List<TaskViewModel> fullDoneTasks, int serviceVisitID)
        {
            //initial task list
            if (fullUndoneTasksoTasks != null || fullDoneTasks != null)
            {
                _tasksList = new List<TaskViewModel>();
                foreach (TaskViewModel taskViewmodel in fullUndoneTasksoTasks)
                {
                    this._tasksList.Add(taskViewmodel);
                }
                foreach (TaskViewModel taskViewmodel in fullDoneTasks)
                {
                    this._tasksList.Add(taskViewmodel);
                }
            }
            TaskList = _tasksList;

            ServiceVisitID = serviceVisitID;

            //InitializeGettingTasks();
            DropDownStatus = new List<string>();
            DropDownStatus.Add("Revisit Required");
            DropDownStatus.Add("Inspected");
            DropDownStatus.Add("Access Review");
            DropDownStatus.Add("Field Review");
            DropDownStatus.Add("Office Review");
            DropDownStatus.Add("Pending External");
            bool flag = true;
            foreach (TaskViewModel taskViewModel in TaskList)
            {
                if (taskViewModel.ThisResult == TaskResultStatus.NoTest)
                {
                    flag = false;
                }
            }
            if (flag == true)
            {
                DropDownStatus.Add("Completed");
            }
            else
            {
                DropDownStatus.Add("Vacant");
            }
            ChosenStatus = "Revisit Required";
            CustomerInspection = TechnicianInspection = false;
            SubmitCommand = new Command(OnSubmit);
            DateTime date = DateTime.Now;
            CurrentDate = "Date: " + date.Day.ToString() + "/" + date.Month.ToString() + "/" + date.Year.ToString();
        }

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// TaskList used to store the tasks
        /// </summary>
        public List<TaskViewModel> TaskList
        {
            get { return _tasksList; }
            set
            {
                _tasksList = value;
                OnPropertyChanged(nameof(TaskList));
            }

        }


        /// <summary>
        /// Function binded to the submit button command
        /// </summary>
        public async void OnSubmit()
        {
            ServiceVisitSubmission obj = new ServiceVisitSubmission();
            obj.customerSignature = CustomerInspection;
            obj.techSignature = TechnicianInspection;
            obj.reviewNotes = Notes == null? "" : Notes;
            DateTime date = DateTime.Now;
            obj.lastUpdatedBy = CrossSecureStorage.Current.GetValue("UserName");

			//default status
			obj.serviceVisitStatus = ServiceVisitStatus.RevisitRequired.ToString();

			//find out the selected status
			foreach(var field in typeof(ServiceVisitStatus).GetFields()) {
				var attribute = Attribute.GetCustomAttribute(field,
					typeof(DescriptionAttribute)) as DescriptionAttribute;
				if (attribute == null)
					continue;
				if (attribute.Description.Equals(ChosenStatus)) {
					obj.serviceVisitStatus = ((ServiceVisitStatus)field.GetValue(null)).ToString();
					break;
				}
			}

			if (TechnicianInspection == true)
            {
                bool result = await new ServicesHandler.ServiceVisitServices().SubmitServiceVisit(ServiceVisitID.ToString(), obj);
                System.Console.WriteLine("*******" + result);
                if(result == true) {
					MessagingCenter.Send(this, "submitTask");
				}
				else {
					MessagingCenter.Send(this, "error");
				}

			}
            else
            {
                MessagingCenter.Send(this, "needInspection");
            }

        }

        ///// <summary>
        /////  create TaskList with dummy data
        ///// </summary>
        //private void InitializeGettingTasks()
        //{
        //    TaskList = new List<TaskViewModel>
        //    {
        //        new TaskViewModel()
        //        {
        //            TaskNumber = "234234",
        //            EquipmentName = "Building elements to sarisy fire",
        //            EquipmentLocation = "Throughout",
        //            LastServiceDate = new DateTime(2019, 10, 31),
        //            ReferenceNumber = "23434",
        //            ThisResult = TaskResultStatus.Passed
        //        },
        //        new TaskViewModel()
        //        {
        //            TaskNumber = "234236",
        //            EquipmentName = "Fire indices for materials",
        //            EquipmentLocation = "Throughout",
        //            LastServiceDate = new DateTime(2019, 10, 31),
        //            ReferenceNumber = "23434",
        //            ThisResult = TaskResultStatus.Failed
        //        },
        //        new TaskViewModel()
        //        {
        //            TaskNumber = "234236",
        //            EquipmentName = "Fire indices ",
        //            EquipmentLocation = "Throughout",
        //            LastServiceDate = new DateTime(2019, 10, 31),
        //            ReferenceNumber = "23434",
        //            ThisResult = TaskResultStatus.Passed
        //        }
        //    };
        //}

        //public TaskResultStatus Status
        //{
        //    get
        //    {
        //        return _status;
        //    }
        //    set
        //    {
        //        _status = value;
        //    }
        //}


        //public String ColorResult
        //{
        //    get
        //    {

        //        return TaskResultStatusMethods.GetColor(Status);
        //    }
        //    set
        //    {
        //        _colorResult = value;
        //    }
        //}
    }
}

