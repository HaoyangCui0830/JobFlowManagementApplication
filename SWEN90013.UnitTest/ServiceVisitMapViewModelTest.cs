using System;
using System.Collections.Generic;
using NUnit.Framework;
using SWEN90013.Enums;
using SWEN90013.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms.Maps;
using Xamarin.Essentials;

namespace Tests
{
    [TestFixture]
    public class ServiceVisitMapViewModelTest
    {
        private ServiceVisitMapViewModel visitMapViewModel;
        List<ServiceVisitViewModel> visits;


        [SetUp]
        public void Setup()
        {
            visits = new List<ServiceVisitViewModel>()
            {
                new ServiceVisitViewModel()
                {
                    Id = 1,
                    SiteName = "Alice Hoy",
                    ServiceVisitNumber = 10003193,
                    Status = ServiceVisitStatus.Ready,
                    DueDate = new DateTime(2019,07,31),
                    SiteSuburb = "Parkville",
                    SiteAddressLine1 = "162",
                    SiteAddressLine2 = "The University of Melbourne",
                    SiteAddressLine3 = "Monash Rd",
                    SiteAddressPostCode = 3052,
                    TotalTasks = 4
                },
            };
            visitMapViewModel = new ServiceVisitMapViewModel(visits);

        }


        [Test]
        public void GetCoordinateCloseEnough()
        {
            Position getCoordiate = visitMapViewModel.FullVisitsList[0].GetCoordinateInfo;
            Location originCoordinate = new Location(-37.798349, 144.963483);
            double distance = Location.CalculateDistance(getCoordiate.Latitude,
                getCoordiate.Longitude, originCoordinate, DistanceUnits.Kilometers);
            Console.WriteLine(distance);
            Boolean roughlyEqual = distance < 0.25;
            Assert.IsTrue(roughlyEqual);
        }

    }
}
