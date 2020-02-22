using System;
using System.Net;
//using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Xamarin.Forms.Maps;

namespace SWEN90013.ServicesHandler.GeoLocationHandler
{
    public class GeoLocationInfoHelper
    {
        public GeoLocationInfoHelper()
        {

            

        }

        public Position GetCoordinates(string region)
        {
            using (var client = new WebClient())
            {
            	string APIkey = "";
                string uri = "https://maps.googleapis.com/maps/api/geocode/json?address=" + region + "&key=" + APIkey;

                string geocodeInfo = client.DownloadString(uri);
                //JavaScriptSerializer oJS = new JavaScriptSerializer();
                //RootObject latlongdata = oJS.Deserialize<RootObject>(geocodeInfo);
                RootObject son = new RootObject();
                RootObject latlongdata = JsonConvert.DeserializeAnonymousType<RootObject>(geocodeInfo, son);
                return new Position(Convert.ToDouble(latlongdata.results[0].geometry.location.lat), Convert.ToDouble(latlongdata.results[0].geometry.location.lng));

            }
        }
    }
}

