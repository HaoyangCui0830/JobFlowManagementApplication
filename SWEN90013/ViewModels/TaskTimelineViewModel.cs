using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SWEN90013.Enums;
using SWEN90013.Models;
using SWEN90013.ServicesHandler;

namespace SWEN90013.ViewModels
{
    public class TaskTimelineViewModel : INotifyPropertyChanged
    {
        private string _taskNumber;
        private DateTime _startDate;
        private DateTime _endDate;

        private TaskTimelineServices _timelineService;
        private bool _isBusy;
        private bool _hasPreviousPage;

        //this property indicates whether the page is waiting for data
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        //this property indicates whether the previous timeline history still exists
        public bool HasPreviousPage
        {
            get { return _hasPreviousPage; }
            set
            {
                _hasPreviousPage = value;
                OnPropertyChanged(nameof(HasPreviousPage));
            }
        }

        //this property indicates the task ID
        public string TaskNumber
        {
            get
            {
                return _taskNumber;
            }
            set
            {
                _taskNumber = value;
            }
        }

        private ObservableCollection<TimelineViewModel> _timelines;

        public event PropertyChangedEventHandler PropertyChanged;
       

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TaskTimelineViewModel(int itemNumber)
        {
            _timelineService = new TaskTimelineServices();
            TaskNumber = itemNumber.ToString();

            InitializeDate();
            GetTimelineAsync();
        }

        public int GetLastIndex()
        {
            return Timelines.Count-1;
        }

        // Setup initial date range
        private void InitializeDate()
        {
            HasPreviousPage = true;
            _endDate = DateTime.Now;
            _startDate = _endDate.AddYears(-3);
        }

        // Setup the previous 3 years date range
        private void SubscractDateRange()
        {
            _endDate = _startDate.AddDays(-1);
            _startDate = _endDate.AddYears(-3);
        }

        // Convert date to String
        private String GetFormatStringDate(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        public ObservableCollection<TimelineViewModel> Timelines
        {
            get
            {
                return _timelines;
            }
            set
            {
                _timelines = value;
                OnPropertyChanged(nameof(Timelines));
            }
        }

     
        public void LoadItems()
        {
            GetPreviousTimelineAsync();
        }

        // Method to fetch the timeline data and transform it to viewmodel
        private async Task GetTimelineAsync()
        {
            try
            {
                IsBusy = true;
                string startDate = GetFormatStringDate(_startDate);
                string endDate = GetFormatStringDate(_endDate);

                var result = await _timelineService.GetTimelineList(TaskNumber, startDate, endDate);
                if (result != null)
                {
                    var timelines = new List<Timeline>(result).Select(s => new TimelineViewModel(s)).ToList();
                    var orderedTimelines = timelines.OrderByDescending(o => o.TestDate);
                    var newTimelines = new List<TimelineViewModel>();

                    if (Timelines != null) {

                        foreach (var item in Timelines)
                        {
                            newTimelines.Add(item);
                        }

                    } 
                    foreach (var item in orderedTimelines) {
                        newTimelines.Add(item);
                    }
                    HasPreviousPage = true;
                    IsBusy = false;

                    Timelines = new ObservableCollection<TimelineViewModel>(newTimelines);
                }
                else
                {
                    HasPreviousPage = false;
                    IsBusy = false;
                }
            }
            catch
            {

            }
        }

        // Method to fetch the previous data
        private async Task GetPreviousTimelineAsync()
        {
            if (!IsBusy && HasPreviousPage)
            {
                SubscractDateRange();
                await GetTimelineAsync();
            }

        }
    }
}
