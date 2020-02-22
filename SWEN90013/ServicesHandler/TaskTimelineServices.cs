using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.SecureStorage;
using SWEN90013.Models;

namespace SWEN90013.ServicesHandler
{
    public class TaskTimelineServices
    {
        HttpClient _httpClient = new HttpClient();

        public TaskTimelineServices()
        {
        }

        public async Task<List<Timeline>> GetTimelineList(String taskId,String startDate, String endDate)
        {
            //get request (example for now)
            String url = Configuration.baseURL + "/tasks/" + taskId + "/" + startDate + "/" + endDate + "/timeline";
            try
            {

                //set token
                String token = CrossSecureStorage.Current.GetValue("Token");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                String json = await _httpClient.GetStringAsync(url);
                //convert json to model
                return JsonConvert.DeserializeObject<List<Timeline>>(json);
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
