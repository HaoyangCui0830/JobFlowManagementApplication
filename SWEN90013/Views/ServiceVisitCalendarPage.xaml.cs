using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using SWEN90013.Enums;
using System.Collections.Generic;
using SWEN90013.Views.ServiceVisitPages;
using SWEN90013.ViewModels;

namespace SWEN90013.Views
{
    public partial class ServiceVisitCalendarPage : ContentPage
    {
        DateTime today = DateTime.Today;
        public List<ServiceVisitViewModel> ServiceVisitSchedule;
        private ServiceVisitCalendarViewModel Visit;

        //private ObservableCollection<GroupedJobsModel> grouped { get; set; }
        public ServiceVisitCalendarPage(ServiceVisitCalendarViewModel visit)
        {
            InitializeComponent();
            today = startDatePicker.Date;
            ServiceVisitSchedule = visit.FullVisitsList;
            Visit = visit;
            Visit.SetDate(today);
            BuildUpPage();
        }

        // As it's hard to customise the calendar perfectly in Xaml, and Xamarin seems very slow on
        // rendering numbers of different grids with different listeners, I build the calendar table body in
        // C# manually, which could be more flexible to set UI details (using loop) and add GestureRecognizer
        // This page will re-build the main calendar body every time it's called.
        void BuildUpPage()
        {
            var totalTable = TableGrid;
            totalTable.Children.Clear();
            totalTable.RowSpacing = 0;
            totalTable.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            var title = DateRangeLabel;
            title.Text = today.Date.ToString("dd/MM/yyyy") + "-" + today.AddDays(4).ToString("dd/MM/yyyy");
            title.FontSize = 20;
            title.HorizontalOptions = LayoutOptions.CenterAndExpand;
            title.TextColor = Color.Red;

            for (var i = 0; i < 25; i++)
            {
                var oneRow = new Grid();
                oneRow.ColumnSpacing = 5;
                oneRow.HorizontalOptions = LayoutOptions.CenterAndExpand;
                oneRow.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                oneRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
                oneRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                oneRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                oneRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                oneRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                oneRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

                var headColumn = new Label();
                var eventColumn = new List<Label>();
                
                // build and customer the first row
                if (i == 0)
                {
                    headColumn = new Label { Text = " ",
                        BackgroundColor = Color.FromHex("black"),
                        WidthRequest = 50, HeightRequest = 40 };
                    for (var j = 0; j < 5; j++)
                    {
                        eventColumn.Add(new Label
                        {
                            Text = "    " + today.AddDays(j).ToString("dd/MM/yyyy"),
                            WidthRequest = 125,
                            HeightRequest = 40,
                            HorizontalOptions = LayoutOptions.Center,
                        });
                        oneRow.Children.Add(eventColumn[j], j + 1, 0);
                    }
                }
                else
                {
                    var time = 0;
                    if (i <= 12)
                    {
                        time = (i + 10) % 12 + 1;
                        if (time < 10)
                        {
                            headColumn = new Label { Text = " " + time + "AM",
                                BackgroundColor = Color.FromHex("#FDE9EA"),
                                WidthRequest = 50 };
                        }
                        else
                        {
                            headColumn = new Label { Text = time + "AM",
                                BackgroundColor = Color.FromHex("#FDE9EA"),
                                WidthRequest = 50 };
                        }

                    }
                    else
                    {
                        time = (i + 10) % 12 + 1;
                        if (time < 10)
                        {
                            headColumn = new Label { Text = " " + time + "PM",
                                BackgroundColor = Color.FromHex("#FDE9EA"),
                                WidthRequest = 50 };
                        }
                        else
                        {
                            headColumn = new Label { Text = time + "PM",
                                BackgroundColor = Color.FromHex("#FDE9EA"),
                                WidthRequest = 50 };
                        }
                    }

                    // add GestureRecognizer 
                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.NumberOfTapsRequired = 2; // double-tap
                    tapGestureRecognizer.Tapped += OnDisplayActionSheetButtonClicked;

                    int Maximal_Rows = 0;

                    // The follow lines (to the end of this function) will recognise event,
                    // hardle conflict schedules and set the calender format.
                    for (var j = 0; j < 5; j++)
                    {
                        int Schedule_num = 0;
                        for (var k = 0; k < ServiceVisitSchedule.ToArray().Length; k++)
                        {
                            if ((ServiceVisitSchedule[k].ScheduledDate.HasValue)
                                && (today.AddDays(j).Equals(ServiceVisitSchedule[k].ScheduledDate.Value.Date))
                                && (ServiceVisitSchedule[k].ScheduledDate.Value.Hour.ToString().Equals((i-1).ToString())))
                            {
                                Schedule_num += 1;
                            }
                        }
                        if(Schedule_num > Maximal_Rows)
                        {
                            Maximal_Rows = Schedule_num;
                        }
                    }


                    if (Maximal_Rows > 0)
                    {
                        for (var m = 0; m < Maximal_Rows; m++)
                        {
                            if (m == 0)
                            {
                                for (var j = 0; j < 5; j++)
                                {
                                    oneRow.Children.Add(new Label { Text = " ",
                                        BackgroundColor = Color.FromHex("#FDE9EA"),
                                        WidthRequest = 125 }, j + 1, m);
                                    oneRow.RowSpacing = 0;
                                }
                            }
                            else
                            {
                                oneRow.Children.Add(new Label { Text = " ",
                                    BackgroundColor = Color.FromHex("#FDE9EA"),
                                    WidthRequest = 50 }, 0, m);
                                for (var j = 0; j < 5; j++)
                                {
                                    oneRow.Children.Add(new Label { Text = " ",
                                        BackgroundColor = Color.FromHex("#FDE9EA"),
                                        WidthRequest = 125 }, j + 1, m);
                                    oneRow.RowSpacing = 0;
                                }

                            }
                            
                        }
                    }
                    else
                    {
                        for (var j = 0; j < 5; j++)
                        {
                            eventColumn.Add(new Label { Text = " ",
                                BackgroundColor = Color.FromHex("#FDE9EA"),
                                WidthRequest = 125 });
                            oneRow.Children.Add(eventColumn[j], j + 1, 0);
                        }
                    }

                    for (var j = 0; j < 5; j++)
                    {
                        var ServiceVisitList = new List<Label>();
                        int Schedule_num = 0;
                        for (var k = 0; k < ServiceVisitSchedule.ToArray().Length; k++)
                        {
                            if ((ServiceVisitSchedule[k].ScheduledDate.HasValue)
                                && (today.AddDays(j).Equals(ServiceVisitSchedule[k].ScheduledDate.Value.Date))
                                && ServiceVisitSchedule[k].ScheduledDate.Value.Hour.ToString().Equals((i - 1).ToString()) )
                            {
                                Schedule_num += 1;
                                var label = new Label
                                {
                                    Text = ServiceVisitSchedule[k].SiteName,
                                    BackgroundColor = Color.FromHex("#FDE9EA"),
                                    WidthRequest = 125
                                };
                                label.FontSize = 11;
                                ServiceVisitList.Add(label);
                                label.GestureRecognizers.Add(tapGestureRecognizer);
                                Console.WriteLine(Schedule_num);
                            }
                        }
                        for (var n = 0; n < Schedule_num; n++)
                        {
                            oneRow.Children.Add(ServiceVisitList[n], j + 1, n);
                        }
                    }
                }
                oneRow.Children.Add(headColumn, 0, 0);
                totalTable.Children.Add(oneRow, 0, i);
            }
        }



