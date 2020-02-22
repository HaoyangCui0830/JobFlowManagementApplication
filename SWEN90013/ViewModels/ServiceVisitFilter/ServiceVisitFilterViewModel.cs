using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SWEN90013.Enums;

namespace SWEN90013.ViewModels
{
    public class ServiceVisitFilterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //properties to filter by suburbs
        public List<String> Suburbs;
        private String _selectedSuburbsString;
        public List<String> SelectedSuburbs;

        //properties to filter by due date
        private Boolean _isThisMonthDueDate;
        private Boolean _isAllDueDate;

        //properties to filter by schedule
        public ServiceVisitSchedule _selectedSchedule;
        public String _selectedScheduleString;

        //properties to filter by status
        public ServiceVisitStatus _selectedStatus;
        public String _selectedStatusString;

        public ServiceVisitFilterViewModel(List<ServiceVisitViewModel> visits)
        { 
            InitializeSuburbs(visits);
            InitializeSelectedSuburbs();
            InitializeSelectedDueDate();
            InitializeSelectedSchedule();
            InitializeSelectedStatus();
        }

        public ServiceVisitStatus SelectedStatus
        {
            get
            {
                return _selectedStatus;
            }
            set
            {
                _selectedStatus = value;
                this.SelectedStatusString = ServiceVisitStatusMethods.GetDescription(_selectedStatus);
            }
        }

        public String SelectedStatusString
        {
            get
            {
                return _selectedStatusString;
            }
            set
            {
                _selectedStatusString = value;
                OnPropertyChanged(nameof(SelectedStatusString));
            }
        }


        public ServiceVisitSchedule SelectedSchedule
        {
            get
            {
                return _selectedSchedule;
            }
            set
            {
                _selectedSchedule = value;
                this.SelectedScheduleString = ServiceVisitScheduleMethods.GetDescription(_selectedSchedule);
            }
        }

        public String SelectedScheduleString
        {
            get
            {
                return _selectedScheduleString;
            }
            set
            {
                _selectedScheduleString = value;
                OnPropertyChanged(nameof(SelectedScheduleString));
            }
        }

        public Boolean IsAllDueDate
        {
            get { return _isAllDueDate; }
            set
            {
                _isAllDueDate = value;
                OnPropertyChanged(nameof(IsAllDueDate));
            }
        }

        public Boolean IsThisMonthDueDate
        {
            get { return _isThisMonthDueDate; }
            set
            {
                _isThisMonthDueDate = value;
                OnPropertyChanged(nameof(IsThisMonthDueDate));
            }
        }

        public Boolean IsAllFilterSettings()
        {
            return SelectedStatusString.Equals("All") && SelectedScheduleString.Equals("All") && SelectedSuburbsString.Equals("All") && IsAllDueDate;
        }

        // Initiaize the suburbs based on service visits which have been fetched
        private void InitializeSuburbs(List<ServiceVisitViewModel> visits)
        {
            this.Suburbs = new List<String>();
            this.Suburbs.Add("All");

            visits.ForEach((item) =>
            {
                var visit = (ServiceVisitViewModel)item;
                this.Suburbs.Add(visit.SiteSuburb);
            });
        }
        //Set the default value of selected due date filter
        private void InitializeSelectedDueDate()
        {
            this.IsThisMonthDueDate = false;
            this.IsAllDueDate = true;
        }

        //Set the default value of selected suburb filter
        private void InitializeSelectedSuburbs()
        {
            this.SelectedSuburbs = new List<String>();
            this.SelectedSuburbs.Add(this.Suburbs[0]);
            this.SelectedSuburbsString = this.ConstructSelectedSuburbsString();
        }

        //Construct the string of filter by suburb based on selected suburbs
        private String ConstructSelectedSuburbsString()
        {
            var selectedString = "";

            for (int i = 0; i < this.SelectedSuburbs.Count; i++)
            {
                if (i == this.SelectedSuburbs.Count - 1)
                {
                    selectedString = selectedString + this.SelectedSuburbs[i];
                }
                else
                {
                    selectedString = selectedString + this.SelectedSuburbs[i] + ", ";
                }
            }
            return selectedString;
        }

        //Set the default value of selected schedule filter
        private void InitializeSelectedSchedule()
        {
            this.SelectedSchedule = ServiceVisitSchedule.All;
        }

        //Set the default value of selected status filter
        private void InitializeSelectedStatus()
        {
            this.SelectedStatus = ServiceVisitStatus.All;
        }

        public String SelectedSuburbsString
        {
            get { return _selectedSuburbsString; }
            set
            { 
                _selectedSuburbsString = value;
                OnPropertyChanged(nameof(SelectedSuburbsString));
            }
        }

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // This event is called after the selected suburbs are confirmed
        public void ConfirmSuburbs(List<ServiceVisitFilterSuburbViewModel> suburbs)
        {
            this.SelectedSuburbs.Clear();
            foreach (ServiceVisitFilterSuburbViewModel suburb in suburbs)
            {
                if (suburb.IsSelected)
                {
                    this.SelectedSuburbs.Add(suburb.SuburbName);
                }
            }
            this.SelectedSuburbsString = this.ConstructSelectedSuburbsString();
        }

        // Update the selected schedule
        public void SelectSchedule(ServiceVisitSchedule schedule)
        {
            this.SelectedSchedule = schedule;
        }

        // Update the selected status
        public void SelectStatus(ServiceVisitStatus status)
        {
            this.SelectedStatus = status;
        }

        // Update the selected due date
        public void SelectAllDueDate()
        {
            this.IsAllDueDate = true;
            this.IsThisMonthDueDate = false;
        }

        // Update the selected due date
        public void SelectThisMonthDueDate()
        {
            this.IsThisMonthDueDate = true;
            this.IsAllDueDate = false;
        }

    }
}
