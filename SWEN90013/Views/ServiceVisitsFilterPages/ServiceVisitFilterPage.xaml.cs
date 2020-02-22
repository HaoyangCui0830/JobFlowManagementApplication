using System;
using System.Collections.Generic;
using SWEN90013.ViewModels;
using Xamarin.Forms;
using SWEN90013.Enums;

namespace SWEN90013.Views.ServiceVisitPages
{
    public partial class ServiceVisitFilterPage : ContentPage
    {
        private ServiceVisitFilterViewModel ViewModel;

        public ServiceVisitFilterPage(ServiceVisitFilterViewModel viewModel)
        {
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
            ViewModel = viewModel;

            SubscribingInformation();
            BindingContext = ViewModel;
        }

        public async void Suburb_Clicked(object sender, System.EventArgs e)
        {
            var viewModel = new ServiceVisitFilterListSuburbViewModel(ViewModel.Suburbs,ViewModel.SelectedSuburbs);
            await Navigation.PushAsync(new ServiceVisitFilterSuburbPage(viewModel), false);
        }

        private void SubscribingInformation()
        {
            MessagingCenter.Subscribe<ServiceVisitFilterSuburbPage, List<ServiceVisitFilterSuburbViewModel>>(this, "FilterServiceVisitSuburb", (sender, arg) => {
                this.ViewModel.ConfirmSuburbs(arg);
            });

            MessagingCenter.Subscribe<ServiceVisitFilterSchedulePage, ServiceVisitSchedule>(this, "FilterServiceVisitSchedule", (sender, arg) => {
                this.ViewModel.SelectSchedule(arg);
            });

            MessagingCenter.Subscribe<ServiceVisitFilterStatusPage, ServiceVisitStatus>(this, "FilterServiceVisitStatus", (sender, arg) => {
                this.ViewModel.SelectStatus(arg);
            });
        }

        public async void Schedule_Clicked(object sender, System.EventArgs e)
        {
            var viewModel = new ServiceVisitFilterListScheduleViewModel(this.ViewModel.SelectedSchedule);
            await Navigation.PushAsync(new ServiceVisitFilterSchedulePage(viewModel), false);
        }

        public async void Status_Clicked(object sender, System.EventArgs e)
        {
            var viewModel = new ServiceVisitFilterListStatusViewModel(this.ViewModel.SelectedStatus);
            await Navigation.PushAsync(new ServiceVisitFilterStatusPage(viewModel), false);
        }

        public void AllDueDate_Tapped(object sender, System.EventArgs e)
        {
            this.ViewModel.SelectAllDueDate();
        }

        public void ThisMonthDueDate_Tapped(object sender, System.EventArgs e)
        {
            this.ViewModel.SelectThisMonthDueDate();
        }

        public void Confirm_Clicked(object sendier, System.EventArgs e)
        {
            MessagingCenter.Send<ServiceVisitFilterPage, ServiceVisitFilterViewModel>(this, "FilterServiceVisitFilterSettings", ViewModel);
            Navigation.PopAsync();
        }

    }
}
