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
    public partial class TaskDetailsView : ScrollView
    {
        TaskDetailsViewModel vm;
        /// <summary>
        /// This is the constructor for Details of Task View
        /// </summary>
        /// <param name="targetTask">The Task currenty showing in this view</param>
        public TaskDetailsView(TaskViewModel targetTask)
        {
            InitializeComponent();
            SubscribingInformation();

            vm = new TaskDetailsViewModel(targetTask);
            BindingContext = vm;
            FrameClass.BorderColor = Color.FromRgba(0, 0, 0, 0.1);
        }

        private void SubscribingInformation()
        {
            MessagingCenter.Subscribe<ContractorListPage, String>(this, "Contractor", (sender, arg) =>
            {
                String chosenCompany = arg;

                vm.SetANewContractor(chosenCompany);
            });
        }

        private async void Contractor_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ContractorListPage());
        }
    }
}