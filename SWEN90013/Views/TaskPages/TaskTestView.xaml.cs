using SWEN90013.ViewModels;
using SWEN90013.ViewModels.TaskChecklist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SWEN90013.Views.TaskPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskTestView : ScrollView
    {
        TestListPageViewModel viewModel = new TestListPageViewModel();
        /// <summary>
        /// This is the constructor for Test of Task View
        /// </summary>
        /// <param name="targetTask">The Task currenty showing in this view</param>
        public TaskTestView(TaskViewModel targetTask)
        {
            viewModel = new TestListPageViewModel(targetTask.Checklists, targetTask.Comments, targetTask.LastResult, targetTask.ThisResult, targetTask.ServiceVisitId, targetTask.ServiceVisitItemNumber);
            InitializeComponent();
            BindingContext = viewModel;
            SubscribingInformation();
        }

        /// <summary>
        /// This methods subscribed the page to required events
        /// 1. Add/Update a check item comment
        /// 2. Add a new task comment
        /// </summary>
        private void SubscribingInformation()
        {
            MessagingCenter.Subscribe<CheckItemCommentPageViewModel, Tuple<int,CommentViewModel>>(this, "UpdateCheckItemComment", (sender, arg) => {
                viewModel.UpdateCheckItemComment(arg.Item1, arg.Item2);
            });

            MessagingCenter.Subscribe<TestCommentPageViewModel, CommentViewModel>(this, "AddTestComment", (sender, arg) => {
                viewModel.AddTestComment(arg);
            });

        }
    }
}