using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SWEN90013.Views;
using DLToolkit.Forms.Controls;
using SegmentedControl.FormsPlugin.Abstractions;
using SWEN90013.Data;
using System.IO;

namespace SWEN90013
{
    public partial class App : Application
    {
        public static double ScreenHeight;
        public static double ScreenWidth;

        static ServiceVisitDatabase _serviceVisitsDatabase;
        static TaskDatabase _taskDatabase;
        static ContractorDatabase _contractorDatabase;
        static CheckItemDatabase _checkItemDatabase;
        static DefectReportDatabase _defectReportDatabase;

        public static ServiceVisitDatabase ServiceVisitsDatabase
        {
            get
            {
                if (_serviceVisitsDatabase == null)
                {
                    _serviceVisitsDatabase = new ServiceVisitDatabase(
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "test1.db3"));

                    Console.WriteLine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "test1.db3"));
                }
                return _serviceVisitsDatabase;
            }
        }

        public static TaskDatabase TaskDatabase
        {
            get
            {
                if (_taskDatabase == null)
                {
                    _taskDatabase = new TaskDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "tasktest.db3"));
                    Console.WriteLine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "test10.db3"));
                }
                return _taskDatabase;
            }
        }

        public static ContractorDatabase ContractorDatabase
        {
            get
            {
                if (_contractorDatabase == null)
                {
                    _contractorDatabase = new ContractorDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                        , "contractor.db3"));
                }
                return _contractorDatabase;
            }
        }
        public static CheckItemDatabase CheckItemDatabase
        {
            get
            {
                if (_checkItemDatabase == null)
                {
                    _checkItemDatabase = new CheckItemDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                           "tasktest.db3"));
                    Console.WriteLine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                       "test10.db3"));
                }
                return _checkItemDatabase;
            }
        }

        public static DefectReportDatabase DefectReportDatabase
        {
            get
            {
                if (_defectReportDatabase == null)
                {
                    _defectReportDatabase = new DefectReportDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "tasktest.db3"));
                    Console.WriteLine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "test10.db3"));
                }
                return _defectReportDatabase;
            }
        }

        public App()
        {
            InitializeComponent();
            FlowListView.Init();

            //MainPage = new LoginPage();
            //MainPage = new NavigationPage(new TaskListPage());

            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
