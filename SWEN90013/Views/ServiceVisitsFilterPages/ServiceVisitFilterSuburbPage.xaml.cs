using System;
using System.Collections.Generic;
using SWEN90013.ViewModels;

using Xamarin.Forms;

namespace SWEN90013.Views.ServiceVisitPages
{
    public partial class ServiceVisitFilterSuburbPage : ContentPage
    {
        private ServiceVisitFilterListSuburbViewModel ViewModel;

        public ServiceVisitFilterSuburbPage(ServiceVisitFilterListSuburbViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
            BindingContext = ViewModel;
        }

        private void Suburb_Tapped(object sender, ItemTappedEventArgs e)
        {
            var listView = (ListView)sender;
            var suburb = (ServiceVisitFilterSuburbViewModel)listView.SelectedItem;
            var suburbName = suburb.SuburbName;

            this.ViewModel.SelectSuburb(suburbName);
        }

        private void Confirm_Clicked(object sender, System.EventArgs e)
        {
            //Sending the information of suburb selected to filter page
            MessagingCenter.Send<ServiceVisitFilterSuburbPage, List<ServiceVisitFilterSuburbViewModel>>(this, "FilterServiceVisitSuburb",this.ViewModel.Suburbs);
            Navigation.PopAsync();
        }
    }

}