        async void OnDisplayActionSheetButtonClicked(object sender, System.EventArgs e)
        {
            // Another option here is using Action Sheet, However, this Xamarin native seems no longer completely suitable
            // The message could only be shown in the center.
            bool answer = await DisplayAlert("Make sure to delete this Service Visit?", "Click Yes to Delete", "Yes", "No");
            if (answer)
            {
                for (var k = 0; k < ServiceVisitSchedule.ToArray().Length; k++)
                {
                    if (((Label)sender).Text.Equals(ServiceVisitSchedule[k].SiteName))
                    {
                        Visit.FullVisitsList.ForEach((item) =>
                        {
                            if (item.SiteName.Equals(ServiceVisitSchedule[k].SiteName))
                            {
                                //item.ScheduledDate = null;
                                Visit.DeleteSchedule(item);
                            }
                        });

                    }
                }
                BuildUpPage();
                await DisplayAlert("Success", "You have deleted this Service Visit", "OK");
            }
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            Visit.SetDate(startDatePicker.Date);
            today = Visit.StartDate;
            BuildUpPage();
        }

        void LastWeek(object sender, System.EventArgs e)
        {
            Visit.MoveToLastFiveDay();
            today = Visit.StartDate;
            BuildUpPage();
        }

        void NextWeek(object sender, System.EventArgs e)
        {
            Visit.MoveToNextFiveDay();
            today = Visit.StartDate;
            BuildUpPage();
        }

    }
}

