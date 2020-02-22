using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SWEN90013.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using SWEN90013.ViewModels;
using Plugin.SecureStorage;
using Plugin.Media.Abstractions;

namespace SWEN90013.ServicesHandler
{
    public class MediaServices
    {
        HttpClient _httpClient = new HttpClient();
        
        public MediaServices()
        {
        }

        public async Task<string> UploadImage(MediaFile NewImage)
        {
            //TODO call this when the endpoint implementation is done
            String url = Configuration.baseURL + "/images";
            var content = new MultipartFormDataContent();
            content.Headers.ContentType.MediaType = "multipart/form-data";
            content.Add(new StreamContent(NewImage.GetStream()), "file", NewImage.Path);

			//set token
			String token = CrossSecureStorage.Current.GetValue("Token");
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

			String newUrl = null;

            try
            {
                var response = await _httpClient.PutAsync(url, content);
                newUrl = await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("Upload image failed from MediaServices.cs. Error message: " + e.Message);
            }

            return newUrl;
        }
    }
}
