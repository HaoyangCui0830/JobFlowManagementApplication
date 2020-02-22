using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using SWEN90013.ServicesHandler;

namespace SWEN90013.ViewModels
{
    // View model to show the Schedule infomation in a self-built Calender
    public class ServiceVisitCalendarViewModel : INotifyPropertyChanged
    {

        #region Variables
        private List<ServiceVisitViewModel> _fullVisit;
        private DateTime _today;
        #endregion

        public ServiceVisitCalendarViewModel(List<ServiceVisitViewModel> visits)
        {
            this._fullVisit = visits;
        }
        public List<ServiceVisitViewModel> FullVisitsList
        {
            get
            {
                return _fullVisit;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return _today;
            }
        }




        #region Methods

        public void SetDate(DateTime today)
        {
            this._today = today;
        }

        public void MoveToNextFiveDay()
        {
            this._today = this._today.AddDays(5);
        }

        public void MoveToLastFiveDay()
        {
            this._today = this._today.AddDays(-5);
        }

        public void DeleteSchedule(ServiceVisitViewModel item)
        {
            item.ScheduledDate = null;
            new ServiceVisitServices().DeleteServiceVisitSchedule(item.Id.ToString());
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

