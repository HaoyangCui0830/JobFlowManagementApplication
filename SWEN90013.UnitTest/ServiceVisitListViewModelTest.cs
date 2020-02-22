using System;
using System.Collections.Generic;
using NUnit.Framework;
using SWEN90013.Enums;
using SWEN90013.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using SWEN90013.ViewModels.ServiceVisitSort;

namespace Tests
{
    [TestFixture]
    public class ServiceVisitListViewModelTest
    {
        //Global variables
        ServiceVisitListViewModel visitListViewModel;
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
                    SiteAddressLine1 = "162",
                    SiteAddressLine2 = "The University of Melbourne",
                    SiteAddressLine3 = "Monash Rd",
                    SiteAddressPostCode = 3052,
                    SiteSuburb = "Parkville",
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
                    TotalTasks = 2
                },
                new ServiceVisitViewModel()
                {
                    Id = 3,
                    SiteName = "The Stables",
                    ServiceVisitNumber = 10004821,
                    ScheduledDate = new DateTime(2019,5,14),
                    Status = ServiceVisitStatus.InProgress,
                    SiteSuburb = "Southbank",
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
                    SiteAddressLine1 = "234",
                    SiteAddressLine2 = "St Kilda Rd",
                    SiteAddressLine3 = "Southbank",
                    SiteAddressPostCode = 3006,
                    DueDate = new DateTime(2019,05,31),
                    TotalTasks = 1
                },
            };
            visitListViewModel = new ServiceVisitListViewModel(visits);
        }

        [Test]
        public void SearchForId_ServiceVisits_ItemsNarrowedDown()
        {
            //Arrange
            string searchedTerm = "319";

            //Act
            visitListViewModel.SearchedTerm = searchedTerm;

            //Assert
            bool visitsSearched = (visitListViewModel.Visits.Count > 0) &&
                                  visitListViewModel.Visits.All(v => v.ServiceVisitNumber.ToString().Contains(searchedTerm));
            Assert.IsTrue(visitsSearched);
        }

        [Test]
        public void SearchForSuburb_ServiceVisits_ItemsNarrowedDown()
        {
            //Arrange
            var searchedTerm = "Southbank";

            //Act
            visitListViewModel.SearchedTerm = searchedTerm;

            //Assert
            bool visitsSearched = (visitListViewModel.Visits.Count > 0) &&
                                  visitListViewModel.Visits.All(v => v.SiteSuburb.Contains(searchedTerm));
            Assert.IsTrue(visitsSearched);
        }

        [Test]
        public void SearchForAddress_ServiceVisits_ItemsNarrowedDown()
        {
            //Arrange
            var searchedTerm = "Monash Rd";

            //Act
            visitListViewModel.SearchedTerm = searchedTerm;

            //Assert
            bool visitsSearched = (visitListViewModel.Visits.Count > 0) &&
                                  visitListViewModel.Visits.All(v => v.FullAddress.Contains(searchedTerm));
            Assert.IsTrue(visitsSearched);
        }

        [Test]
        public void SearchForSiteName_ServiceVisits_ItemsNarrowedDown()
        {
            //Arrange
            var searchedTerm = "Alice Hoy";

            //Act
            visitListViewModel.SearchedTerm = searchedTerm;

            //Assert
            bool visitsSearched = (visitListViewModel.Visits.Count > 0) &&
                                  visitListViewModel.Visits.All(v => v.SiteName.Contains(searchedTerm));
            Assert.IsTrue(visitsSearched);
        }

        [Test]
        public void FilterByStatusTest()
        {
            //Arrange
            ServiceVisitFilterViewModel vm = new ServiceVisitFilterViewModel(visitListViewModel.FullVisitsList);

            //Act
            vm.SelectedStatus = ServiceVisitStatus.Ready;
            visitListViewModel.UpdateVisitsBasedOnFilter(vm);

            //Assert
            bool visitsFiltered = (visitListViewModel.Visits.All(v => v.Status.Equals(ServiceVisitStatus.Ready)));
            Assert.IsTrue(visitsFiltered);
        }

        [Test]
        public void FilterBySuburbTest()
        {
            //Arrange
            ServiceVisitFilterViewModel vm = new ServiceVisitFilterViewModel(visitListViewModel.FullVisitsList);
            var suburbs = new List<String>();
            suburbs.Add("Parkville");

            //Act
            vm.SelectedSuburbs = suburbs;
            visitListViewModel.UpdateVisitsBasedOnFilter(vm);

            //Assert
            bool visitsFiltered = (visitListViewModel.Visits.All(v => v.SiteSuburb.Equals("Parkville")));
            Assert.IsTrue(visitsFiltered);
        }

        [Test]
        public void FilterByUnscheduleTest()
        {
            //Arrange
            ServiceVisitFilterViewModel vm = new ServiceVisitFilterViewModel(visitListViewModel.FullVisitsList);

            //Act
            vm.SelectedSchedule = ServiceVisitSchedule.Unscheduled;
            visitListViewModel.UpdateVisitsBasedOnFilter(vm);

            //Assert
            bool visitsFiltered = (visitListViewModel.Visits.All(v => v.ScheduledDate.Equals(null)));
            Assert.IsTrue(visitsFiltered);
        }

        [Test]
        public void FilterByTodayScheduleTest()
        {
            //Arrange
            ServiceVisitFilterViewModel vm = new ServiceVisitFilterViewModel(visitListViewModel.FullVisitsList);

            //Act
            vm.SelectedSchedule = ServiceVisitSchedule.TodayScheduled;
            visitListViewModel.UpdateVisitsBasedOnFilter(vm);

            //Assert
            Boolean isScheduleMatch = true;
            foreach(ServiceVisitViewModel visit in visitListViewModel.Visits)
            {
                if(!ServiceVisitScheduleMethods.IsTodayScheduled((System.DateTime)visit.ScheduledDate))
                {
                    isScheduleMatch = false;
                }
            }
            Assert.IsTrue(isScheduleMatch);
        }

        [Test]
        public void FilterByWeeklyScheduleTest()
        {
            //Arrange
            ServiceVisitFilterViewModel vm = new ServiceVisitFilterViewModel(visitListViewModel.FullVisitsList);

            //Act
            vm.SelectedSchedule = ServiceVisitSchedule.WeeklyScheduled;
            visitListViewModel.UpdateVisitsBasedOnFilter(vm);

            //Assert
            Boolean isScheduleMatch = true;
            foreach (ServiceVisitViewModel visit in visitListViewModel.Visits)
            {
                if (!ServiceVisitScheduleMethods.IsTodayScheduled((System.DateTime)visit.ScheduledDate))
                {
                    isScheduleMatch = false;
                }
            }
            Assert.IsTrue(isScheduleMatch);
        }

        [Test]
        public void FilterByMonthlyScheduleTest()
        {
            //Arrange
            ServiceVisitFilterViewModel vm = new ServiceVisitFilterViewModel(visitListViewModel.FullVisitsList);

            //Act
            vm.SelectedSchedule = ServiceVisitSchedule.MonthlyScheduled;
            visitListViewModel.UpdateVisitsBasedOnFilter(vm);

            //Assert
            Boolean isScheduleMatch = true;
            foreach (ServiceVisitViewModel visit in visitListViewModel.Visits)
            {
                if (!ServiceVisitScheduleMethods.IsMonthlyScheduled((System.DateTime)visit.ScheduledDate))
                {
                    isScheduleMatch = false;
                }
            }
            Assert.IsTrue(isScheduleMatch);
        }

        [Test]
        public void FilterByAllDueDate()
        {
            //Arrange
            ServiceVisitFilterViewModel vm = new ServiceVisitFilterViewModel(visitListViewModel.FullVisitsList);

            //Act
            vm.SelectAllDueDate();
            visitListViewModel.UpdateVisitsBasedOnFilter(vm);

            //Assert
            Assert.IsTrue(visitListViewModel.Visits.Count.Equals(4));
        }

        [Test]
        public void FilterByThisMonthDueDate()
        {
            //Arrange
            ServiceVisitFilterViewModel vm = new ServiceVisitFilterViewModel(visitListViewModel.FullVisitsList);

            //Act
            vm.SelectThisMonthDueDate();
            visitListViewModel.UpdateVisitsBasedOnFilter(vm);

            //Assert
            Boolean isDueDateMatch = true;
            var thisMonth = DateTime.Now.Month;
            foreach (ServiceVisitViewModel visit in visitListViewModel.Visits)
            {
                if (!visit.DueDate.Month.Equals(thisMonth)){
                    isDueDateMatch = false;
                }
            }
            Assert.IsTrue(isDueDateMatch);
        }

        [Test]
        public void SortBySuburbAsc()
        {
            //Arrange
            ServiceVisitSortListViewModel vm = new ServiceVisitSortListViewModel();

            //Act
            vm.SelectedSortType = ServiceVisitSortType.SuburbAsc;
            visitListViewModel.UpdateVisitsBasedOnSort(vm);

            //Assert
            var expectedList = visits.OrderBy(x => x.SiteSuburb);
            Assert.IsTrue(expectedList.SequenceEqual(visitListViewModel.Visits));
        }

        [Test]
        public void SortBySuburbDesc()
        {
            //Arrange
            ServiceVisitSortListViewModel vm = new ServiceVisitSortListViewModel();

            //Act
            vm.SelectedSortType = ServiceVisitSortType.SuburbDesc;
            visitListViewModel.UpdateVisitsBasedOnSort(vm);

            //Assert
            var expectedList = visits.OrderByDescending(x => x.SiteSuburb);
            Assert.IsTrue(expectedList.SequenceEqual(visitListViewModel.Visits));
        }

        [Test]
        public void SortByDueDateAsc()
        {
            //Arrange
            ServiceVisitSortListViewModel vm = new ServiceVisitSortListViewModel();

            //Act
            vm.SelectedSortType = ServiceVisitSortType.DueDateAsc;
            visitListViewModel.UpdateVisitsBasedOnSort(vm);

            //Assert
            var expectedList = visits.OrderBy(x => x.DueDate);
            Assert.IsTrue(expectedList.SequenceEqual(visitListViewModel.Visits));
        }

        [Test]
        public void SortByDueDateDesc()
        {
            //Arrange
            ServiceVisitSortListViewModel vm = new ServiceVisitSortListViewModel();

            //Act
            vm.SelectedSortType = ServiceVisitSortType.DueDateDesc;
            visitListViewModel.UpdateVisitsBasedOnSort(vm);

            //Assert
            var expectedList = visits.OrderByDescending(x => x.DueDate);
            Assert.IsTrue(expectedList.SequenceEqual(visitListViewModel.Visits));
        }

        [Test]
        public void SortByTaskAsc()
        {
            //Arrange
            ServiceVisitSortListViewModel vm = new ServiceVisitSortListViewModel();

            //Act
            vm.SelectedSortType = ServiceVisitSortType.TaskAsc;
            visitListViewModel.UpdateVisitsBasedOnSort(vm);

            //Assert
            var expectedList = visits.OrderBy(x => x.TotalTasks);
            Assert.IsTrue(expectedList.SequenceEqual(visitListViewModel.Visits));
        }

        [Test]
        public void SortByTaskDesc()
        {
            //Arrange
            ServiceVisitSortListViewModel vm = new ServiceVisitSortListViewModel();

            //Act
            vm.SelectedSortType = ServiceVisitSortType.TaskDesc;
            visitListViewModel.UpdateVisitsBasedOnSort(vm);

            //Assert
            var expectedList = visits.OrderByDescending(x => x.TotalTasks);
            Assert.IsTrue(expectedList.SequenceEqual(visitListViewModel.Visits));
        }

        [Test]
        public void SortByLoc()
        {
            //Arrange
            ServiceVisitSortListViewModel vm = new ServiceVisitSortListViewModel();

            //Act
            vm.SelectedSortType = ServiceVisitSortType.TaskDesc;
            visitListViewModel.UpdateVisitsBasedOnSort(vm);

            //Assert
            var expectedList = visits.OrderByDescending(x => x.TotalTasks);
            Assert.IsTrue(expectedList.SequenceEqual(visitListViewModel.Visits));
        }





    }
}