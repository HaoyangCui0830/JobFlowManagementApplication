using System.Collections.Generic;
using Xamarin.Forms.Maps;
namespace SWEN90013.CustomComponents.MapTools
{
    // A component to set the Map
    public class ServiceVisitMap : Map
    {
        public List<ServiceVisitMapPin> serviceVisitMapPins { get; set; }
    }
}

