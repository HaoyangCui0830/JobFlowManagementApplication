using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SWEN90013.ServicesHandler;
using SWEN90013.Enums;
using System.Windows.Input;
using Xamarin.Forms;
using SWEN90013.Views;
using System.Linq;
using System.Collections.ObjectModel;
using SWEN90013.ViewModels.ServiceVisitSort;
using SWEN90013.Helpers;
using Xamarin.Forms.Maps;
using SWEN90013.Models;
using Xamarin.Essentials;
using Plugin.SecureStorage;
using SWEN90013.Data;

namespace SWEN90013.ViewModels
{
    //view model to be used to show all the service visits that a technician is assigned to
    public class ServiceVisitListViewModel : INotifyPropertyChanged
    {

        #region Variables
        private bool _isBusy;
        private readonly ServiceVisitServices _visitServices = new ServiceVisitServices();

        //this is the full list of visits for the user. Has to be saved as users are allowed to
        //search and filter the list
        private List<ServiceVisitViewModel> _fullVisitsList;
        private ObservableCollection<ServiceVisitViewModel> _visits;
        private string _searchedTerm;

        private string _imageFilter;
        private string _imageSort;
        private bool _isRefreshing = false;    
        private ServiceVisitFilterViewModel _filterViewModel;
        private ServiceVisitSortListViewModel _sortListViewModel;
        #endregion

        #region Constructors
        public ServiceVisitListViewModel()
        {
            _ = InitializeGetVisitsAsync();
            RescheduleCommand = new Command(RescheduleDateTime);
        }

        //initialised the view models with the visits list as parameter (instead of API call)
        //mainly used by unit tests
        public ServiceVisitListViewModel(List<ServiceVisitViewModel> visits)
        {
            _fullVisitsList = visits;
            _visits = new ObservableCollection<ServiceVisitViewModel>(visits);
            InitializeFilterVM();
        }
        #endregion

        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;

        public INavigation Navigation { get; set; }


        //List of servie visits to be displayed
        public ObservableCollection<ServiceVisitViewModel> Visits 
        { 
            get { return _visits; }
            set
            {
                _visits = value;
                OnPropertyChanged(nameof(Visits));
            }
        }

        // The page's refreshing state
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        //  Get the list of Service Visit
        public List<ServiceVisitViewModel> FullVisitsList
        {
            get
            {
                return _fullVisitsList;
            }
        }

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

        //term that the user wants to search service visits by
        public string SearchedTerm
        {
            get { return _searchedTerm; }
            set
            {
                _searchedTerm = value;
                OnPropertyChanged(nameof(SearchedTerm));
                _ = FilterAndSearchVisit();
            }
        }

        //icon to be shown when service visits are filtered
        public string ImageFilter
        {
            get { return _imageFilter; }
            set
            {
                _imageFilter = value;
                OnPropertyChanged(nameof(ImageFilter));
            }
        }

        public string ImageSort
        {
            get
            {
                return _imageSort;
            }
            set
            {
                _imageSort = value;
                OnPropertyChanged(nameof(ImageSort));
            }
        }
        //list of filters that are currently applied
        public ServiceVisitFilterViewModel FilterViewModel
        {
            get
            {
                return _filterViewModel;
            }

        }

        public ServiceVisitSortListViewModel SortListViewModel
        {
            get
            {
                return _sortListViewModel;
            }
        }

        
        #endregion

        #region Commands
        public ICommand RescheduleCommand{ get; set; }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    await InitializeGetVisitsAsync();

					ImageSort = "";
					ImageFilter = "";

