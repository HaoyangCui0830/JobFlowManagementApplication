using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using SWEN90013.Enums;
using SWEN90013.Models;

namespace SWEN90013.ViewModels
{
    public class TimelineViewModel : INotifyPropertyChanged
    {
        private ObservableCollection <TimelineDefectViewModel> _defects;
        private DateTime _testDate;
        private String _dateResult;
        private Boolean _isDefectsEmpty;
        private String _colorResult;
        private TaskResultStatus _status;

        public TimelineViewModel()
        {

        }
        public TimelineViewModel(Timeline timeline)
        {
            _testDate = timeline.date;
            if (timeline.result == "Pass")
            {
                _status = TaskResultStatus.Passed;
            }
            else
            {
                _status = TaskResultStatus.Failed;
            }

            var defects = timeline.DefectReport.Select(s => new TimelineDefectViewModel(s)).ToList();
            _defects = new ObservableCollection<TimelineDefectViewModel>(defects);

            if (defects.Count > 0)
            {
                IsDefectsEmpty = false;
                IsDefectsNotEmpty = true;
            }
            else
            {
                IsDefectsEmpty = true;
                IsDefectsNotEmpty = false;
            }
        }
        // to show whether the task test is pass or fail
        public TaskResultStatus Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }

        // represents the color label based on the task test result
        public String ColorResult
        {
            get
            {
                return TaskResultStatusMethods.GetColor(Status);
            }
            set
            {
                _colorResult = value;
            }
        }

        public Boolean IsDefectsEmpty
        {
            get
            {
                return _isDefectsEmpty;
            }
            set
            {
                _isDefectsEmpty = value;
            }
        }

        private Boolean _isDefectsNotEmpty;

        public Boolean IsDefectsNotEmpty
        {
            get
            {
                return _isDefectsNotEmpty;
            }
            set
            {
                _isDefectsNotEmpty = value;
            }
        }

        public String DateResult
        {
            get
            {
                return GetFormattedResultDate();
            }
            set
            {
                _dateResult = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
      
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // show the date of the task test
        public DateTime TestDate
        {
            get
            {
                return _testDate;
            }
            set
            {
                _testDate = value;
            }
        }

        // all defect reports
        public ObservableCollection <TimelineDefectViewModel> Defects
        {
            get { return _defects; }
            set { _defects = value;
                OnPropertyChanged(nameof(Defects));
                    }
        }

        // Combine date result and value
        public String GetFormattedResultDate()
        {
            string date = string.Format("On {0} :  {1}", TestDate.ToString("dd/MM/yyyy"), Status.ToString());
            return date;
        }
    }
}
