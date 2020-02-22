using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Windows.Input;

namespace SWEN90013.ViewModels
{

    public class BarcodeInputManuallyViewModel : INotifyPropertyChanged
    {
        private string _barcode;

        private List<TaskViewModel> _undoneTasks;
        private List<TaskViewModel> _doneTasks;
        private TaskViewModel _existedTasks;
        private TaskListViewModel _taskListViewModel;


        public event PropertyChangedEventHandler PropertyChanged;

        public BarcodeInputManuallyViewModel(List<TaskViewModel> toDoTasks, List<TaskViewModel> doneTasks, TaskListViewModel taskListViewModel)
        {
            SubmitBarcodeCommand = new Command(SubmitBarcode);
            _undoneTasks = toDoTasks;
            _doneTasks = doneTasks;
            _taskListViewModel = taskListViewModel;
        }
        
        public string Barcode
        {
            set
            {
                _barcode = value;
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

        public ICommand SubmitBarcodeCommand { get; set; }

        public async void SubmitBarcode()
        {
            if (_undoneTasks != null)
            {
                foreach (TaskViewModel task in _undoneTasks)
                {
                    if (Int64.Parse(_barcode) == task.TaskInfo.Barcode)
                    {
                        _existedTasks = task;
                        MessagingCenter.Send(this, "ExistedTask", _barcode);
                        return;
                    }
                }
            }
            if (_doneTasks != null)
            {
                foreach (TaskViewModel task in _doneTasks)
                {
                    if (Int64.Parse(_barcode) == task.TaskInfo.Barcode)
                    {
                        _existedTasks = task;
                        MessagingCenter.Send(this, "ExistedTask", _barcode);
                        return;
                    }
                }
            }
            
            MessagingCenter.Send(this, "NewTask", _barcode);
        }
    }
}