                    IsRefreshing = false;
                });
            }
        }

        public ICommand SyncLocalDataCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsBusy = true;
                    try
                    {
                        await ServiceVisitDataController.submitAllLocalUpdateAsync();
                        await TaskSyncController.submitAllLocalTaskChanges();
                    }
                    catch (Exception e) { Console.WriteLine("Sync fail"); }
                    finally
                    {
                        IsBusy = false;
                    }
                }
                );
            }
        }
        #endregion

        #region Methods
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //method to redirect to the calendar page
        //the object parameter is a string that contains the id of the visit
        private void RescheduleDateTime(object obj)
        {
            //int Id = Int16.Parse(obj.ToString());
            //Console.WriteLine(obj.ToString());
            int Id = Int32.Parse(obj.ToString());
            foreach (ServiceVisitViewModel sv in _visits)
            {
                if (sv.Id == Id)
                {
                    MessagingCenter.Subscribe<ServiceVisitListPage, DateTime>(this, "ScheduledDateTime" + Id, (sender, datetime) =>
                       {
                           sv.ScheduledDate = datetime;
                           //Console.WriteLine(sv.ScheduledDate);
                        
                           new ServiceVisitServices().RescheduleServiceVisitSchedule(Id.ToString(), datetime);

                       }
                    );

                }
            }
            //Navigation.PushAsync(new LoginPage());
        }

        //method to find service visits according to the searched term
        //user can find visits based on id, site name, suburb, or full address
        private ObservableCollection<ServiceVisitViewModel> SearchServiceVisits()
        {

            if (!String.IsNullOrWhiteSpace(_searchedTerm))
            {
                var visitList = _fullVisitsList.Where(v => (v.ServiceVisitNumber.ToString().Contains(_searchedTerm) ||
                                                            v.SiteName.ToLower().Contains(_searchedTerm.ToLower()) ||
                                                            v.SiteSuburb.ToLower().Contains(_searchedTerm.ToLower()) ||
                                                            v.FullAddress.ToLower().Contains(_searchedTerm.ToLower()))
                                                            ).ToList();
                return new ObservableCollection<ServiceVisitViewModel>(visitList);
            }
            //if the search term is empty, display all of the visits
            else
            {
                return new ObservableCollection<ServiceVisitViewModel>(_fullVisitsList);
            }
        }

        private async Task<ObservableCollection<ServiceVisitViewModel>> SortedVisitAsync(ObservableCollection<ServiceVisitViewModel> visits)
        { 
            var sortType = this.SortListViewModel.SelectedSortType;
            var result = new List<ServiceVisitViewModel>();
            if (sortType != ServiceVisitSortType.LocAsc && sortType != ServiceVisitSortType.LocDesc)
            {
                EnableSortIcon();
            }
            if (sortType != ServiceVisitSortType.None)
            {
                switch(sortType)
                {
                    case ServiceVisitSortType.SuburbAsc:
                        result = visits.OrderBy(o => o.SiteSuburb).ToList();
                        break;
                    case ServiceVisitSortType.SuburbDesc:
                        result = visits.OrderByDescending(o => o.SiteSuburb).ToList();
                        break;
                    case ServiceVisitSortType.DueDateAsc:
                        result = visits.OrderBy(o => o.DueDate).ToList();
                        break;
                    case ServiceVisitSortType.DueDateDesc:
                        result = visits.OrderByDescending(o => o.DueDate).ToList();
                        break;
                    case ServiceVisitSortType.TaskAsc:
                        result = visits.OrderBy(o => o.TotalTasks).ToList();
                        break;

                    case ServiceVisitSortType.TaskDesc:
                        result = visits.OrderByDescending(o => o.TotalTasks).ToList();
                        break;
                    case ServiceVisitSortType.LocAsc:
                        result = await CalculateDistanceToCurrentLocationAsync(visits, true);
                        break;

                    case ServiceVisitSortType.LocDesc:
                        result = await CalculateDistanceToCurrentLocationAsync(visits, false);
                        break;
                }
                return new ObservableCollection<ServiceVisitViewModel>(result);
            }
            else
            {
                return new ObservableCollection<ServiceVisitViewModel>(visits);
            }
        }

        private async Task <List<ServiceVisitViewModel>> CalculateDistanceToCurrentLocationAsync(ObservableCollection<ServiceVisitViewModel> visits, Boolean isAsc)
        {
            var currentLocation = await LocationHelper.CurrentPositionAsync();
            var currentLatitude = currentLocation.Latitude;
            var currentLongitude = currentLocation.Longitude;
           
            var result = new List<ServiceVisitViewModel>();
            if (currentLatitude.Equals(0) && currentLongitude.Equals(0))
            {
                this.SortListViewModel.RemoveSortSetting();
                await Application.Current.MainPage.DisplayAlert("Location Service Alert", "You need to turn on or give permission to your location service to enable sorting by location", "Ok");
                result = visits.ToList();
            }
            else
            {
                foreach (ServiceVisitViewModel viewModel in visits)
                {
                    viewModel.DistanceToCurrentLoc = LocationHelper.getDistanceFromLatLonInKmd(currentLatitude, currentLongitude, viewModel.PositionLat, viewModel.PostionLong);
                }

               
                if (isAsc)
                {
                    result = visits.OrderBy(o => o.DistanceToCurrentLoc).ToList();
                }
                else
                {
                    result = visits.OrderByDescending(o => o.DistanceToCurrentLoc).ToList();
                }

            }
            this.EnableSortIcon();

            return result;
        }

        // store the internet state of last update
        private Boolean lastInternetState = true;

        //make an api call to get all the visits for the technician that is logged in
        private async Task InitializeGetVisitsAsync()
        {
            try
            {
                IsBusy = true;
                var current = Connectivity.NetworkAccess;

                if (current == NetworkAccess.Internet)
                {
                    if (this.lastInternetState == false)
                    {
						await ServiceVisitDataController.submitAllLocalUpdateAsync();
                        this.lastInternetState = true;
					}
                }
               
                // Connection to internet is available
                var result = await _visitServices.GetServiceVisitListsForUser(CrossSecureStorage.Current.GetValue("UserCode"));

                if (result != null)
                {
                    _fullVisitsList = new List<ServiceVisit>(result).Select(s => new ServiceVisitViewModel(s)).ToList();
                    List<ServiceVisit> serviceVisits = new List<ServiceVisit>(result);

                    // save into local DB at the first time
                    
                    if (current == NetworkAccess.Internet)
                    {
                        this.lastInternetState = true;
                        foreach (ServiceVisit sv in serviceVisits)
                        {
                            ServiceVisitDBInfo svDBInfo = new ServiceVisitDBInfo(sv);
                            _ = App.ServiceVisitsDatabase.SaveServiceVisitAsync(svDBInfo);
                            Console.WriteLine(svDBInfo.ServiceVisitId);
                        }
                    }
                    else
                    {
                        this.lastInternetState = false;
                    }
                        
                    //App.ServiceVisitsDatabase.SaveNoteAsync(_fullVisitsList);

                    Visits = new ObservableCollection<ServiceVisitViewModel>(_fullVisitsList);
                    InitializeFilterVM();
                    InitializeSortVM();
                }
                //if result is null, there is an error with the call
                else
                {
                    await Navigation.PushAsync(new ErrorPage(new ServiceVisitListMasterPage()));
                }

                
                //else
                //{
                //    var serviceVisitDB = await App.ServiceVisitsDatabase.GetServiceVisitsAsync();
                //    List<ServiceVisitDBInfo> serviceVisitDBList = new List<ServiceVisitDBInfo>(serviceVisitDB);
                //    List<ServiceVisit> result = new List<ServiceVisit>();
                //    foreach (ServiceVisitDBInfo svInfo in serviceVisitDBList)
                //    {
                //        result.Add(svInfo.getServiceVisit());
                //    }

                //    if (result != null)
                //    {
                //        _fullVisitsList = new List<ServiceVisit>(result).Select(s => new ServiceVisitViewModel(s)).ToList();
                //        List<ServiceVisit> serviceVisits = new List<ServiceVisit>(result);
                        
                //        //App.ServiceVisitsDatabase.SaveNoteAsync(_fullVisitsList);

                //        Visits = new ObservableCollection<ServiceVisitViewModel>(_fullVisitsList);
                //        InitializeFilterVM();
                //        InitializeSortVM();
                //    }
                //    //if result is null, there is an error with the call
                //    else
                //    {
                //        await Navigation.PushAsync(new ErrorPage(new ServiceVisitListMasterPage()));
                //    }

                //}
                //var result = await _visitServices.GetServiceVisitListsForUser("P09");
                
            }
            finally
            {
                IsBusy = false;
            }
        }


        


        // Initialize filter after we get the full visit list
        public void InitializeFilterVM()
        {
            this._filterViewModel = new ServiceVisitFilterViewModel(_fullVisitsList);
        }

        public void InitializeSortVM()
        {
            this._sortListViewModel = new ServiceVisitSortListViewModel();
        }

        //Update the displayed service visit with the filtered values
        public void UpdateVisitsBasedOnFilter(ServiceVisitFilterViewModel viewModel)
        {
            this._filterViewModel = viewModel;
            _ = this.FilterAndSearchVisit();
        }

        //Update the displayed service visit with the filtered values
        public void UpdateVisitsBasedOnSort(ServiceVisitSortListViewModel viewModel)
        {
            this._sortListViewModel = viewModel;
            _ = this.FilterAndSearchVisit();
        }

        // The logic to filter service visit based on selected suburb,  calendar, due date, and status
        public ObservableCollection<ServiceVisitViewModel> FilterServiceVisit()
        {
            EnableFilterIcon();
            var temporaryVisits = _fullVisitsList;
            var filteredBySuburb = this.FilterBySuburb(temporaryVisits);
            var filteredByCalendar = this.FilterByCalendar(filteredBySuburb);
            var filteredByDueDate = this.FilterByDueDate(filteredByCalendar);
            return this.FilterByStatus(filteredByDueDate);
        }

        public async Task FilterAndSearchVisit()
        {
            var filteredVisits = new ObservableCollection<ServiceVisitViewModel>(_fullVisitsList);
            var searchedVisits = new ObservableCollection<ServiceVisitViewModel>(_fullVisitsList);

            //get filtered visits
            filteredVisits = FilterServiceVisit();

            //get searched visits
            searchedVisits = SearchServiceVisits();


            //get the conjunction of the two lists
            Visits = new ObservableCollection<ServiceVisitViewModel>(filteredVisits.Where(v => searchedVisits.Contains(v)));
            Visits = await SortedVisitAsync(Visits);
        }

        public void EnableFilterIcon()
        {
            if (!this.FilterViewModel.IsAllFilterSettings()) 
            {
                ImageFilter = "icon-checked.png";
            }
            else
            {
                ImageFilter = "";
            }
        }

        public void EnableSortIcon()
        {
            if (SortListViewModel.SelectedSortType.Equals(ServiceVisitSortType.None))
            {
                ImageSort = ""; 
            }
            else
            {
                ImageSort = "icon-checked.png";
            }

        }

        // The logic to filter service visit based on selected suburb
        private List<ServiceVisitViewModel> FilterBySuburb(List<ServiceVisitViewModel> temp)
        {
            var temporaryVisits = new List<ServiceVisitViewModel>();

            foreach (ServiceVisitViewModel visit in temp)
            {
                foreach (String suburbName in this.FilterViewModel.SelectedSuburbs)
                {
                    if (visit.SiteSuburb.Equals(suburbName)|| suburbName.Equals("All"))
                    {
                        temporaryVisits.Add(visit);
                    }
                }
            }

            return temporaryVisits;
        }

        // The logic to filter service visit based on selected calendar
        private List<ServiceVisitViewModel> FilterByCalendar(List<ServiceVisitViewModel> temp)
        {
            var temporaryVisits = new List<ServiceVisitViewModel>();

            foreach (ServiceVisitViewModel visit in temp)
            {
                switch (this.FilterViewModel.SelectedSchedule)
                {
                    case ServiceVisitSchedule.All:
                        temporaryVisits.Add(visit);
                        break;

                    case ServiceVisitSchedule.AllScheduled:
                        if (visit.ScheduledDate != null)
                        {
                            temporaryVisits.Add(visit);
                        }
                        break;

                    case ServiceVisitSchedule.MonthlyScheduled:
                        if (visit.ScheduledDate != null && ServiceVisitScheduleMethods.IsMonthlyScheduled((System.DateTime)visit.ScheduledDate) == true)
                        {
                            temporaryVisits.Add(visit);
                        }
                        break;

                    case ServiceVisitSchedule.TodayScheduled:
                        if (visit.ScheduledDate != null && ServiceVisitScheduleMethods.IsTodayScheduled((System.DateTime)visit.ScheduledDate) == true)
                        {
                            temporaryVisits.Add(visit);
                        }
                        break;

                    case ServiceVisitSchedule.Unscheduled:
                        if (visit.ScheduledDate == null)
                        {
                            temporaryVisits.Add(visit);
                        }
                        break;

                    case ServiceVisitSchedule.WeeklyScheduled:
                        if (visit.ScheduledDate != null && ServiceVisitScheduleMethods.IsWeeklyScheduled((System.DateTime)visit.ScheduledDate) == true)
                        {
                            temporaryVisits.Add(visit);
                        }
                        break;

                }
            }
            return temporaryVisits;
        }

        // The logic to filter service visit based on selected due date
        private List<ServiceVisitViewModel> FilterByDueDate(List<ServiceVisitViewModel> temp)
        {
            var temporaryVisits = new List<ServiceVisitViewModel>();

            var thisMonth = DateTime.Now;
            foreach (ServiceVisitViewModel visit in temp)
            {

                if (visit.DueDate.Month.Equals(thisMonth.Month) || this.FilterViewModel.IsAllDueDate)
                {
                    temporaryVisits.Add(visit);
                }
            }

            return temporaryVisits;
        }

        // The logic to filter service visit based on selected status
        private ObservableCollection <ServiceVisitViewModel> FilterByStatus(List<ServiceVisitViewModel> temp)
        {
            var temporaryVisits = new ObservableCollection<ServiceVisitViewModel>();

            var thisMonth = DateTime.Now;
            foreach (ServiceVisitViewModel visit in temp)
            {
                if (visit.Status.Equals(this.FilterViewModel.SelectedStatus) || this.FilterViewModel.SelectedStatus.Equals(ServiceVisitStatus.All))
                {
                    temporaryVisits.Add(visit);
                }
                else if(this.FilterViewModel.SelectedStatus.Equals(ServiceVisitStatus.Scheduled) && visit.ScheduledDate != null)
                {
                    temporaryVisits.Add(visit);
                }
            }

            return temporaryVisits;
        }

        #endregion
    }
}
