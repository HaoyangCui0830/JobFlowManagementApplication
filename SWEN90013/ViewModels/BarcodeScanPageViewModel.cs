using System;
using System.Collections.Generic;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace SWEN90013.ViewModels
{
    public class BarcodeScanPageViewModel
    {
        private String _result;
        private List<TaskViewModel> _undoneTasks;
        private List<TaskViewModel> _doneTasks;
        private TaskViewModel _existedTasks;
        private TaskListViewModel _taskListViewModel;
        ZXingScannerView _zxing = new ZXingScannerView();

        public BarcodeScanPageViewModel(List<TaskViewModel> toDoTasks, List<TaskViewModel> doneTasks, TaskListViewModel taskListViewModel)
        {
            _undoneTasks = toDoTasks;
            _doneTasks = doneTasks;
            _taskListViewModel = taskListViewModel;
        }

        public ZXing.Result Result
        {
            set
            {
                _result = value.Text;
            }
        }

        public TaskListViewModel TaskListViewModel
        {
            get
            {
                return _taskListViewModel;
            }
        }

        public List<TaskViewModel> UndoneTasks
        {
            get
            {
                return _undoneTasks;
            }
        }
        public List<TaskViewModel> DoneTasks
        {
            get
            {
                return _doneTasks;
            }
        }

        public TaskViewModel ExistedTask
        {
            get
            {
                return _existedTasks;
            }
        }

        public Command QRScanResultCommand => new Command(() =>
        {
            if (_undoneTasks != null) {
                foreach (TaskViewModel task in _undoneTasks)
                {
                    if (Int64.Parse(_result) == task.TaskInfo.Barcode)
                    {
                        _existedTasks = task;
                        MessagingCenter.Send(this, "ExistedTask", _result);
                        return;
                    }
                }
            }
            if (_doneTasks != null)
            {
                foreach (TaskViewModel task in _doneTasks)
                {
                    if (Int64.Parse(_result) == task.TaskInfo.Barcode)
                    {
                        _existedTasks = task;
                        MessagingCenter.Send(this, "ExistedTask", _result);
                        return;
                    }
                }
            }
            MessagingCenter.Send(this, "NewTask", _result);
            
        });

    }
}
