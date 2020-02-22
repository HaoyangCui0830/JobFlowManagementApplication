using System;
using System.Collections.Generic;
using SWEN90013.ViewModels;
using SWEN90013.Views.ServiceVisitPages;
using SWEN90013.Views.ServiceVisitSortPages;
using Xamarin.Forms;
using SWEN90013.ViewModels.ServiceVisitSort;

namespace SWEN90013.Views
{
    public partial class ServiceVisitListPage : ContentPage
    {


        ServiceVisitListViewModel ViewModel;
        private int Selected_Schedule_Button_Id;

        public ServiceVisitListPage()
        {
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
            SubscribingInformation();
            var viewModel = new ServiceVisitListViewModel
            {
                Navigation = Navigation
            };
            BindingContext = viewModel;
            ViewModel = viewModel;
			NavigationPage.SetHasBackButton(this, false);
		}

		public async void ServiceVisit_Clicked(object sender, ItemTappedEventArgs e)
        {
            var myListView = (ListView)sender;
            var selectedVisit = (ServiceVisitViewModel)myListView.SelectedItem;

            await Navigation.PushAsync(new ServiceVisitPage(selectedVisit));
        }


        async void Handle_Map_Clicked(object sender, System.EventArgs e)
        {
            var serviceVisitMapViewModel = new ServiceVisitMapViewModel(ViewModel.FullVisitsList);
            await Navigation.PushAsync(new ServiceVisitMapPage(serviceVisitMapViewModel));
        }

        async void Handle_Calendar_Clicked(object sender, System.EventArgs e)
        {
            var serviceVisitCalendarViewModel = new ServiceVisitCalendarViewModel(ViewModel.FullVisitsList);
            await Navigation.PushAsync(new ServiceVisitCalendarPage(serviceVisitCalendarViewModel));
        }

        // Handle datepicker event, set the scheduled date
        void Add_Schedule(object sender, System.EventArgs e)
        {
            Selected_Schedule_Button_Id = sender.GetHashCode();
            DatePicker datePicker = scheduleDatePicker;
            if(Selected_Schedule_Button_Id == sender.GetHashCode())
            {
                IsEnabled = true;
                datePicker.Focus();
                datePicker.Unfocused += (s, date_select_event) =>
                {
                    TimePicker timePicker = scheduleTimePicker;
                    IsEnabled = true;
                    timePicker.Focus();
                    timePicker.Unfocused += (time_select_sender, time_select_event) =>
                    {
                        if(Selected_Schedule_Button_Id == ((Button)sender).GetHashCode())
                        {
                            ((Button)sender).Text = "Scheduled for: " + datePicker.Date.ToString("dd/MM/yyyy") + " " + timePicker.Time.ToString();
                            //Console.WriteLine(sender.GetHashCode());
                            MessagingCenter.Send(this, "ScheduledDateTime"+ ((Button)sender).CommandParameter.ToString(),
                                DateTime.Parse(datePicker.Date.ToString("dd/MM/yyyy")
                                + " " + timePicker.Time.ToString()));
                        }
                    };
                };
            }
        }


        private async void Filter_Clicked(object sender, System.EventArgs e)
        {
            var viewModel = this.ViewModel.FilterViewModel;
            await Navigation.PushAsync(new ServiceVisitFilterPage(viewModel), false);
        }

        private async void Sort_Clicked(object sender, System.EventArgs e)
        {
            var viewModel = this.ViewModel.SortListViewModel;
            await Navigation.PushAsync(new ServiceVisitSortPage(viewModel), false);
        }

        private void SubscribingInformation()
        {
            MessagingCenter.Subscribe<ServiceVisitFilterPage, ServiceVisitFilterViewModel >(this, "FilterServiceVisitFilterSettings", (sender, arg) => {
                this.ViewModel.UpdateVisitsBasedOnFilter(arg);
            });

            MessagingCenter.Subscribe<ServiceVisitSortPage, ServiceVisitSortListViewModel>(this, "FilterServiceVisitSort", (sender, arg) => {
                this.ViewModel.UpdateVisitsBasedOnSort(arg);
            });
           
        }

    }
}
