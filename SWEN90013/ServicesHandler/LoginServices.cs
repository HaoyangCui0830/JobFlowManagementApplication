using System;
using System.Net.Http;
using Xamarin.Forms;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.SecureStorage;
using SWEN90013.Models;
using System.Text;

namespace SWEN90013.ServicesHandler
{
    public class LoginServices
    {

        private string url = "";
        HttpClient _httpClient = new HttpClient();

        //GET visits for a technician
        public async Task Login(string username, string userPwd)
        {

            //set the url
            url = Configuration.baseURL + "/user/auth";

            Login loginModel = new Login(username, userPwd);

            var json = JsonConvert.SerializeObject(loginModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = _httpClient.PostAsync(url, content);
            try
            {
                var respond = await request;
                var contents = await respond.Content.ReadAsStringAsync();

                if (respond.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    User user = JsonConvert.DeserializeObject<User>(contents);
                    CrossSecureStorage.Current.SetValue("LoginStatus", "true");
                    CrossSecureStorage.Current.SetValue("Token", user.token);
                    CrossSecureStorage.Current.SetValue("UserID", user.userID.ToString());
                    CrossSecureStorage.Current.SetValue("UserName", user.userName);
					CrossSecureStorage.Current.SetValue("UserCode", user.userCode);

                }
                else
                {
                    CrossSecureStorage.Current.SetValue("LoginStatus", "false");
                }
            }
            catch (Exception)
            {
                CrossSecureStorage.Current.SetValue("LoginStatus", "false");
            }




        }


    }
}

