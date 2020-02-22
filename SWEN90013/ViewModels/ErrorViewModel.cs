using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SWEN90013.ViewModels
{
    public class ErrorViewModel
    {
        #region Constructors
        public ErrorViewModel(Page page)
        {
            RetryPreviousPageCommand = new Command(RetryPreviousPage);
            Page = page;
        }

        #endregion

        #region Properties
        //page that we want to reload
        public Page Page { get; set; }
        #endregion

        #region Commands
        public ICommand RetryPreviousPageCommand { get; set; }
        #endregion

        #region Methods
        public async void RetryPreviousPage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(Page);
        }
        #endregion
    }
}
