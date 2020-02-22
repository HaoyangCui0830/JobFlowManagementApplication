using System;
using System.Collections.Generic;
using SWEN90013.ViewModels;
using Xamarin.Forms;

namespace SWEN90013.Views
{
    public partial class EquipmentListPage : ContentPage
    {
        public EquipmentListViewModel viewModel;
        public EquipmentListPage(EquipmentListViewModel vm)
        {
            InitializeComponent();
            viewModel = vm;
            BindingContext = viewModel;
        }
        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            String targetEquipment = e.Item.ToString();

            MessagingCenter.Send<EquipmentListPage, String>(this, "Equipment", targetEquipment);
            Navigation.PopAsync();
        }
    }
}
