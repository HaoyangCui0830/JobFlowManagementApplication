using System;
using System.Collections.Generic;
using SWEN90013.ViewModels;
using Xamarin.Forms;

namespace SWEN90013.Views
{
    public partial class BarcodeInputManuallyPage : ContentPage
    {
        BarcodeInputManuallyViewModel viewmodel;
        TaskListViewModel taskListViewModel;
        public BarcodeInputManuallyPage(BarcodeInputManuallyViewModel viewModel)
        {
            InitializeComponent();
            viewmodel = viewModel;
            this.BindingContext = viewmodel;
            taskListViewModel = viewmodel.TaskListViewModel;
            //MessagingCenter.Unsubscribe<BarcodeInputManuallyViewModel, string>(this, "NewTask");
            Console.WriteLine("Before Subscribe");
            MessagingCenter.Subscribe<BarcodeInputManuallyViewModel, string>(this, "NewTask", async (sender, barcode) =>
            {
                //Device.BeginInvokeOnMainThread(async() => await Navigation.PushAsync(new LoginPage()) );
                Console.WriteLine("After Subscribe");
                Device.BeginInvokeOnMainThread(async () =>
                //await DisplayAlert("It's a new task, should jump to a new task page", barcode, "OK"));
                {
                    bool answer = await DisplayAlert("This is new barcode", "Would you like to create a new task?", "Yes", "No");
                    if (answer)
                    {
                        AddNewTaskPageViewModel addTaskViewModel = new AddNewTaskPageViewModel(barcode, taskListViewModel);
                        await Navigation.PushAsync(new AddNewTaskPage(addTaskViewModel));
                    }
                }
                );
            }
            );
            //MessagingCenter.Unsubscribe<BarcodeInputManuallyViewModel, string>(this, "NewTask");
            MessagingCenter.Subscribe<BarcodeInputManuallyViewModel, string>(this, "ExistedTask", async (sender, barcode) =>
            {
                //Device.BeginInvokeOnMainThread(async () => await Navigation.PushAsync(new LoginPage()));
                Device.BeginInvokeOnMainThread(async () =>
                //await DisplayAlert("It's an existed task, should jump to existed task page", barcode, "OK")
                await Navigation.PushAsync(new TaskPage(viewModel.ExistedTask))
                );
            }
            );
        }

        public async void ToTaskPage()
        {
            
        }
    }
}
