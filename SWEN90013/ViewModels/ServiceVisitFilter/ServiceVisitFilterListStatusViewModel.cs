using System;
using SWEN90013.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SWEN90013.Enums;

namespace SWEN90013.ViewModels
{
    public class ServiceVisitFilterListStatusViewModel
    {
        private ServiceVisitStatus SelectedStatus;
        private List<ServiceVisitFilterStatusViewModel> _status;
        public event PropertyChangedEventHandler PropertyChanged;

        public ServiceVisitFilterListStatusViewModel(ServiceVisitStatus selectedStatus)
        {
            this.SelectedStatus = selectedStatus;
            InitializeComponent();
        }

        public List<ServiceVisitFilterStatusViewModel> Status
        {
            get { return _status; }
            set
            {

                _status = value;

                OnPropertyChanged(nameof(Status));
            }
        }

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void InitializeComponent()
        {
            this.Status = new List<ServiceVisitFilterStatusViewModel>();

            foreach (ServiceVisitStatus status in Enum.GetValues(typeof(ServiceVisitStatus)))
            {
                var statusVM = new ServiceVisitFilterStatusViewModel();
                statusVM.IsSelected = false;
                statusVM.Status = status;
                if (status == this.SelectedStatus)
                {
                    statusVM.IsSelected = true;
                }
                this.Status.Add(statusVM);
            }

        }

        public void SelectStatus(ServiceVisitStatus selectedStatus)
        {
            foreach (ServiceVisitFilterStatusViewModel viewModel in this.Status)
            {
                if (viewModel.Status.Equals(selectedStatus))
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
