using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SWEN90013.ViewModels;
using Xamarin.Forms;

namespace SWEN90013.Views
{
    public partial class TaskReviewSubmissionPage : ContentPage
    {
        public TaskReviewSubmissionViewModel viewModel;
        private List<TaskViewModel> _taskList;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fullUndoneTasksoTasks"></param>
        /// <param name="fullDoneTasks"></param>
        public TaskReviewSubmissionPage(List<TaskViewModel> fullUndoneTasksoTasks, List<TaskViewModel> fullDoneTasks, int serviceVisitID)
        {
            InitializeComponent();

            viewModel = new TaskReviewSubmissionViewModel(fullUndoneTasksoTasks, fullDoneTasks, serviceVisitID)
            {
                Navigation = Navigation
            };

            BindingContext = viewModel;

            MessagingCenter.Subscribe<TaskReviewSubmissionViewModel>(this, "submitTask", (sender) =>
            {
				Navigation.PushAsync(new ServiceVisitListMasterPage(), false);
            });
            
            MessagingCenter.Subscribe<TaskReviewSubmissionViewModel>(this, "needInspection", (sender) =>
            {
                DisplayAlert("Alert", "Please submit after you have the technician inspection clicked.", "OK");
            });

			MessagingCenter.Subscribe<TaskReviewSubmissionViewModel>(this, "error", (sender) => {
				DisplayAlert("Alert", "Something went wrong during submission. Please try again", "OK");
			});
		}

    }
}
