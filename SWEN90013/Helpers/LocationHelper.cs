using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SWEN90013.Helpers
{
    public class LocationHelper
    {
        public LocationHelper()
        {
        }

        public static double getDistanceFromLatLonInKmd(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            Location location1 = new Location();
            location1.Latitude = latitude1;
            location1.Longitude = longitude1;

            Location location2 = new Location();
            location2.Latitude = latitude2; 
            location2.Longitude = longitude2;

            return Location.CalculateDistance(location1, location2,DistanceUnits.Kilometers);
        }

        public static double DegreeToRadian(double degree)
        {
            return degree * (Math.PI / 180);
        }

        public static async Task<Location> CurrentPositionAsync()
        {
            Location location = null;
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.High);
                location = await Geolocation.GetLocationAsync(request);
            }
            catch (FeatureNotEnabledException)
            {
                location = new Location();
            }
            catch (Exception)
            {
                location = new Location();
            }

            return location;
        }
    }
}
