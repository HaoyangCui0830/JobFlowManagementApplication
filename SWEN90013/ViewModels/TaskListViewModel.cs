using SWEN90013.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;


using Xamarin.Forms;
using SWEN90013.ViewModels.TaskChecklist;
using SWEN90013.Enums;
using SWEN90013.Models;
using SWEN90013.ServicesHandler;
using System.Collections;
using SWEN90013.Data;

namespace SWEN90013.ViewModels
{
    public class TaskListViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// This is the constructor for TaskListViewModel
        /// </summary>
        //public TaskListViewModel()
        //{
        //    _ = InitializeGettingTasks();
        //    ItemTappedCommand = new Command(ItemTappedCommandAct);
        //}
        private int _serviceVisitID;
        private TaskServices _taskService = new TaskServices();
        private bool _isBusy = false;

        public TaskListViewModel(int serviceVisitID)
        {
            _serviceVisitID = serviceVisitID;
            _ = InitializeGettingTasks();
            ItemTappedCommand = new Command(ItemTappedCommandAct);
            SubmitCommand = new Command(SubmitCommandAct);
            //_=getFromAPI();
        }

        /// <summary>
        /// Constructor for TaskListViewModel mostly used for testing purposes
        /// </summary>
        /// <param name="fullUndoneTasks">List of tasks to do</param>
        /// <param name="fullDoneTasks">List of completed tasks</param>
        public TaskListViewModel(List<TaskViewModel> fullUndoneTasks, List<TaskViewModel> fullDoneTasks)
        {
            _fullUndoneTasks = fullUndoneTasks;
            _fullDoneTasks = fullDoneTasks;
            //_serviceVisitID = serviceVisitID;
            _undoneTasks = new ObservableCollection<TaskViewModel>(fullUndoneTasks);
            _doneTasks = new ObservableCollection<TaskViewModel>(fullDoneTasks);
            ItemTappedCommand = new Command(ItemTappedCommandAct);
        }

        public INavigation Navigation { get; set; }

        private ObservableCollection<TaskViewModel> _undoneTasks;
        //records the full undone tasks for searching functionality
        private List<TaskViewModel> _fullUndoneTasks;
        private ObservableCollection<TaskViewModel> _doneTasks;
        //records the full done tasks for searching functionality
        private List<TaskViewModel> _fullDoneTasks;

        //records the search term that user entered
        private String _searchedTerm;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<TaskViewModel> ToDoTasks
        {
            get { return _undoneTasks; }
            set
            {
                _undoneTasks = value;
                OnPropertyChanged(nameof(ToDoTasks));
                OnPropertyChanged(nameof(IsNoToDoTasks));
                //also changed the title
                OnPropertyChanged(nameof(ToDoTasksTitle));
            }

        }

        public ObservableCollection<TaskViewModel> DoneTasks
        {
            get { return _doneTasks; }
            set
            {
                _doneTasks = value;
                OnPropertyChanged(nameof(DoneTasks));
                OnPropertyChanged(nameof(IsNoDoneTasks));
            }
        }

        public String SearchedTerm
        {
            get { return _searchedTerm; }
            set
            {
                _searchedTerm = value;
                //update the displayed tasks
                SearchTasks();
                OnPropertyChanged(nameof(SearchedTerm));
                //also changed the title
                OnPropertyChanged(nameof(ToDoTasksTitle));
            }
        }

        //this property indicates whether the page is waiting for data
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public bool IsNoToDoTasks
        {
            get {
                if (ToDoTasks == null)
                    return false;
                return ToDoTasks.Count == 0;
            }
        }
        public bool IsNoDoneTasks
        {
            get {
                if (DoneTasks == null)
                    return false;
                return DoneTasks.Count == 0;
            }
        }

        public int ServiceVisitId
        {
            get { return _serviceVisitID; }
        }

