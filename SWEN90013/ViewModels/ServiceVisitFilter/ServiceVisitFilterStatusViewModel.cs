using System;
using SWEN90013.Enums;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace SWEN90013.ViewModels
{
    public class ServiceVisitFilterStatusViewModel : INotifyPropertyChanged
    {
        private Boolean _isSelected;
        public ServiceVisitStatus Status { get; set; }
        private String _statusDescription;
       
        public ServiceVisitFilterStatusViewModel()
        {
        }

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

        public String StatusDescription
        {
            get
            {
                return ServiceVisitStatusMethods.GetDescription(this.Status);
            }
            set
            {
                _statusDescription = value;
            }
        }

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
