using System;
using Xamarin.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace SWEN90013.ViewModels
{
    public class ServiceVisitFilterSuburbViewModel : INotifyPropertyChanged
    {
       
        public ServiceVisitFilterSuburbViewModel()
        {

        }
        private Boolean _isSelected;

        public String SuburbName { get; set; }

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

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