        //title to be displayed on the UI for to do tasks
        public string ToDoTasksTitle
        {
            get
            {
                if (ToDoTasks != null)
                {
                    return String.Format("To-Do ({0})", ToDoTasks.Count());
                }
                return String.Format("To-Do ({0})", 0);
            }
        }

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// This is used to request the task list during initialization
        /// </summary>
        public async Task InitializeGettingTasks()
        {
            //var fieldsOne = new ObservableCollection<CheckItemFieldViewModel>()
            //{
            //    new CheckItemFieldViewModel("Presure","kPa",""),
            //    new CheckItemFieldViewModel("Water Pressure", "kPa", ""),
            //};
            //var checklists = new List<CheckItemViewModel>()
            //{
            //    new CheckItemViewModel(1, 1, "INSPECT fire door panel to ensure it is free of any visible delamination, and other damage", ChecklistStatus.NoTest,
            //                           new CommentViewModel("https://5.imimg.com/data5/IC/IC/MY-25107910/fire-doors-shakti-met-door-500x500.png", "The seal on the fire door at G20 theatre needs to be replaced"),
            //                           fieldsOne),
            //    new CheckItemViewModel(2, 2, "INSPECT that the perimeter seal is in good condition and not damaged", ChecklistStatus.NoTest, null, null)
            //};

            //var comments = new List<CommentViewModel>()
            //{
            //    new CommentViewModel("https://5.imimg.com/data5/IC/IC/MY-25107910/fire-doors-shakti-met-door-500x500.png", "The fire door needs to be fixed ASAP"),
            //    new CommentViewModel("https://firesafe-au.com/wp-content/uploads/2016/06/fire-doors.jpg", "The fire door outside the building needs to be reinstalled")
            //};

            //ToDoTasks = new ObservableCollection<TaskViewModel>
            //{
            //    new TaskViewModel()
            //    {
            //        // new API
            //        TaskInfo = new TaskInformation()
            //        {
            //            Contractor = "FBS",
            //            TaskTypeDescription = "Building elements to sarisy fire",
            //            Location = "Near Front Office",
            //            Barcode = 1234456,
            //            OPNumber = 12345,
            //            IsMoving2012Standard = true,
            //            MaintainedStandard = "Earlier than 2005",
            //            LastServicedBy = "John"
            //        },
            //        LastService = new DateTime(2019, 8, 20),
            //        LastResult = TaskResultStatus.Pass,
            //        ThisResult = TaskResultStatus.NoTest,
            //        TaskType = "passive",
            //        ServiceVisitItemNumber = 700006,
            //        TaskName = "Test Fire Door",
            //        SiteEquipmentLineNumber = 21,
            //        ServiceVisitId = 7000001,
            //        SiteId = 61435214,

            //        // old API
            //        TaskNumber = "234234",
            //        EquipmentName = "Building elements to sarisy fire",
            //        EquipmentLocation = "Throughout",
            //        LastServiceDate = new DateTime(2019, 10, 31),
            //        ReferenceNumber = "23434",
            //        LastTestResult = "Passed",
            //        Barcode = "12345678",
            //        Checklists = checklists,
            //        Comments = comments
            //    },
            //    new TaskViewModel()
            //    {
            //        TaskInfo = new TaskInformation()
            //        {
            //            Contractor = "FBS",
            //            TaskTypeDescription = "Fire indices for materials",
            //            Location = "Throughout",
            //            Barcode = 1234457,
            //            OPNumber = 12346,
            //            IsMoving2012Standard = true,
            //            MaintainedStandard = "Earlier than 2005",
            //            LastServicedBy = "John"
            //        },
            //        LastService = new DateTime(2019, 8, 20),
            //        LastResult = TaskResultStatus.Failed,
            //        ThisResult = TaskResultStatus.NoTest,
            //        TaskType = "passive",
            //        ServiceVisitItemNumber = 700006,
            //        TaskName = "Test Fire Door",
            //        SiteEquipmentLineNumber = 21,
            //        ServiceVisitId = 7000001,
            //        SiteId = 61435214,

            //        // old API
            //        TaskNumber = "234236",
            //        EquipmentName = "Fire indices for materials",
            //        EquipmentLocation = "Throughout",
            //        LastServiceDate = new DateTime(2019, 10, 31),
            //        ReferenceNumber = "23434",
            //        LastTestResult = "Passed",
            //        Barcode = "123456789",
            //        Checklists = checklists,
            //        Comments = comments
            //    },
            //    new TaskViewModel()
            //    {
            //        TaskInfo = new TaskInformation()
            //        {
            //            Contractor = "FBS",
            //            TaskTypeDescription = "Fire indices for materials",
            //            Location = "Throughout",
            //            Barcode = 1234457,
            //            OPNumber = 12346,
            //            IsMoving2012Standard = true,
            //            MaintainedStandard = "Earlier than 2005",
            //            LastServicedBy = "John"
            //        },
            //        LastService = new DateTime(2019, 8, 20),
            //        LastResult = TaskResultStatus.Failed,
            //        ThisResult = TaskResultStatus.NoTest,
            //        TaskType = "passive",
            //        ServiceVisitItemNumber = 700006,
            //        TaskName = "Test Fire Door",
            //        SiteEquipmentLineNumber = 21,
            //        ServiceVisitId = 7000001,
            //        SiteId = 61435214,

            //        // old API
            //        TaskNumber = "234236",
            //        EquipmentName = "Fire indices for materials",
            //        EquipmentLocation = "Throughout",
            //        LastServiceDate = new DateTime(2019, 10, 31),
            //        ReferenceNumber = "23434",
            //        LastTestResult = "Passed",
            //        Barcode = "123456789",
            //        Checklists = checklists,
            //        Comments = comments
            //    }
            //};

            //TODO initialise done tasks - for now this is empty
            //DoneTasks = new ObservableCollection<TaskViewModel>
            //{
            //    new TaskViewModel()
            //    {
            //        // new API
            //        TaskInfo = new TaskInformation()
            //        {
            //            Contractor = "FBS",
            //            TaskTypeDescription = "Penetrations in fire rated structures",
            //            Location = "Near Front Office",
            //            Barcode = 1234456,
            //            OPNumber = 12345,
            //            IsMoving2012Standard = true,
            //            MaintainedStandard = "Later than 2005",
            //            LastServicedBy = "John"
            //        },
            //        LastService = new DateTime(2019, 8, 20),
            //        LastResult = TaskResultStatus.Pass,
            //        ThisResult = TaskResultStatus.Failed,
            //        TaskType = "passive",
            //        ServiceVisitItemNumber = 700006,
            //        TaskName = "Test Fire Door",
            //        SiteEquipmentLineNumber = 21,
            //        ServiceVisitId = 7000001,
            //        SiteId = 61435214,

            //        // old API
            //        TaskNumber = "234234",
            //        EquipmentName = "Building elements to sarisy fire",
            //        EquipmentLocation = "Throughout",
            //        LastServiceDate = new DateTime(2019, 10, 31),
            //        ReferenceNumber = "23434",
            //        LastTestResult = "Passed",
            //        Barcode = "12345678",
            //        Checklists = checklists,
            //        Comments = comments
            //    },
            //    new TaskViewModel()
            //    {
            //        TaskInfo = new TaskInformation()
            //        {
            //            Contractor = "FBS",
            //            TaskTypeDescription = "Fire resisting structures",
            //            Location = "Throughout",
            //            Barcode = 1234457,
            //            OPNumber = 12346,
            //            IsMoving2012Standard = true,
            //            MaintainedStandard = "Later than 2005",
            //            LastServicedBy = "John"
            //        },
            //        LastService = new DateTime(2019, 8, 20),
            //        LastResult = TaskResultStatus.Failed,
            //        ThisResult = TaskResultStatus.Pass,
            //        TaskType = "passive",
            //        ServiceVisitItemNumber = 700006,
            //        TaskName = "Test Fire Door",
            //        SiteEquipmentLineNumber = 21,
            //        ServiceVisitId = 7000001,
            //        SiteId = 61435214,

            //        // old API
            //        TaskNumber = "234236",
            //        EquipmentName = "Fire indices for materials",
            //        EquipmentLocation = "Throughout",
            //        LastServiceDate = new DateTime(2019, 10, 31),
            //        ReferenceNumber = "23434",
            //        LastTestResult = "Passed",
            //        Barcode = "123456789",
            //        Checklists = checklists,
            //        Comments = comments
            //    },
            //    new TaskViewModel()
            //    {
            //        TaskInfo = new TaskInformation()
            //        {
            //            Contractor = "FBS",
            //            TaskTypeDescription = "Fire resisting structures",
            //            Location = "Throughout",
            //            Barcode = 1234457,
            //            OPNumber = 12346,
            //            IsMoving2012Standard = true,
            //            MaintainedStandard = "Later than 2005",
            //            LastServicedBy = "John"
            //        },
            //        LastService = new DateTime(2019, 8, 20),
            //        LastResult = TaskResultStatus.Failed,
            //        ThisResult = TaskResultStatus.Pass ,
            //        TaskType = "passive",
            //        ServiceVisitItemNumber = 700006,
            //        TaskName = "Test Fire Door",
            //        SiteEquipmentLineNumber = 21,
            //        ServiceVisitId = 7000001,
            //        SiteId = 61435214,

            //        // old API
            //        TaskNumber = "234236",
            //        EquipmentName = "Fire indices for materials",
            //        EquipmentLocation = "Throughout",
            //        LastServiceDate = new DateTime(2019, 10, 31),
            //        ReferenceNumber = "23434",
            //        LastTestResult = "Passed",
            //        Barcode = "123456789",
            //        Checklists = checklists,
            //        Comments = comments
            //    }
            //};


            //also record the full tasks in case user starts to filter down the list by searching

            if (IsBusy) return;
            IsBusy = true;

            _fullUndoneTasks = new List<TaskViewModel>();
            _fullDoneTasks = new List<TaskViewModel>();

            List<TaskViewModel> taskViewModelFromAPI = new List<TaskViewModel>();
            try
            {
                var result = await _taskService.GetTaskForServiceVisit(_serviceVisitID.ToString());
                if (result != null)
                {
                    Console.WriteLine(result[0].lastService);
                    taskViewModelFromAPI = new List<TaskClass>(result).Select(s => new TaskViewModel(s)).ToList();
                    List<TaskClass> tasks = new List<TaskClass>(result);
                    Console.WriteLine(tasks.Count);
                    foreach (TaskClass t in tasks)
                    {
                        TaskDBInfo taskDBInfo = new TaskDBInfo(t);
                        _ = App.TaskDatabase.SaveTaskAsync(taskDBInfo);

                        foreach (CheckItem checkItem in t.checkLists)
                        {
                            CheckItemDBInfo checkItemDBInfo = new CheckItemDBInfo(t.serviceVisitItemNumber, checkItem);
                            //Console.WriteLine(checkItemDBInfo.taskTypeDescription + " " + checkItemDBInfo.serviceVisitID);
                            await App.CheckItemDatabase.SaveCheckListInfoAsync(checkItemDBInfo);
                        }
                        foreach (DefectReport defect in t.defectReport)
                        {
                            DefectReportDBInfo defectDBInfo = new DefectReportDBInfo(defect, t.serviceVisitItemNumber);
                            await App.DefectReportDatabase.SaveDefectReportInfoAsync(defectDBInfo);
                        }
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e.StackTrace); }
            finally
            {
                IsBusy = false;
                Console.WriteLine("In task list vm" + taskViewModelFromAPI[0].TaskInfo.Barcode);
            }
            if (taskViewModelFromAPI != null)
            {
                foreach (TaskViewModel taskvm in taskViewModelFromAPI)
                {
                    if (taskvm.ThisResult.Equals(TaskResultStatus.NoTest))
                    {
                        Console.WriteLine("todo");
                        _fullUndoneTasks.Add(taskvm);
                    }
                    else
                    {
                        Console.WriteLine("done");
                        _fullDoneTasks.Add(taskvm);
                    }
                }
            }

            ToDoTasks = new ObservableCollection<TaskViewModel>(_fullUndoneTasks);
            DoneTasks = new ObservableCollection<TaskViewModel>(_fullDoneTasks);
        }

        

        public Command ItemTappedCommand { get; set; }
        public Command SubmitCommand { get; set; }

        /// <summary>
        /// This command will be triggered when centain Task was tapped in the list
        /// </summary>
        /// <param name="sender"></param>
        void ItemTappedCommandAct (object sender)
        {
            Navigation.PushAsync(new TaskPage((TaskViewModel)sender));
        }

        void SubmitCommandAct(object sender)
        {
            Navigation.PushAsync(new TaskReviewSubmissionPage(_fullUndoneTasks, _fullDoneTasks, _serviceVisitID));
        }
        /// <summary>
        /// This method filter both the todo and done tasks based on the searched term entered by the user
        /// The search is based on the equipment name
        /// </summary>
        void SearchTasks()
        {
            if (!String.IsNullOrWhiteSpace(_searchedTerm))
            {
                var todoTasks = _fullUndoneTasks.Where(t => t.TaskInfo.TaskTypeDescription.ToLower().Contains(_searchedTerm.ToLower())).ToList();
                var doneTasks = _fullDoneTasks.Where(t => t.TaskInfo.TaskTypeDescription.ToLower().Contains(_searchedTerm.ToLower())).ToList();
                ToDoTasks = new ObservableCollection<TaskViewModel>(todoTasks);
                DoneTasks = new ObservableCollection<TaskViewModel>(doneTasks);
            }
            //if the search term is empty, display all of the tasks
            else
            {
                //TODO uncomment the search for done tasks
                ToDoTasks = new ObservableCollection<TaskViewModel>(_fullUndoneTasks);
                DoneTasks = new ObservableCollection<TaskViewModel>(_fullDoneTasks);
            }
        }
    }
}