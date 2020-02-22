using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Linq;
using SWEN90013.Enums;
using SWEN90013.Models;
using Xamarin.Forms.Maps;
using SWEN90013.ServicesHandler.GeoLocationHandler;
using System.Runtime.Serialization.Formatters.Binary;
using SWEN90013.ServicesHandler;
using Xamarin.Forms;

namespace SWEN90013.ViewModels
{
    public class ServiceVisitViewModel
    {
        #region Constructors
        public ServiceVisitViewModel()
        {
        }
        public ServiceVisitViewModel(ServiceVisit visit)
        {
            //load lists of postcodes and suburbs
            List<Locale> locales = File.ReadAllLines("australian_postcodes.csv")
                               .Skip(1)
                               .Select(v => new Locale(v))
                               .ToList();

            //TODO ServiceVisitNumber
            Id = visit.ServiceVisitId;
            DueDate = visit.DueDate;
            ServiceVisitNumber = visit.ServiceVisitId;
            SiteId = visit.SiteId;
            SiteName = visit.SiteName;
            SiteAddressLine1 = visit.Address.SiteAddressLine1;
            SiteAddressLine2 = visit.Address.SiteAddressLine2;
            SiteAddressLine3 = visit.Address.SiteAddressLine3;
            SiteAddressPostCode = visit.Address.SiteAddressPostCode;

            //search for suburb based on postcode
            Locale currentLocale = locales.Where(l => l.PostCode == visit.Address.SiteAddressPostCode).FirstOrDefault();
            if (currentLocale != null)
            {
                SiteSuburb = currentLocale.Suburb;
            }

            ContactMemo = visit.Memos.ContactMemo;
            InductionMemo = visit.Memos.InductionMemo;
            OHSMemo = visit.Memos.OHSMemo;
            FSMMemo = visit.Memos.FSMMemo;
            ServiceMemo = visit.Memos.ServiceMemo;

            ContactName = visit.Contact.ContactName;
            ContactTelephone = visit.Contact.ContactTelephone;
            ContactMobile = visit.Contact.ContactMobile;
            ContactEmail = visit.Contact.ContactEmail;

            BuildingEra = visit.Building.BuildingEra;
            BuildingSize = visit.Building.BuildingSize;
            BuildingParts = visit.Building.BuildingParts;
            BuildingClass = visit.Building.BuildingClass;
            KeyHeld = visit.Building.KeysHeld != 0;

            PositionLat = visit.PositionLat;
            PostionLong = visit.PositionLong;
            SiteImageUrl = visit.SiteImageURL.Length > 0 ? visit.SiteImageURL : "no-image.png";
            ScheduledDate = visit.ScheduledDate;
            TasksRemaining = visit.TasksRemaining;
            TotalTasks = visit.TotalTasks;
            Status = visit.Status;

            // for details page
            _buildingEraIndex = _buildingDetails.GetBuildingDetailsEraIndex(BuildingEra);
            _buildingSizeIndex = _buildingDetails.GetBuildingDetailsSizeIndex(BuildingSize);
            _buildingClassIndex = _buildingDetails.GetBuildingDetailsClassIndex(BuildingClass);
        }
        #endregion

        #region Properties
        //TODO - Subject to change depending on the data from the backend
        public int Id { get; set; }
        public int ServiceVisitNumber { get; set; }
        public int TotalTasks { get; set; }

        public string SiteName { get; set; }
        public string SiteSuburb { get; set; }
        public Position? Coordinate { get; set; }

        public ServiceVisitStatus Status { get; set; }

        public DateTime? ScheduledDate { get; set; }
        public DateTime DueDate { get; set; }
        public double DistanceToCurrentLoc { get; set; }
        public ImageSource SiteImageSource { get; set; }
        public Boolean updateBuildingDetails { get; set; }
        public Boolean updateMemosDetails { get; set; }
        public Boolean updateReschedule { get; set; }
        public Boolean deleteSchedule { get; set; }

        //called to return a string to be displayed on ServiceVisitListPage
        public string ScheduledDateDescription
        {
            get
            {
                if (ScheduledDate.HasValue)
                {
                    return "Scheduled for: " + ScheduledDate.Value.ToString("dd/MM/yyyy HH:mm:ss");
                }
                else
                {
                    return "Add to calendar";
                }
            }
        }
        //called to return the siteId + serviceVisitID on ServiceVisitListPage
        public string FullTitle
        {
            get
            {
                return String.Format("{0} ({1})", SiteName, ServiceVisitNumber);
            }
        }

