using System;
using System.Collections.Generic;
using SWEN90013.ViewModels.ServiceVisitSort;

using Xamarin.Forms;

namespace SWEN90013.Views.ServiceVisitSortPages
{
    public partial class ServiceVisitSortPage : ContentPage
    {
        public ServiceVisitSortListViewModel ViewModel; 

        public ServiceVisitSortPage(ServiceVisitSortListViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            BindingContext = ViewModel;
        }

        public void Location_Clicked(object sender, System.EventArgs e)
        {

        }

        private void Item_Tapped(object sender, ItemTappedEventArgs e)
        {
            var listView = (ListView)sender;
            var sortVM = (ServiceVisitSortViewModel)listView.SelectedItem;

            this.ViewModel.SelectSortSetting(sortVM);
        }

        public void Confirm_Clicked(object sender, System.EventArgs e)
        {
            MessagingCenter.Send<ServiceVisitSortPage, ServiceVisitSortListViewModel>(this, "FilterServiceVisitSort", this.ViewModel);
            Navigation.PopAsync();
        }
    }
}
