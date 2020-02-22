using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin.SecureStorage;
using SWEN90013.Views;
using FES;

namespace SWEN90013.ViewModels
{
    public class LoginPageViewModel 
    {
        
        //username
        public string Username { get; set; }
        //password
        public string Password { get; set; }
        // ICommand
        public ICommand SubmitCommand { get; set; }

        public LoginPageViewModel()
        {
            SubmitCommand = new Command(OnSubmit);
        }

       

        public async void OnSubmit()
        {
             if(String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(Password))
            {
                //Create Alert
                MessagingCenter.Send(this, "nullUsernameorPassword", Username);
            }
            else
            {
                //login
                ServicesHandler.LoginServices loginService = new ServicesHandler.LoginServices();
                var username = Username;
                var pwd = Password;
                // login Task 
                await loginService.Login(username, pwd);
                var loginResult = CrossSecureStorage.Current.GetValue("LoginStatus");

                if (loginResult.Equals("true"))
                {

                    //login  successfully
                    MessagingCenter.Send(this, "loginSuccessNotify", Username);

                }
                else
                {
                    //Create Alert
                    MessagingCenter.Send(this, "wrongUsernameorPassword", Username);

                }
            }
        

            
        }
    }
}
