using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using SWEN90013.CustomComponents.MapTools;
using Xamarin.Essentials;
using SWEN90013.ViewModels;
using SWEN90013.Views.ServiceVisitPages;


namespace SWEN90013.Views
{
    public partial class ServiceVisitMapPage : ContentPage
    {


        private List<ServiceVisitMapPin> ServiceVisitMapPinsList = new List<ServiceVisitMapPin>();


        public ServiceVisitMapPage(ServiceVisitMapViewModel visit)
        {
            InitializeComponent();

            // Build Pins in Map
            visit.FullVisitsList.ForEach((item) =>
            {
                ServiceVisitMapPinsList.Add(
                    new ServiceVisitMapPin()
                    {
                    Type = PinType.Place,
                    Position = new Position(item.GetCoordinateInfo.Latitude,item.GetCoordinateInfo.Longitude),
                    Label = item.SiteName,
                    Address = item.SiteSuburb,
                    Id = item.Id.ToString(),
                    }
                    );

            });


            AddPinClickableEvent(visit);

            // TODO Add algorithm to decide the best initial Map view location
            serviceVisitMap.MoveToRegion(MapSpan.FromCenterAndRadius(ServiceVisitMapPinsList[0].Position, Distance.FromMiles(1.0)));

        }

        //Add Pin Clickable Event
        public void AddPinClickableEvent(ServiceVisitMapViewModel visit)
        {

            serviceVisitMap.MapType = MapType.Street;
            for (var i = 0; i < ServiceVisitMapPinsList.ToArray().Length; i++)
            {
                serviceVisitMap.serviceVisitMapPins = new List<ServiceVisitMapPin> { ServiceVisitMapPinsList[i] };
                serviceVisitMap.Pins.Add(ServiceVisitMapPinsList[i]);
                ServiceVisitMapPinsList[i].Clicked += (sender, e) => {
                    visit.FullVisitsList.ForEach( (item) =>
                    {
                        if (((ServiceVisitMapPin)sender).Label.Equals(item.SiteName))
                        {
                            Navigation.PushAsync(new ServiceVisitPage((ServiceVisitViewModel)item));
                        }

                    });
                };
            }
        }
    }
}

