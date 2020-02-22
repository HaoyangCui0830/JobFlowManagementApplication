using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace SWEN90013.ViewModels
{
    class TaskPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isShowingDetails = true;
        private bool _isShowingTest = false;
        private bool _isShowingTimeline = false;

        public bool IsShowingDetails
        {
            get => _isShowingDetails;
            set
            {
                _isShowingDetails = value;
                OnPropertyChanged();
            }
        }
        public bool IsShowingTest
        {
            get => _isShowingTest;
            set
            {
                _isShowingTest = value;
                OnPropertyChanged();
            }
        }
        public bool IsShowingTimeline
        {
            get => _isShowingTimeline;
            set
            {
                _isShowingTimeline = value;
                OnPropertyChanged();
            }
        }
        public Command<int> SegControlCommand { get; set; }

        /// <summary>
        /// This is the command for the case when segControl value was changed
        /// </summary>
        /// <param name="val">the new segControl value</param>
        void SegControlCommandAct (int val)
        {
            if (val == 0)
            {
                IsShowingDetails = true;
                IsShowingTest = false;
                IsShowingTimeline = false;
            }
            else if (val == 1)
            {
                IsShowingDetails = false;
                IsShowingTest = true;
                IsShowingTimeline = false;
            }
            if (val == 2)
            {
                IsShowingDetails = false;
                IsShowingTest = false;
                IsShowingTimeline = true;
            }
        }

        /// <summary>
        /// This is the constructor for TaskPageViewModel
        /// </summary>
        public TaskPageViewModel()
        {
            SegControlCommand = new Command<int>(SegControlCommandAct);
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
