using System;
using System.Collections.Generic;
using NUnit.Framework;
using SWEN90013.Enums;
using SWEN90013.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using SWEN90013.ViewModels;

namespace Tests
{
    [TestFixture]
    public class TaskVisitListViewModelTest
    {
        //Global variables
        TaskListViewModel taskListViewModel;
        List<TaskViewModel> ToDoVisits;
        List<TaskViewModel> DoneVisits;

        [SetUp]
        public void Setup()
        {
            ToDoVisits = new List<TaskViewModel>()
            {
                new TaskViewModel()
                {
                    TaskNumber = "144",
                    EquipmentName = "Building elements to sarisy fire",
                    EquipmentLocation = "Throughout",
                    LastServiceDate = new DateTime(2019, 10, 31),
                    ReferenceNumber = "23434",
                    LastTestResult = "Passed",
                    Barcode = "12345678"
                },
                new TaskViewModel()
                {
                    TaskNumber = "126",
                    EquipmentName = "Fire indices for materials",
                    EquipmentLocation = "Throughout",
                    LastServiceDate = new DateTime(2019, 10, 31),
                    ReferenceNumber = "23434",
                    LastTestResult = "Passed",
                    Barcode = "123456789",

                },
            };
            DoneVisits = new List<TaskViewModel>()
            { 

                new TaskViewModel()
                {
                    TaskNumber = "123",
                    EquipmentName = "Fire indices for materials",
                    EquipmentLocation = "Throughout",
                    LastServiceDate = new DateTime(2019, 10, 31),
                    ReferenceNumber = "23434",
                    LastTestResult = "Passed"

                }

            };
            taskListViewModel = new TaskListViewModel(ToDoVisits, DoneVisits);
        }
        [Test]
        public void SearchForId_TaskListVisitsTodo_ItemsNarrowedDown()
        {
            //Arrange
            string searchedTerm = "Building";

            //Act
            taskListViewModel.SearchedTerm = searchedTerm;

            //Assert
            bool visitsSearched = (taskListViewModel.ToDoTasks.Count > 0) &&
                                  taskListViewModel.ToDoTasks.All(v => v.EquipmentName.ToString().Contains(searchedTerm));
            Assert.IsTrue(visitsSearched);
        }

        [Test]
        public void SearchForId_TaskListVisitsDone_ItemsNarrowedDown()
        {
            //Arrange
            string searchedTerm = "materials";

            //Act
            taskListViewModel.SearchedTerm = searchedTerm;

            //Assert
            bool visitsSearched = (taskListViewModel.DoneTasks.Count > 0) &&
                                  taskListViewModel.DoneTasks.All(v => v.EquipmentName.ToString().Contains(searchedTerm));
            Assert.IsTrue(visitsSearched);
        }

    }
}
