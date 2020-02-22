using System;
using System.Collections.Generic;
using NUnit.Framework;
using SWEN90013.Enums;
using SWEN90013.ViewModels;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Windows.Input;

namespace Tests
{
    [TestFixture]
    public class BarcodeInputManuallyViewModelTest
    {
        private BarcodeInputManuallyViewModel barcodeInputManuallyViewModel;
        List<TaskViewModel> doneTasks;
        List<TaskViewModel> undoneTasks;

        [SetUp]
        public void Setup()
        {
            doneTasks = new List<TaskViewModel>()
            {
                new TaskViewModel()
                {
                    TaskNumber = "234234",
                    EquipmentName = "Building elements to sarisy fire",
                    EquipmentLocation = "Throughout",
                    LastServiceDate = new DateTime(2019, 10, 31),
                    ReferenceNumber = "23434",
                    LastTestResult = "Passed",
                    Barcode = "12345678"
                }
            };
            undoneTasks = new List<TaskViewModel>()
            {
                new TaskViewModel()
                {
                    TaskNumber = "234236",
                    EquipmentName = "Fire indices for materials",
                    EquipmentLocation = "Throughout",
                    LastServiceDate = new DateTime(2019, 10, 31),
                    ReferenceNumber = "23434",
                    LastTestResult = "Passed",
                    Barcode = "123456789",

                }
            };
            barcodeInputManuallyViewModel = new BarcodeInputManuallyViewModel(undoneTasks, doneTasks);

        }


        [Test]
        public void TestOldQR1()
        {
            //Act
            barcodeInputManuallyViewModel.Barcode = "123456789";
            barcodeInputManuallyViewModel.SubmitBarcode();
            

            Assert.IsTrue(barcodeInputManuallyViewModel.ExistedTask != null);
        }
        [Test]
        public void TestOldQR2()
        {
            //Act
            barcodeInputManuallyViewModel.Barcode = "12345678";
            barcodeInputManuallyViewModel.SubmitBarcode();


            Assert.IsTrue(barcodeInputManuallyViewModel.ExistedTask != null);
        }
        [Test]
        public void TestNewQR()
        {
            //Act
            barcodeInputManuallyViewModel.Barcode = "1234567890";
            barcodeInputManuallyViewModel.SubmitBarcode();


            Assert.IsTrue(barcodeInputManuallyViewModel.ExistedTask == null);
        }

    }
}
