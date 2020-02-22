using SWEN90013.ViewModels;
using SWEN90013.Views.TaskPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SWEN90013.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskPage : ContentPage
    {
        TaskPageViewModel ViewModel;

        /// <summary>
        /// This is the constructor for Task Page
        /// </summary>
        /// <param name="selectedTask">The Task currenty showing in this page</param>
        public TaskPage(TaskViewModel selectedTask)
        {
            InitializeComponent();
            SubscribingInformation();

            ViewModel = new TaskPageViewModel();
            BindingContext = ViewModel;

            Title = selectedTask.TaskInfo.TaskTypeDescription;

            TaskDetails.Children.Add(new TaskDetailsView(selectedTask));
            TaskTest.Children.Add(new TaskTestView(selectedTask));
            TaskTimeline.Children.Add(new TaskTimelineView(selectedTask));
        }

        /// <summary>
        /// This is for handling the case when segControl value has changed
        /// </summary>
        /// <param name="sender">the object triggered this event</param>
        /// <param name="e">the event containing new value</param>
        private void SegControl_ValueChanged(object sender, SegmentedControl.FormsPlugin.Abstractions.ValueChangedEventArgs e)
        {
            int newVal = e.NewValue;

            ViewModel.SegControlCommand.Execute(newVal);
        }

        /// <summary>
        /// This is used to subscribe messages
        /// </summary>
        private void SubscribingInformation()
        {
            MessagingCenter.Subscribe<TaskDetailsViewModel>(this, "TaskInfoUpdateFailed", async (sender) =>
            {
                await DisplayAlert("Alert", "Failed to update the task information, please try again later", "OK");
            });
        }
    }
}