using SWEN90013.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using SWEN90013.ViewModels;


namespace SWEN90013.Views
{
    public partial class TaskListPage : ContentPage
    {

        TaskListViewModel ViewModel;

        /// <summary>
        /// This is the constructor for Task List Page
        /// </summary>
        public TaskListPage(int serviceVisitId)
        {
            InitializeComponent();

            ViewModel = new TaskListViewModel(serviceVisitId)
            {
                Navigation = Navigation
            };
            BindingContext = ViewModel;

        }


        public async void BarcodeIconClicked(object sender, System.EventArgs e)
        {
            List<TaskViewModel> toDoTasksList = new List<TaskViewModel>();
            List<TaskViewModel> doneTasksList = new List<TaskViewModel>();

            if (ViewModel.ToDoTasks != null)
            {

                toDoTasksList = new List<TaskViewModel>(ViewModel.ToDoTasks);
            }
            if(ViewModel.DoneTasks != null) 
            {
                doneTasksList = new List<TaskViewModel>(ViewModel.DoneTasks);
            } 


            var barcodeScanPageViewModel = new BarcodeScanPageViewModel(toDoTasksList, doneTasksList, ViewModel);
            await Navigation.PushAsync(new BarcodeScanPage(barcodeScanPageViewModel));
        }

        public async void AddNewTaskClicked(Object sender, System.EventArgs e)
        {
            
            var AddNewTaskPageViewModel = new AddNewTaskPageViewModel(ViewModel);
            await Navigation.PushAsync(new AddNewTaskPage(AddNewTaskPageViewModel));
        }

        /// <summary>
        /// Everytime the page appear, make an API call to make sure the list is updated
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(ViewModel.IsBusy != true)
            {
                _ = ViewModel.InitializeGettingTasks();
            }
        }
    }
}

