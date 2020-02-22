using System;
using SWEN90013.Enums;
using SQLite;

namespace SWEN90013.Models
{
    public class ServiceVisit
    {
        #region constructor
        public ServiceVisit()
        {
        }
        #endregion constructor

        #region properties
        //[PrimaryKey, AutoIncrement]
        public int ServiceVisitId { get; set; }
        public DateTime DueDate { get; set; }
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public Address Address { get; set; }
        public Memos Memos { get; set; }
        public Contact Contact { get; set; }
        public Building Building { get; set; }
        public double PositionLat { get; set; }
        public double PositionLong { get; set; }
        public string SiteImageURL { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public int TasksRemaining { get; set; }
        public int TotalTasks { get; set; }
        public ServiceVisitStatus Status { get; set; }
        #endregion
    }
}
