using SWEN90013.Enums;
using SWEN90013.Models;
using SWEN90013.ServicesHandler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SWEN90013.ViewModels
{
    class TaskDetailsViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public TaskDetailsViewModel(TaskViewModel targetTast)
        {
            TargetTask = targetTast;
            //DuplicateTaskInfo = new TaskInformation
            DuplicateTaskInfo = new TaskInformation()
            {
                Contractor = _taskViewModel.TaskInfo.Contractor,
                TaskTypeDescription = _taskViewModel.TaskInfo.TaskTypeDescription,
                Barcode = _taskViewModel.TaskInfo.Barcode,
                IsMoving2012Standard = _taskViewModel.TaskInfo.IsMoving2012Standard,
                LastServicedBy = _taskViewModel.TaskInfo.LastServicedBy,
                Location = _taskViewModel.TaskInfo.Location,
                MaintainedStandard = _taskViewModel.TaskInfo.MaintainedStandard,
                OPNumber = _taskViewModel.TaskInfo.OPNumber
            };
            SaveTaskInfoCommand = new Command(async () => await SaveTaskInfoCommandAct());
        }

        private TaskDetailsValues _detailsValues = new TaskDetailsValues();

        private TaskServices _taskService = new TaskServices();
        private bool _isBusy = false;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value)
                    return;
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public IList<String> IsMoving2012StandardList
        {
            get
            {
                return _detailsValues.TaskOwnerMovingTo2012StandardList;
            }
        }
        public IList<String> MaintainedStandardList
        {
            get
            {
                return _detailsValues.TaskMaintainedByWhatStandardList;
            }
        }
        public TaskInformation DuplicateTaskInfo { get; set; }
  
        private int _isMoving2012StandardIndex;
        private int _maintainedStandardIndex;
        public int IsMoving2012StandardIndex
        {
            get
            {
                return _isMoving2012StandardIndex;
            }
            set
            {
                if (_isMoving2012StandardIndex == value)
                    return;
                _isMoving2012StandardIndex = value;
                String Moving2012StandardIndex = _detailsValues
                    .GetTaskOwnerMovingTi2012StandardDescription(value);
                DuplicateTaskInfo.IsMoving2012Standard = Moving2012StandardIndex == "Yes" ? true : false;

                OnPropertyChanged();
                OnPropertyChanged(nameof(DuplicateTaskInfo));
            }
        }
        public int MaintainedStandardIndex
        {
            get
            {
                return _maintainedStandardIndex;
            }
            set
            {
                _maintainedStandardIndex = value;
				String MaintainedStandard = _detailsValues
					.GetTaskMaintainedByWhatStandardDescription(value);
                DuplicateTaskInfo.MaintainedStandard = MaintainedStandard;

                OnPropertyChanged();
                OnPropertyChanged(nameof(DuplicateTaskInfo));
            }
        }

        private TaskViewModel _taskViewModel;
        public TaskViewModel TargetTask
        {
            get { return _taskViewModel; }
            set
            {
                if (_taskViewModel == value)
                    return;
                _taskViewModel = value;

                DuplicateTaskInfo = new TaskInformation()
                {
                    Contractor = _taskViewModel.TaskInfo.Contractor,
                    TaskTypeDescription = _taskViewModel.TaskInfo.TaskTypeDescription,
                    Barcode = _taskViewModel.TaskInfo.Barcode,
                    IsMoving2012Standard = _taskViewModel.TaskInfo.IsMoving2012Standard,
                    LastServicedBy = _taskViewModel.TaskInfo.LastServicedBy,
                    Location = _taskViewModel.TaskInfo.Location,
                    MaintainedStandard = _taskViewModel.TaskInfo.MaintainedStandard,
                    OPNumber = _taskViewModel.TaskInfo.OPNumber
                };

                IsMoving2012StandardIndex = _detailsValues.GetTaskOwnerMovingTi2012StandardIndex
                    (DuplicateTaskInfo.IsMoving2012Standard ? "Yes" : "No");
                MaintainedStandardIndex = _detailsValues.GetTaskMaintainedByWhatStandardIndex
                    (DuplicateTaskInfo.MaintainedStandard);

                OnPropertyChanged();
                OnPropertyChanged(nameof(DuplicateTaskInfo));
                OnPropertyChanged(nameof(IsMoving2012StandardIndex));
                OnPropertyChanged(nameof(MaintainedStandardIndex));
            }
        }

        public void SetANewContractor(String contractor)
        {
            DuplicateTaskInfo.Contractor = contractor;
            OnPropertyChanged(nameof(DuplicateTaskInfo));
        }

        public ICommand SaveTaskInfoCommand { get; set; }

        async Task SaveTaskInfoCommandAct()
        {
            // TODO API call
            var serviceVisitItemId = TargetTask.ServiceVisitItemNumber;

            if (IsBusy) return;

            IsBusy = true;
            bool isSucceed = await _taskService.EditTaskInformation(serviceVisitItemId, DuplicateTaskInfo);
            IsBusy = false;

            if (isSucceed)
            {
                TargetTask.TaskInfo = DuplicateTaskInfo;
            }
            else
            {
                MessagingCenter.Send<TaskDetailsViewModel>(this, "TaskInfoUpdateFailed");
            }
        }
    }
}
