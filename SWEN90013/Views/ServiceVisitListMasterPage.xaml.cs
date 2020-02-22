using System;
using System.Collections.Generic;
using SWEN90013.Models;
using Xamarin.Forms;
using Plugin.SecureStorage;

namespace SWEN90013.Views
{
    public partial class ServiceVisitListMasterPage : MasterDetailPage
    {
        public ServiceVisitListMasterPage()
        {
            InitializeComponent();

            Detail = new NavigationPage(new ServiceVisitListPage());

            //get current user name and display it on the menu
            UsernameLabel.Text = CrossSecureStorage.Current.GetValue("UserName");
        }

        //Method to be called when logout button is called
        async void Logout_Clicked(object sender, System.EventArgs e)
        {
            //TODO cleanup logged user information

            //redirect back to login page
            await Navigation.PushAsync(new LoginPage(), false);
        }
    }
}
