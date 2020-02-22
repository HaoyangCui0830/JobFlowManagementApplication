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
    public partial class AddContractorPage : ContentPage
    {
        public AddContractorViewModel vm;
        public AddContractorPage(ContractorListViewModel contractorListVM)
        {
            InitializeComponent();
            vm = new AddContractorViewModel();
            vm.ExistContractorList = contractorListVM.FullCompanyList;
            vm.Navigation = Navigation;

            BindingContext = vm;

            MessagingCenter.Subscribe<AddContractorViewModel>(this, "emptyContractor", async (sender) =>
            {
                await DisplayAlert("Alert", "Please input a non-empty contractor name", "OK");
            });
            MessagingCenter.Subscribe<AddContractorViewModel>(this, "duplicateContractor", async (sender) =>
            {
                await DisplayAlert("Alert", "This contractor name already exist in the list", "OK");
            });
            MessagingCenter.Subscribe<AddContractorViewModel>(this, "AddContractorFailed", async (sender) =>
             {
                 await DisplayAlert("Alert", "Failed to add this contractor, please try again later", "OK");
             });
        }
    }
}