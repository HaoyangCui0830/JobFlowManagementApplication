using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace SWEN90013.ViewModels
{
    // View model that is used to show site locations on the Map
    public class ServiceVisitMapViewModel : INotifyPropertyChanged
    {
        // Full service visit list
        private List<ServiceVisitViewModel> _fullVisit;
        private bool _isBusy;

        public ServiceVisitMapViewModel(List<ServiceVisitViewModel> visits)
        {
            this._fullVisit = visits;

        }

        // get the Full visit list info
        public List<ServiceVisitViewModel> FullVisitsList
        {
            get
            {
                return _fullVisit;
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

