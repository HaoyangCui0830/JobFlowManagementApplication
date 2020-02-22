using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SWEN90013.Enums;
using SWEN90013.ServicesHandler;
using Xamarin.Forms;

namespace SWEN90013.ViewModels
{
    public class AddNewTaskPageViewModel : INotifyPropertyChanged
    {

        private TaskListViewModel _taskListViewModel;
        private TaskViewModel _task;
		private TaskDetailsValues _detailsValues = new TaskDetailsValues();
		private int _isMoving2012StandardIndex;
		private int _maintainedStandardIndex;

		public AddNewTaskPageViewModel(TaskListViewModel taskListViewModel)
        {
            _taskListViewModel = taskListViewModel;
            _task = new TaskViewModel();
            _task.TaskInfo = new Models.TaskInformation();
            SubmitNewTaskCommand = new Command(SubmitNewTaskCommandAct);
        }

        public AddNewTaskPageViewModel(String Barcode, TaskListViewModel taskListViewModel)
        {
            _task = new TaskViewModel();
            //_task.Barcode = Barcode;
            _task.TaskInfo = new Models.TaskInformation();
            _task.TaskInfo.Barcode = Int32.Parse(Barcode);
            _taskListViewModel = taskListViewModel;
            SubmitNewTaskCommand = new Command(SubmitNewTaskCommandAct);
        }

        public TaskViewModel Task
        {
            get
            {
                return _task;
            }
            set
            {
                _task = value;
            }
        }

		public int IsMoving2012StandardIndex {
			get {
				return _isMoving2012StandardIndex;
			}
			set {
				if (_isMoving2012StandardIndex == value)
					return;
				_isMoving2012StandardIndex = value;
				String Moving2012StandardIndex = _detailsValues
					.GetTaskOwnerMovingTi2012StandardDescription(value);
				Task.TaskInfo.IsMoving2012Standard = Moving2012StandardIndex == "Yes" ? true : false;

				OnPropertyChanged();
				OnPropertyChanged(nameof(Task));
			}
		}
		public int MaintainedStandardIndex {
			get {
				return _maintainedStandardIndex;
			}
			set {
				_maintainedStandardIndex = value;
				String MaintainedStandard = _detailsValues
					.GetTaskMaintainedByWhatStandardDescription(value);
				Task.TaskInfo.MaintainedStandard = MaintainedStandard;

				OnPropertyChanged();
				OnPropertyChanged(nameof(Task));
			}
		}

		public String TaskContractor { get; set; }
        public String EquipmentName { get; set; }

        public void SetANewContractor(String contractor)
        {
            Task.TaskInfo.Contractor = contractor;
            //Console.WriteLine("TAskcontractor" + TaskContractor);
            OnPropertyChanged(nameof(Task));
            
        }

        public void SetANewEquipment(String equiupment)
        {
            EquipmentName = equiupment;
            Task.TaskInfo.TaskTypeDescription = new EquipmentListViewModel().FindEquipmentIDByDescription(equiupment);
            Console.WriteLine("Equipment ID" + Task.TaskInfo.TaskTypeDescription);
            OnPropertyChanged(nameof(Task));
            OnPropertyChanged(nameof(EquipmentName));

        }
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand SubmitNewTaskCommand { get; set; }

        private async void SubmitNewTaskCommandAct()
        {
            //TODO API call
            _ = new TaskServices().AddNewTask(_task, _taskListViewModel.ServiceVisitId);
            Boolean _result =true;
            MessagingCenter.Send(this, "TaskSubmitStatus", _result);
        }
    }
}
