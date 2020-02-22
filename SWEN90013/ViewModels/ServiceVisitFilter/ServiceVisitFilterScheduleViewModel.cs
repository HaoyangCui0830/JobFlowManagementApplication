using System;
using SWEN90013.Enums;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace SWEN90013.ViewModels
{
    public class ServiceVisitFilterScheduleViewModel : INotifyPropertyChanged
    {
        public ServiceVisitFilterScheduleViewModel()
        { 
        }

        private Boolean _isSelected;
        public ServiceVisitSchedule Schedule { get; set; }
        private String _scheduleDescription;

        public event PropertyChangedEventHandler PropertyChanged;

        public Boolean IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public String ScheduleDescription
        {
            get
            {
                return ServiceVisitScheduleMethods.GetDescription(this.Schedule);
            }
            set
            {
                _scheduleDescription = value;
            }
        }

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
 
}
