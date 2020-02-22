using Xamarin.Forms;
using ZXing;
using ZXing.Net.Mobile.Forms;
using SWEN90013.ViewModels;
using System;

namespace SWEN90013.Views
{
    public partial class BarcodeScanPage : ContentPage
    {
        BarcodeScanPageViewModel viewModel;
        int currentThread;
        TaskListViewModel taskListViewModel;
        public BarcodeScanPage(BarcodeScanPageViewModel vm)
        {
            InitializeComponent();
            viewModel = vm;
            BindingContext = viewModel;
            taskListViewModel = viewModel.TaskListViewModel;
        }

        public void Handle_OnScanResult(Result result)
		{
            //Device.BeginInvokeOnMainThread(async() =>
            //{
            //await DisplayAlert("Scanned result", result.Text, "OK");
            //MessagingCenter.Subscribe<BarcodeScanPageViewModel, string>(this, "NewTask", async (sender, barcode) =>
            //await DisplayAlert("Scanned result", barcode, "OK")
            //await Navigation.PushAsync(new LoginPage());
            //);
            //});
            MessagingCenter.Unsubscribe<BarcodeInputManuallyViewModel, string>(this, "NewTask");
            Console.WriteLine("Before Subscribe");
            if (currentThread != Environment.CurrentManagedThreadId)
            {
                currentThread = Environment.CurrentManagedThreadId;
                MessagingCenter.Subscribe<BarcodeScanPageViewModel, string>(this, "NewTask", async (sender, barcode) =>
                {
                    
                    MessagingCenter.Unsubscribe<BarcodeScanPageViewModel, string>(this, "NewTask");
                    //Device.BeginInvokeOnMainThread(async() => await Navigation.PushAsync(new LoginPage()) );
                    Device.BeginInvokeOnMainThread(async () =>
                    //await DisplayAlert("It's a new task, should jump to a new task page", barcode, "OK"));
                    {
                        bool answer = await DisplayAlert("This is new barcode", "Would you like to create a new task?", "Yes", "No");
                        MessagingCenter.Unsubscribe<BarcodeInputManuallyViewModel, string>(this, "NewTask");
                        if (answer)
                        {
                            AddNewTaskPageViewModel addTaskViewModel = new AddNewTaskPageViewModel(barcode, taskListViewModel);
                            await Navigation.PushAsync(new AddNewTaskPage(addTaskViewModel));
                            currentThread = 0;
                        }
                    }
                );
                }
            );
            }
            
            
            MessagingCenter.Subscribe<BarcodeScanPageViewModel, string>(this, "ExistedTask", async (sender, barcode) =>
                {
                    MessagingCenter.Unsubscribe<BarcodeScanPageViewModel, string>(this, "ExistedTask");
                    //Device.BeginInvokeOnMainThread(async () => await Navigation.PushAsync(new LoginPage()));
                    Device.BeginInvokeOnMainThread(async () =>
                    //await DisplayAlert("It's an existed task, should jump to existed task page", barcode, "OK")
                    await Navigation.PushAsync(new TaskPage(viewModel.ExistedTask))
                    );
                }
            );

        }

        private async void Input_Barcode_Manually(object sender, System.EventArgs e)
        {
            var viewmodel = new BarcodeInputManuallyViewModel(viewModel.UndoneTasks, viewModel.DoneTasks, taskListViewModel);
            await Navigation.PushAsync(new BarcodeInputManuallyPage(viewmodel));
        }

        protected override void OnAppearing()
		{
			base.OnAppearing();

			_scanView.IsScanning = true;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			_scanView.IsScanning = false;
		}
    }
}