        //called to return status enum's description
        public string StatusDescription
        {
            get { return Status.GetDescription(); }
        }

        //called to get the color of a status
        public string StatusColor
        {
            get { return Status.GetColor(); }
        }

        public int SiteId { get; set; }
        public string SiteAddressLine1 { get; set; }
        public string SiteAddressLine2 { get; set; }
        public string SiteAddressLine3 { get; set; }
        public string SiteAddressState { get; set; }
        public int SiteAddressPostCode { get; set; }
        public string FullAddress
        {
            get
            {
                return String.Format("{0}, {1}, {2}, {3} {4}"
                    , SiteAddressLine1
                    , SiteAddressLine2
                    , SiteAddressLine3
                    , SiteAddressState
                    , SiteAddressPostCode);
            }
        }
        public string ContactName { get; set; }
        public string ContactTelephone { get; set; }
        public string ContactMobile { get; set; }
        public string ContactSnippt
        {
            get
            {
                return String.Format("{0}, ({1})", ContactName, ContactMobile);
            }
        }
        public string ContactEmail { get; set; }
        public string ContactMemo { get; set; }

        private BuildingDetailsValues _buildingDetails = new BuildingDetailsValues();
        public string BuildingEra { get; set; }
        public string BuildingSize { get; set; }
        public string BuildingParts { get; set; }
        public string BuildingClass { get; set; }
        public bool KeyHeld { get; set; }

        private int _buildingSizeIndex { get; set; }
        private int _buildingEraIndex { get; set; }
        private int _buildingClassIndex { get; set; }

        public IList<string> BuildingEraList
        {
            get => _buildingDetails.BuildingDetailsEraList;
        }
        public IList<string> BuildingSizeList
        {
            get => _buildingDetails.BuildingDetailsSizeList;
        }
        public IList<string> BuildingClassList
        {
            get => _buildingDetails.BuildingDetailsClassList;
        }
        public int BuildingEraIndex
        {
            get => _buildingEraIndex;
            set {
                if (value == _buildingEraIndex)
                    return;

                _buildingEraIndex = value;
                //BuildingEra = _buildingDetails.GetBuildingDetailsEraDesciption(_buildingEraIndex);
            }
        }
        public int BuildingSizeIndex
        {
            get => _buildingSizeIndex;
            set {
                if (value == _buildingSizeIndex)
                    return;

                _buildingSizeIndex = value;
                //BuildingSize = _buildingDetails.GetBuildingDetailsSizeDescription(_buildingSizeIndex);
            }
        }
        public int BuildingClassIndex
        {
            get => _buildingClassIndex;
            set {
                if (value == _buildingClassIndex)
                    return;

                _buildingClassIndex = value;
                //BuildingClass = _buildingDetails.GetBuildingDetailsClassDescription(_buildingClassIndex);
            }
        }
        
        public string InductionMemo { get; set; }
        public string OHSMemo { get; set; }
        public string ServiceMemo { get; set; }
        public string FinanceBackOfficeMemo { get; set; }
        public string FSMMemo { get; set; }
        public double PositionLat { get; set; }
        public double PostionLong { get; set; }
        public int TasksRemaining { get; set; }

        public string SiteImageUrl { get; set; }
        
        public Position GetCoordinateInfo
        {
            get
            {
                if (Coordinate.HasValue)
                {
                    return Coordinate.Value;
                }
                else
                {
                    //Using Google API to get Coordinate
                    GeoLocationInfoHelper LocationHelper = new GeoLocationInfoHelper();
                    Position SearchedCoordinate = LocationHelper.GetCoordinates(FullAddress);
                    return SearchedCoordinate;
                }
            }
        }

        //public static ServiceVisitViewModel DeepClone<ServiceVisitViewModel>(ServiceVisitViewModel obj)
        //{
        //    using (var ms = new MemoryStream())
        //    {
        //        var formatter = new BinaryFormatter();
        //        formatter.Serialize(ms, obj);
        //        ms.Position = 0;

        //        return (ServiceVisitViewModel)formatter.Deserialize(ms);
        //    }
        //}
        #endregion

    }
}
