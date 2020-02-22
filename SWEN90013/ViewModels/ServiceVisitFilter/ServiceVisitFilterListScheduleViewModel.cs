using System;
using SWEN90013.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SWEN90013.Enums;


namespace SWEN90013.ViewModels
{
    public class ServiceVisitFilterListScheduleViewModel
    {
        private ServiceVisitSchedule SelectedSchedule;

        public ServiceVisitFilterListScheduleViewModel(ServiceVisitSchedule selectedSchedule)
        {
            this.SelectedSchedule = selectedSchedule;
            InitializeComponent();
        }

        private List<ServiceVisitFilterScheduleViewModel> _schedules;
        public event PropertyChangedEventHandler PropertyChanged;

        public List<ServiceVisitFilterScheduleViewModel> Schedules
        {
            get { return _schedules; }
            set
            {

                _schedules = value;

                OnPropertyChanged(nameof(Schedules));
            }
        }

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void InitializeComponent()
        {
            this.Schedules = new List<ServiceVisitFilterScheduleViewModel>();

            foreach (ServiceVisitSchedule schedule in Enum.GetValues(typeof(ServiceVisitSchedule)))
            {
                var scheduleVM = new ServiceVisitFilterScheduleViewModel();
                scheduleVM.IsSelected = false;
                scheduleVM.Schedule = schedule;
                if (schedule == this.SelectedSchedule)
                {
                    scheduleVM.IsSelected = true;
                }
                this.Schedules.Add(scheduleVM);
            }

        }

        public void SelectSchedule(ServiceVisitSchedule selectedSchedule)
        {
            foreach (ServiceVisitFilterScheduleViewModel viewModel in this.Schedules)
            {
                if (viewModel.Schedule.Equals(selectedSchedule))
                {
                    viewModel.IsSelected = true;
                }
                else
                {
                    viewModel.IsSelected = false;
                }
            }
 
        }
    }
}
