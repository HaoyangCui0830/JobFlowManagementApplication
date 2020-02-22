using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SWEN90013.Enums;
using Xamarin.Essentials;

namespace SWEN90013.ViewModels.ServiceVisitSort
{
    public class ServiceVisitSortListViewModel: INotifyPropertyChanged
    {
        private List<ServiceVisitSortViewModel> _menus;
        public event PropertyChangedEventHandler PropertyChanged;
        private ServiceVisitSortType _selectedSortType;

        public ServiceVisitSortType SelectedSortType
        {
            get
            {
                return _selectedSortType;
            }
            set
            {
                _selectedSortType = value;
            }
        }

        public List<ServiceVisitSortViewModel> Menus
        {
            get
            {
                return _menus;
            }
            set
            {
                _menus = value;
                OnPropertyChanged(nameof(Menus));
            }
        }

        public ServiceVisitSortListViewModel()
        {
            InitializeMenus();
        }

        public void InitializeMenus()
        {
            var suburbAsc = new ServiceVisitSortViewModel();
            suburbAsc.Title = "Sort by Suburb (A-Z)";
            suburbAsc.IsInnerClickEnable = false;
            suburbAsc.IsSelected = false;
            suburbAsc.SortType = ServiceVisitSortType.SuburbAsc;
            suburbAsc.IconName = "arrow-pointing-down.png";

            var suburbDesc = new ServiceVisitSortViewModel();
            suburbDesc.Title = "Sort by Suburb (Z-A)";
            suburbDesc.IsInnerClickEnable = false;
            suburbDesc.IsSelected = false;
            suburbDesc.SortType = ServiceVisitSortType.SuburbDesc;
            suburbDesc.IconName = "arrow-pointing-up.png";

            var duedateAsc = new ServiceVisitSortViewModel();
            duedateAsc.Title = "Sort by Due Date";
            duedateAsc.IsInnerClickEnable = false;
            duedateAsc.IsSelected = false;
            duedateAsc.SortType = ServiceVisitSortType.DueDateAsc;
            duedateAsc.IconName = "arrow-pointing-down.png";

            var duedateDesc = new ServiceVisitSortViewModel();
            duedateDesc.Title = "Sort by Due Date";
            duedateDesc.IsInnerClickEnable = false;
            duedateDesc.IsSelected = false;
            duedateDesc.SortType = ServiceVisitSortType.DueDateDesc;
            duedateDesc.IconName = "arrow-pointing-up.png";

            var taskAsc = new ServiceVisitSortViewModel();
            taskAsc.Title = "Sort by Number of Tasks";
            taskAsc.IsInnerClickEnable = false;
            taskAsc.IsSelected = false;
            taskAsc.SortType = ServiceVisitSortType.TaskAsc;
            taskAsc.IconName = "arrow-pointing-down.png";

            var taskDesc = new ServiceVisitSortViewModel();
            taskDesc.Title = "Sort by Number of Tasks";
            taskDesc.IsInnerClickEnable = false;
            taskDesc.IsSelected = false;
            taskDesc.SortType = ServiceVisitSortType.TaskDesc;
            taskDesc.IconName = "arrow-pointing-up.png";

            var locAsc = new ServiceVisitSortViewModel();
            locAsc.Title = "Sort by Current Location";
            locAsc.IsInnerClickEnable = false;
            locAsc.IsSelected = false;
            locAsc.Location = "(Near You)";
            locAsc.SortType = ServiceVisitSortType.LocAsc;
            locAsc.IconName = "arrow-pointing-down.png";

            var locDesc = new ServiceVisitSortViewModel();
            locDesc.Title = "Sort by Current Location";
            locDesc.IsInnerClickEnable = false;
            locDesc.IsSelected = false;
            locDesc.Location = "(Near You)";
            locDesc.SortType = ServiceVisitSortType.LocDesc;
            locDesc.IconName = "arrow-pointing-up.png";

            Menus = new List<ServiceVisitSortViewModel>();
            Menus.Add(suburbAsc);
            Menus.Add(suburbDesc);
            Menus.Add(duedateAsc);
            Menus.Add(duedateDesc);
            Menus.Add(taskAsc);
            Menus.Add(taskDesc);
            Menus.Add(locAsc);
            Menus.Add(locDesc);
        }

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SelectSortSetting(ServiceVisitSortViewModel selectedVM)
        {
            foreach(ServiceVisitSortViewModel sortVM in Menus)
            {
                if(selectedVM.Equals(sortVM))
                {
                    sortVM.IsSelected = !sortVM.IsSelected;

                    if (sortVM.IsSelected)
                    {
                        _selectedSortType = sortVM.SortType;
                    }
                    else
                    {
                        _selectedSortType = ServiceVisitSortType.None;
                    }

                }
                else
                {
                    sortVM.IsSelected = false;
                }
            }
        }

        public void RemoveSortSetting()
        {
            this.SelectedSortType = ServiceVisitSortType.None;
            foreach (ServiceVisitSortViewModel sortVM in Menus)
            { 
                sortVM.IsSelected = false;
            }   
        }
    }
}
