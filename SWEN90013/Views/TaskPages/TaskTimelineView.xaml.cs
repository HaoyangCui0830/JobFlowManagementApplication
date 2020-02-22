using SWEN90013.ViewModels;
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


    public partial class TaskTimelineView : ContentView
    {
        /// <summary>
        /// This is the constructor for Timeline of Task View
     
        public TaskTimelineViewModel ViewModel;

        void Handle_ItemAppearing(object sender, Xamarin.Forms.ItemVisibilityEventArgs e)
        {
            if (e.ItemIndex == ViewModel.GetLastIndex())
            {
                ViewModel.LoadItems();
            }
        }

        public TaskTimelineView(TaskViewModel targetTask)
        {
            InitializeComponent();
            ViewModel = new TaskTimelineViewModel(targetTask.ServiceVisitItemNumber);

            BindingContext = ViewModel;
        }

    }
}