using System;
using System.Collections.Generic;
using SWEN90013.ViewModels;
using SWEN90013.Enums;
using Xamarin.Forms;

namespace SWEN90013.Views.ServiceVisitPages
{
    public partial class ServiceVisitFilterStatusPage : ContentPage
    {
        private ServiceVisitFilterListStatusViewModel ViewModel;

        public ServiceVisitFilterStatusPage(ServiceVisitFilterListStatusViewModel viewModel)
        {
            InitializeComponent();
            this.ViewModel = viewModel;
            BindingContext = ViewModel;
        }

        public void Status_Tapped(Object sender, System.EventArgs e)
        {
            var listView = (ListView)sender;
            var scheduleSelected = (ServiceVisitFilterStatusViewModel)listView.SelectedItem;

            this.ViewModel.SelectStatus(scheduleSelected.Status);

            //Send information of status selected to filter page
            MessagingCenter.Send<ServiceVisitFilterStatusPage, ServiceVisitStatus>(this, "FilterServiceVisitStatus", scheduleSelected.Status);
            Navigation.PopAsync();
        }
    }
}

