using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using SWEN90013.ServicesHandler;
using SWEN90013.Models;
using System.Threading.Tasks;
using SWEN90013.Enums;
using Plugin.Media.Abstractions;

namespace SWEN90013.ViewModels
{
    class ServiceVisitPageViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// This is the constructor for ServiceVisitPage ViewModel
        /// </summary>
        public ServiceVisitPageViewModel()
        {

            BuildingDetailsCommand = new Command(async () => await BuildingDetailsCommandAct());
            MemoCommand = new Command(async () => await MemoCommandAct());
            SegControlCommand = new Command<int>(SegControlCommandAct);
            MapCommand = new Command(MapCommandAct);
            UpdateImageCommand = new Command<MediaFile>(async (MediaFile NewImage) => await UpdateImageCommandAct(NewImage));
        }

        private readonly ServiceVisitServices _visitService = new ServiceVisitServices();
        private readonly BuildingDetailsValues _buildingDetailsValues = new BuildingDetailsValues();

        private bool _isShowingInfo = true;
        private bool _isShowingDetails = false;

        private bool _isUpdatingBuildingDetails = false;
        private bool _isUpdatingMemo = false;

        private bool _isDetailsBusy = false;
        private bool _isMemosBusy = false;

        private string _buildingDetailsButtonTxt = "Edit";
        private string _memoButtonTxt = "Edit";
        private string _siteImageUploadButtonTxt = "+ Upload Site Picture";

        public Memos DuplicateMemo { get; set; }
        public int DuplicateBuildingClassIndex { get; set; }
        public int DuplicateBuildingEraIndex { get; set; }
        public int DuplicateBuildingSizeIndex { get; set; }

        private ServiceVisitViewModel _serviceVisit;

        public ServiceVisitViewModel ServiceVisit {
            get => _serviceVisit;
            set
            {
                if (_serviceVisit == value)
                    return;
                _serviceVisit = value;

                DuplicateMemo = new Memos()
                {
                    ContactMemo = _serviceVisit.ContactMemo,
                    FSMMemo = _serviceVisit.FSMMemo,
                    InductionMemo = _serviceVisit.InductionMemo,
                    OHSMemo = _serviceVisit.OHSMemo,
                    ServiceMemo = _serviceVisit.ServiceMemo
                };

                DuplicateBuildingClassIndex = _serviceVisit.BuildingClassIndex;
                DuplicateBuildingEraIndex = _serviceVisit.BuildingEraIndex;
                DuplicateBuildingSizeIndex = _serviceVisit.BuildingSizeIndex;

                OnPropertyChanged();
                OnPropertyChanged(nameof(DuplicateMemo));
                OnPropertyChanged(nameof(DuplicateBuildingClassIndex));
                OnPropertyChanged(nameof(DuplicateBuildingEraIndex));
                OnPropertyChanged(nameof(DuplicateBuildingSizeIndex));
            }
        }

