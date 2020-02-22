using System;
using System.Collections.Generic;
using NUnit.Framework;
using SWEN90013.Enums;
using SWEN90013.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;


namespace Tests
{
    [TestFixture]
    public class ServiceVisitCalendarViewModelTest
    {
        private ServiceVisitCalendarViewModel visitCalendarViewModel;
        List<ServiceVisitViewModel> visits;

        public ServiceVisitCalendarViewModelTest()
        {
        }

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
                new ServiceVisitViewModel()
                {
                    Id = 2,
                    SiteName = "Old Engineering",
                    ScheduledDate = new DateTime(2019,7,31),
                    ServiceVisitNumber = 10003195,
                    Status = ServiceVisitStatus.Ready,
                    SiteSuburb = "Parkville",
                    SiteAddressLine1 = "184",
                    SiteAddressLine2 = "The University of Melbourne",
                    SiteAddressLine3 = "Monash Rd",
                    SiteAddressPostCode = 3052,
                    DueDate = new DateTime(2019,07,31),
                    TotalTasks = 4
                },
                new ServiceVisitViewModel()
                {
                    Id = 3,
                    SiteName = "The Stables",
                    ServiceVisitNumber = 10004821,
                    ScheduledDate = new DateTime(2019,5,14),
                    Status = ServiceVisitStatus.InProgress,
                    SiteSuburb = "Southbank",
                    SiteAddressLine1 = "234",
                    SiteAddressLine2 = "",
                    SiteAddressLine3 = "Southbank",
                    SiteAddressPostCode = 3006,
                    DueDate = new DateTime(2019,05,31),
                    TotalTasks = 4
                },
                new ServiceVisitViewModel()
                {
                    Id = 4,
                    SiteName = "Melbourne Theatre Company",
                    ServiceVisitNumber = 10008783,
                    Status = ServiceVisitStatus.Completed,
                    SiteSuburb = "Southbank",
                    DueDate = new DateTime(2019,05,31),
                    SiteAddressLine1 = "234",
                    SiteAddressLine2 = "St Kilda Rd",
                    SiteAddressLine3 = "Southbank",
                    SiteAddressPostCode = 3006,
                    TotalTasks = 4
                },
            };
            visitCalendarViewModel = new ServiceVisitCalendarViewModel(visits);
        }


        [Test]
        public void MoveToNextFiveDaysTest()
        {
            //Arrange
            visitCalendarViewModel.SetDate(DateTime.Today);

            //Act
            visitCalendarViewModel.MoveToNextFiveDay();

            //Assert
            Boolean moveForwardByFiveDays = DateTime.Today.AddDays(5).Equals(visitCalendarViewModel.StartDate);
            Assert.IsTrue(moveForwardByFiveDays);

        }

        [Test]
        public void MoveToLastFiveDaysTest()
        {
            //Arrange
            visitCalendarViewModel.SetDate(DateTime.Today);

            //Act
            visitCalendarViewModel.MoveToLastFiveDay();

            //Assert
            Boolean moveBackwardByFiveDays = DateTime.Today.AddDays(-5).Equals(visitCalendarViewModel.StartDate);
            Assert.IsTrue(moveBackwardByFiveDays);

        }

        [Test]
        public void UpdateDate()
        {
            //arrange
            visitCalendarViewModel.SetDate(DateTime.Today);

            //Act
            visitCalendarViewModel.SetDate(DateTime.Today.AddDays(1));

            //Assert
            Boolean updateDate = DateTime.Today.AddDays(1).Equals(visitCalendarViewModel.StartDate);
            Assert.IsTrue(updateDate);

        }


        [Test]
        public void DeteleSchedule()
        {
            //Act
            visitCalendarViewModel.DeleteSchedule(visitCalendarViewModel.FullVisitsList[2]);

            //Assert
            Boolean deleteSchedule = visitCalendarViewModel.FullVisitsList[2].ScheduledDateDescription.Equals("Add to calendar");
            Assert.IsTrue(deleteSchedule);

        }

    }
}
