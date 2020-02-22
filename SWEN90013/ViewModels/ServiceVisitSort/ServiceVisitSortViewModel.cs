using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SWEN90013.Enums;

namespace SWEN90013.ViewModels.ServiceVisitSort
{
    public class ServiceVisitSortViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private String _iconName;

        public String IconName
        {
            get
            {
                return _iconName;
            }
            set
            {
                _iconName = value;
            }
        }

        private ServiceVisitSortType _sortType;

        public ServiceVisitSortType SortType
        {
            get
            {
                return _sortType;
            }
            set
            {
                _sortType = value;
            }
        }

        private String _location;

        public String Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
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

        private bool _isInnerClickEnable;

        public bool IsInnerClickEnable
        {
            get
            {
                return _isInnerClickEnable;
            }
            set
            {
                _isInnerClickEnable = value;
            }
        }

        private String _title;

        public String Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }

        public ServiceVisitSortViewModel()
        {

        }
    }
}
