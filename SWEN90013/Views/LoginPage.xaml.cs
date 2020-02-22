using System;
using System.Collections.Generic;
using System.Net.Http;
using FES;
using Xamarin.Forms;
using SWEN90013.ViewModels;

namespace SWEN90013.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPageViewModel loginViewModel;
      
        public LoginPage()
        {
            InitializeComponent();
            loginViewModel = new LoginPageViewModel();
            MessagingCenter.Subscribe<LoginPageViewModel, string>(this, "loginSuccessNotify", (sender, username)=>
            {
                Navigation.PushAsync(new ServiceVisitListMasterPage(), false);
            });
            this.BindingContext = loginViewModel;
            MessagingCenter.Subscribe<LoginPageViewModel, string>(this, "nullUsernameorPassword", async (sender, username) =>
                //Create Alert
                await DisplayAlert("Alert", "Please fill the username and password.", "OK"));
            MessagingCenter.Subscribe<LoginPageViewModel, string>(this, "wrongUsernameorPassword", async (sender, username) =>
                //Create Alert
                await DisplayAlert("Wrong password", "Sorry, this is wrong password for your account.", "OK"));
        }        


        
    }
}
