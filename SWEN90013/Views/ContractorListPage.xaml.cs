using SWEN90013.ViewModels;
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
    public partial class ContractorListPage : ContentPage
    {
        public ContractorListViewModel vm;
        public ContractorListPage()
        {
            InitializeComponent();
            SubscribingInformation();

            vm = new ContractorListViewModel();
            BindingContext = vm;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            String targetCompngy = e.Item.ToString();

            MessagingCenter.Send<ContractorListPage, String>(this, "Contractor", targetCompngy);
            Navigation.PopAsync();
        }

        private void AddContractor_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddContractorPage(vm));
        }

        private void SubscribingInformation()
        {
            MessagingCenter.Subscribe<AddContractorViewModel, String>(this, "AddContractor", (sender, arg) =>
            {
                String newContractor = arg;

                vm.addNewContractor(newContractor);
            });

            MessagingCenter.Subscribe<ContractorListViewModel>(this, "GetContractorListFailed", async (sender) =>
            {
                await DisplayAlert("Alert", "Getting contractor list failed, please try again later!", "OK");
            });
        }
    }
}