        public bool IsDetailsBusy
        {
            get => _isDetailsBusy;
            set
            {
                if (_isDetailsBusy == value)
                    return;

                _isDetailsBusy = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsUploading));
            }
        }

        public bool IsMemosBusy
        {
            get => _isMemosBusy;
            set
            {
                if (_isMemosBusy == value)
                    return;

                _isMemosBusy = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsUploading));
            }
        }
        public bool IsUploading {
            get => _isDetailsBusy || _isMemosBusy;
        }

        public bool IsShowingInfo {
            get => _isShowingInfo;
            set
            {
                if (_isShowingInfo == value)
                    return;

                _isShowingInfo = value;
                OnPropertyChanged();
            }
        }
        public bool IsShowingDetails
        {
            get => _isShowingDetails;
            set
            {
                if (_isShowingDetails == value)
                    return;

                _isShowingDetails = value;
                OnPropertyChanged();
            }
        }

        public bool IsUpdatingBuildingDetails
        {
            get => _isUpdatingBuildingDetails;
            set
            {
                if (_isUpdatingBuildingDetails == value)
                    return;
                _isUpdatingBuildingDetails = value;
                OnPropertyChanged();
            }
        }

        public bool IsUpdatingMemo
        {
            get => _isUpdatingMemo;
            set
            {
                if (_isUpdatingMemo == value)
                    return;
                _isUpdatingMemo = value;
                OnPropertyChanged();
            }
        }

        public string BuildingDetailsButtonTxt
        {
            get => _buildingDetailsButtonTxt;
            set
            {
                if (_buildingDetailsButtonTxt == value)
                    return;
                _buildingDetailsButtonTxt = value;
                OnPropertyChanged();
            }
        }
        public string MemoButtonTxt
        {
            get => _memoButtonTxt;
            set
            {
                if (_memoButtonTxt == value)
                    return;
                _memoButtonTxt = value;
                OnPropertyChanged();
            }
        }
        public string SiteImageUploadButtonTxt
        {
            get => _siteImageUploadButtonTxt;
            set
            {
                if (_siteImageUploadButtonTxt == value)
                    return;
                _siteImageUploadButtonTxt = value;
                OnPropertyChanged();
            }
        }

        public Command BuildingDetailsCommand { get; set; }
        public Command MemoCommand { get; set; }
        public Command<int> SegControlCommand { get; set; }
        public Command MapCommand { get; set; }
        public Command<MediaFile> UpdateImageCommand { get; set; }
        

        async Task BuildingDetailsCommandAct()
        {
            IsUpdatingBuildingDetails = !IsUpdatingBuildingDetails;

            if (IsUpdatingBuildingDetails)
                BuildingDetailsButtonTxt = "Update";
            else
            {
                IsDetailsBusy = true;
                BuildingDetailsButtonTxt = "Updating...";
                // send the request here
                Building BuildingDetail = new Building()
                {
                    BuildingClass = _buildingDetailsValues.GetBuildingDetailsClassDescription(DuplicateBuildingClassIndex),
                    BuildingEra = _buildingDetailsValues.GetBuildingDetailsEraDesciption(DuplicateBuildingEraIndex),
                    BuildingParts = ServiceVisit.BuildingParts,
                    BuildingSize = _buildingDetailsValues.GetBuildingDetailsSizeDescription(DuplicateBuildingSizeIndex),
                    KeysHeld = ServiceVisit.KeyHeld ? 1 : 0
                };

                var IsSuccessful = await _visitService.UpdateBuildingDetails(ServiceVisit.SiteId, BuildingDetail);

                IsDetailsBusy = false;

                if (IsSuccessful)
                {
                    BuildingDetailsButtonTxt = "Edit";

                    // write back
                    ServiceVisit.BuildingClassIndex = DuplicateBuildingClassIndex;
                    ServiceVisit.BuildingEraIndex = DuplicateBuildingEraIndex;
                    ServiceVisit.BuildingSizeIndex = DuplicateBuildingSizeIndex;
                }
                else
                {
                    // when the request failed.
                    IsUpdatingBuildingDetails = !IsUpdatingBuildingDetails;
                    BuildingDetailsButtonTxt = "Update";
                    MessagingCenter.Send(this, "updateFailedNotify");
                }
            }
                
        }

        async Task MemoCommandAct()
        {
            IsUpdatingMemo = !IsUpdatingMemo;

            if (IsUpdatingMemo)
            {
                MemoButtonTxt = "Update";
            }
            else
            {
                IsMemosBusy = true;
                MemoButtonTxt = "Updating...";

                bool IsSuccessful = await _visitService.UpdateMemo(ServiceVisit.SiteId, ServiceVisit.ServiceVisitNumber, DuplicateMemo);

                IsMemosBusy = false;

                if (IsSuccessful)
                {
                    MemoButtonTxt = "Edit";

                    // Write back
                    ServiceVisit.ServiceMemo = DuplicateMemo.ServiceMemo;
                    ServiceVisit.OHSMemo = DuplicateMemo.OHSMemo;
                    ServiceVisit.FSMMemo = DuplicateMemo.FSMMemo;
                }
                else
                {
                    // when the request failed.
                    IsUpdatingMemo = !IsUpdatingMemo;
                    MemoButtonTxt = "Update";
                    MessagingCenter.Send(this, "updateFailedNotify");
                }
            }
        }
        async Task UpdateImageCommandAct(MediaFile NewImage)
        {
            SiteImageUploadButtonTxt = "Uploading...";

            String newUrl = await _visitService.UpdateSiteImage(ServiceVisit.SiteId, NewImage);

            SiteImageUploadButtonTxt = "+ Upload Site Picture";
            if (newUrl != null)
            {
                ServiceVisit.SiteImageUrl = newUrl;
                OnPropertyChanged(nameof(ServiceVisit));
            }
            else
            {
                MessagingCenter.Send(this, "updateFailedNotify");
            }
        }

        void SegControlCommandAct (int val)
        {
            if (val == 0)
            {
                IsShowingInfo = true;
                IsShowingDetails = false;
            }
            else
            {
                IsShowingInfo = false;
                IsShowingDetails = true;
            }
        }

        async void MapCommandAct ()
        {
            await Map.OpenAsync(ServiceVisit.PositionLat, ServiceVisit.PostionLong,
                new MapLaunchOptions
                {
                    Name = ServiceVisit.FullAddress,
                    NavigationMode = NavigationMode.None
                });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
