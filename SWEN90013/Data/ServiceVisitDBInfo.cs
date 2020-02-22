using System;
using SWEN90013.Models;
using SQLite;
using SWEN90013.Enums;
namespace SWEN90013.Data
{
    public class ServiceVisitDBInfo
    {
        #region constructor
        public ServiceVisitDBInfo(ServiceVisit serviceVisit)
        {
            this.ServiceVisitId = serviceVisit.ServiceVisitId;
            
            this.DueDate = serviceVisit.DueDate;
            this.SiteId = serviceVisit.SiteId;
            this.SiteName = serviceVisit.SiteName;
            this.SiteAddressLine1 = serviceVisit.Address.SiteAddressLine1;
            this.SiteAddressLine2 = serviceVisit.Address.SiteAddressLine2;
            this.SiteAddressLine3 = serviceVisit.Address.SiteAddressLine3;
            this.SiteAddressState = serviceVisit.Address.SiteAddressState;
            this.SiteAddressPostCode = serviceVisit.Address.SiteAddressPostCode;
            this.ContactMemo = serviceVisit.Memos.ContactMemo;
            this.InductionMemo = serviceVisit.Memos.InductionMemo;
            this.OHSMemo = serviceVisit.Memos.OHSMemo;
            this.FSMMemo = serviceVisit.Memos.FSMMemo;
            this.ServiceMemo = serviceVisit.Memos.ServiceMemo;
            this.ContactName = serviceVisit.Contact.ContactName;
            this.ContactTelephone = serviceVisit.Contact.ContactTelephone;
            this.ContactMobile = serviceVisit.Contact.ContactMobile;
            this.ContactEmail = serviceVisit.Contact.ContactEmail;
            this.BuildingEra = serviceVisit.Building.BuildingEra;
            this.BuildingSize = serviceVisit.Building.BuildingSize;
            this.BuildingParts = serviceVisit.Building.BuildingParts;
            this.BuildingClass = serviceVisit.Building.BuildingClass;
            this.KeysHeld = serviceVisit.Building.KeysHeld;
            this.PositionLat = serviceVisit.PositionLat;
            this.PositionLong = serviceVisit.PositionLong;
            this.SiteImageURL = serviceVisit.SiteImageURL;
            this.ScheduledDate = serviceVisit.ScheduledDate;
            this.TasksRemaining = serviceVisit.TasksRemaining;
            this.TotalTasks = serviceVisit.TotalTasks;
            this.serviceVisitStatus = serviceVisit.Status.ToString();
            //this.ServiceVisitIdNum = serviceVisit.ServiceVisitId;
            //this.updateBuildingDetails = false;
            //this.updateMemos = false;
            //this.updateReschedule = false;
            //this.deleteSchedule = false;
		}

        public ServiceVisitDBInfo() { }
		#endregion constructor

		#region properties
		[PrimaryKey]
		public int ServiceVisitId { get; set; }

        //public int ServiceVisitIdNum { get; set; }
		public DateTime DueDate { get; set; }
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public string SiteAddressLine1 { get; set; }
        public string SiteAddressLine2 { get; set; }
        public string SiteAddressLine3 { get; set; }
        public string SiteAddressState { get; set; }
        public int SiteAddressPostCode { get; set; }
        public string ContactMemo { get; set; }
        public string InductionMemo { get; set; }
        public string OHSMemo { get; set; }
        public string FSMMemo { get; set; }
        public string ServiceMemo { get; set; }
        public string ContactName { get; set; }
        public string ContactTelephone { get; set; }
        public string ContactMobile { get; set; }
        public string ContactEmail { get; set; }
        public string BuildingEra { get; set; }
        public string BuildingSize { get; set; }
        public string BuildingParts { get; set; }
        public string BuildingClass { get; set; }
        public int KeysHeld { get; set; }
        public double PositionLat { get; set; }
        public double PositionLong { get; set; }
        public string SiteImageURL { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public int TasksRemaining { get; set; }
        public int TotalTasks { get; set; }
        public string serviceVisitStatus { get; set; }
        public Boolean updateBuildingDetails { get; set; }
        public Boolean updateMemos { get; set; }
        public Boolean updateReschedule { get; set; }
        public Boolean deleteSchedule { get; set; }
        #endregion

        public ServiceVisit getServiceVisit()
        {
            ServiceVisit serviceVisit = new ServiceVisit();

            serviceVisit.ServiceVisitId = this.ServiceVisitId;
            serviceVisit.DueDate = this.DueDate;
            serviceVisit.SiteId = this.SiteId;
            serviceVisit.SiteName = this.SiteName;

            Address address = new Address();
            address.SiteAddressLine1 = this.SiteAddressLine1;
            address.SiteAddressLine2 = this.SiteAddressLine2;
            address.SiteAddressLine3 = this.SiteAddressLine3;
            address.SiteAddressState = this.SiteAddressState;
            address.SiteAddressPostCode =  this.SiteAddressPostCode;
			serviceVisit.Address = address;

			Memos memos = new Memos();
			memos.ContactMemo = this.ContactMemo;
			memos.InductionMemo = this.InductionMemo;
			memos.OHSMemo = this.OHSMemo;
			memos.FSMMemo = this.FSMMemo;
			memos.ServiceMemo = this.ServiceMemo;
			serviceVisit.Memos = memos;

			Contact contact = new Contact();
			contact.ContactName = this.ContactName;
			contact.ContactTelephone = this.ContactTelephone;
			contact.ContactMobile = this.ContactMobile;
			contact.ContactEmail = this.ContactEmail;
			serviceVisit.Contact = contact;

			Building building = new Building();
			building.BuildingEra = this.BuildingEra;
			building.BuildingSize = this.BuildingSize;
			building.BuildingParts = this.BuildingParts;
			building.BuildingClass = this.BuildingClass;
			serviceVisit.Building = building;


			serviceVisit.Building.KeysHeld = this.KeysHeld;
			serviceVisit.PositionLat = this.PositionLat;
			serviceVisit.PositionLong = this.PositionLong;
			serviceVisit.SiteImageURL = this.SiteImageURL;
			serviceVisit.ScheduledDate = this.ScheduledDate;
			serviceVisit.TasksRemaining = this.TasksRemaining;
			serviceVisit.TotalTasks = this.TotalTasks;
			serviceVisit.Status = (ServiceVisitStatus)Enum.Parse(typeof(ServiceVisitStatus), this.serviceVisitStatus);

			return serviceVisit;
        }
    }
}
