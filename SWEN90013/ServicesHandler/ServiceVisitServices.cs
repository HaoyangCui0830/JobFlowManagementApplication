using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Media.Abstractions;
using Plugin.SecureStorage;
using SWEN90013.Helpers;
using SWEN90013.Models;
using Xamarin.Essentials;

namespace SWEN90013.ServicesHandler
{
    public class ServiceVisitServices
    {
        HttpClient _httpClient = new HttpClient();


        //GET visits for a technician
        public async Task<List<ServiceVisit>> GetServiceVisitListsForUser(String user)
        {
            String url = Configuration.baseURL + "/ServiceVisits/" + user;
            try
            {

                //set token
                String token = CrossSecureStorage.Current.GetValue("Token");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    String json = await _httpClient.GetStringAsync(url);
                    //convert json to model
                    return JsonConvert.DeserializeObject<List<ServiceVisit>>(json);
                }
                else
                {
                    var serviceVisitList =await ServiceVisitDBHelper.GetAllServiceVisits();
                    return serviceVisitList;
                } 

            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// This is used to update details of building
        /// </summary>
        /// <param name="SiteId">Unique id for each site</param>
        /// <param name="BuildingDetails">New Building details</param>
        /// <returns></returns>
        public async Task<bool> UpdateBuildingDetails(int SiteId, Building BuildingDetails)
        {
            String url = Configuration.baseURL + "/site/" + SiteId + "/building";
            var json = JsonConvert.SerializeObject(BuildingDetails);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            //set token
            String token = CrossSecureStorage.Current.GetValue("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)

            {
                var respond = await _httpClient.PutAsync(url, content);

                if (respond.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                var result = await ServiceVisitDBHelper.GetServiceVisitBySiteId(SiteId);
                if (result != null)
                {
                    ServiceVisit serviceVisit = (ServiceVisit)(result);
                    await ServiceVisitDBHelper.SaveServiceVisitBuilding(serviceVisit, BuildingDetails);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// This is used to update Memos
        /// </summary>
        /// <param name="SiteId">Unique Id for site, used to target ContactMemo,InductionMemo and OHCMemo</param>
        /// <param name="ServiceVisitNumber">Unique Id for service visit, used to target ServiceMemo</param>
        /// <param name="Memo">All the new Memos</param>
        /// <returns></returns>
        public async Task<bool> UpdateMemo(int SiteId, int ServiceVisitNumber, Memos Memo)
        {
            String url = Configuration.baseURL + "/site/" + SiteId + "/memos";
            String urlForServiceMemo = Configuration.baseURL + "/ServiceVisits/" + ServiceVisitNumber + "/servicememo";

            var json = JsonConvert.SerializeObject(Memo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var json2 = JsonConvert.SerializeObject(Memo);
            var content2 = new StringContent(json, Encoding.UTF8, "application/json");

            //set token
            String token = CrossSecureStorage.Current.GetValue("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                var request1 = _httpClient.PutAsync(url, content);
                var request2 = _httpClient.PutAsync(urlForServiceMemo, content2);

                await Task.WhenAll(request1, request2);

                var respond1 = await request1;
                var respond2 = await request2;

                if (respond1.StatusCode == System.Net.HttpStatusCode.OK
                    && respond2.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                var result = await ServiceVisitDBHelper.GetServiceVisit(ServiceVisitNumber);
                if (result != null)
                {
                    ServiceVisit serviceVisit = (ServiceVisit)(result);
                    await ServiceVisitDBHelper.SaveServiceVisitMemo(serviceVisit, Memo);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
            
        public async Task<bool> DeleteServiceVisitSchedule(String ServiceVisitId)
        {
            String url = Configuration.baseURL + "/ServiceVisits/" + ServiceVisitId + "/schedule";
            try
            {
                //set token
                String token = CrossSecureStorage.Current.GetValue("Token");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    HttpResponseMessage response = await _httpClient.DeleteAsync(url);
                    return true;
                }
                else
                {
                    var result = await ServiceVisitDBHelper.GetServiceVisit(Int32.Parse(ServiceVisitId));
                    if (result != null)
                    {
                        ServiceVisit serviceVisit = (ServiceVisit)(result);
                        await ServiceVisitDBHelper.SaveServiceVisitDeleteSchedule((ServiceVisit)(serviceVisit));
                    }
                    return true;
                }

                //convert json to model
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public async void RescheduleServiceVisitSchedule(String ServiceVisitId, DateTime dateTime)
        {
            String url = Configuration.baseURL + "/ServiceVisits/" + ServiceVisitId + "/schedule";
            try
            {

                //set token
                String token = CrossSecureStorage.Current.GetValue("Token");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    //JObject content = new JObject();

                    //content.Add("DateTime", "{\"DateTime\":\"" + String.Format("{0:s}", dateTime) + "\"}");
                    var content = new StringContent("{\"DateTime\":\"" + String.Format("{0:s}", dateTime) + "\"}", Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await _httpClient.PutAsync(url, content);
                    //convert json to model
                }
                else
                {
                    var result = await ServiceVisitDBHelper.GetServiceVisit(Int32.Parse(ServiceVisitId));
                    if (result != null)
                    {
                        ServiceVisit serviceVisit = (ServiceVisit)(result);
                        await ServiceVisitDBHelper.SaveServiceVisitReschedule((ServiceVisit)(serviceVisit), dateTime);
                    }
                    
                }

            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// This is used to update a new site image
        /// </summary>
        /// <param name="SiteId">Unique Id for site</param>
        /// <param name="NewImage">The new Image waiting to be uploaded</param>
        /// <returns></returns>
        public async Task<string> UpdateSiteImage(int SiteId, MediaFile NewImage)
        {
            String url = Configuration.baseURL + "/site/" + SiteId + "/image";
            //MultipartFormDataContent form = new MultipartFormDataContent();

            //form.Add(new StreamContent(NewImage.GetStream()), "file", NewImage.Path);
            //form.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            var content = new MultipartFormDataContent();
            content.Headers.ContentType.MediaType = "multipart/form-data";
            content.Add(new StreamContent(NewImage.GetStream()), "file", NewImage.Path);

            String newUrl = null;

            try
            {
                //set token
                String token = CrossSecureStorage.Current.GetValue("Token");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PutAsync(url, content);
                newUrl = await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {

            }

            return newUrl;
        }

        public async Task<bool> SubmitServiceVisit(String ServiceVisitId, ServiceVisitSubmission submissionObject)
        {
            String url = Configuration.baseURL + "/ServiceVisits/" + ServiceVisitId + "/submit";
            var json = JsonConvert.SerializeObject(submissionObject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                //set token
                String token = CrossSecureStorage.Current.GetValue("Token");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var current = Connectivity.NetworkAccess;
				if (current == NetworkAccess.Internet) {
					var respond = await _httpClient.PutAsync(url, content);

					if (respond.StatusCode == System.Net.HttpStatusCode.OK) {
						return true;
					} else {
						return false;
					}
				} else {
					return false;
				}
			}
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}
