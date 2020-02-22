using System;
using System.Collections.Generic;
using SWEN90013.ViewModels;
using Xamarin.Forms;

namespace SWEN90013.Views
{
    public partial class AddNewTaskPage : ContentPage
    {
        AddNewTaskPageViewModel viewModel;
        public AddNewTaskPage(AddNewTaskPageViewModel vm)
        {
            InitializeComponent();
            viewModel = vm;
            BindingContext = viewModel;
            viewModel.Task = viewModel.Task;
            FrameClass1.BorderColor = Color.FromRgba(0, 0, 0, 0.1);
            FrameClass2.BorderColor = Color.FromRgba(0, 0, 0, 0.1);
            MessagingCenter.Subscribe<AddNewTaskPageViewModel, Boolean>(this, "TaskSubmitStatus", (s, result) =>
            {
                //Device.BeginInvokeOnMainThread(async () => await Navigation.PushAsync(new LoginPage()));
                Device.BeginInvokeOnMainThread(async () =>
                await DisplayAlert("Submitted", "New Task Has Been Saved", "OK"));
            }
            );
            MessagingCenter.Subscribe<ContractorListPage, String>(this, "Contractor", (sender, arg) =>
            {
                String chosenCompany = arg;

                viewModel.SetANewContractor(chosenCompany);
            });
            MessagingCenter.Subscribe<EquipmentListPage, String>(this, "Equipment", (sender, arg) =>
            {
                String chosenEquiupment = arg;

                viewModel.SetANewEquipment(chosenEquiupment);
            });
        }

        public async void SaveButtonClicked(object sender, System.EventArgs e)
        {
            MessagingCenter.Subscribe<AddNewTaskPageViewModel, Boolean>(this, "TaskSubmitStatus", (s, result) =>
            {
                //Device.BeginInvokeOnMainThread(async () => await Navigation.PushAsync(new LoginPage()));
                Device.BeginInvokeOnMainThread(async () =>
                await DisplayAlert("Submitted","New Task Has Been Saved", "OK"));
            }
            );
            //await Navigation.PushAsync(new TaskListPage());
        }

        private async void ContractorButton_Clicked(object sender, EventArgs e)
        {
            ContractorListViewModel _contractorListVM = new ContractorListViewModel();
            await Navigation.PushAsync(new ContractorListPage());
        }

        private async void EquipmentButton_Clicked(object sender, EventArgs e)
        {
            EquipmentListViewModel equipmentListViewModel = new EquipmentListViewModel();
            await Navigation.PushAsync(new EquipmentListPage(equipmentListViewModel));
        }
    }
}
