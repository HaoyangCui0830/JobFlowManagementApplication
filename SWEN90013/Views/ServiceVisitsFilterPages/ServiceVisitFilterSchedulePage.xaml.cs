using System;
using System.Collections.Generic;
using SWEN90013.ViewModels;
using SWEN90013.Enums;

using Xamarin.Forms;

namespace SWEN90013.Views.ServiceVisitPages
{
    public partial class ServiceVisitFilterSchedulePage : ContentPage
    {
        private ServiceVisitFilterListScheduleViewModel ViewModel;

        public ServiceVisitFilterSchedulePage(ServiceVisitFilterListScheduleViewModel viewModel)
        {
            InitializeComponent();
            this.ViewModel = viewModel;
            BindingContext = ViewModel;
        }

        public void Schedule_Tapped(Object sender, System.EventArgs e)
        {
            var listView = (ListView)sender;
            var scheduleSelected = (ServiceVisitFilterScheduleViewModel)listView.SelectedItem;
      
            this.ViewModel.SelectSchedule(scheduleSelected.Schedule);

            //send information of schedule selected to filter page
            MessagingCenter.Send<ServiceVisitFilterSchedulePage, ServiceVisitSchedule>(this, "FilterServiceVisitSchedule", scheduleSelected.Schedule);
            Navigation.PopAsync();
        }
    }

}
