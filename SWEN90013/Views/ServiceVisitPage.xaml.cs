using Plugin.Toasts;
using SWEN90013.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace SWEN90013.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServiceVisitPage : ContentPage
    {
        ServiceVisitPageViewModel viewModel;

        public ServiceVisitViewModel serviceVisit; 

        /// <summary>
        /// This is the constructor for ServiceVisitPage
        /// </summary>
        /// <param name="selectedVisit">The service visit is currently showing</param>
        public ServiceVisitPage(ServiceVisitViewModel selectedVisit)
        {
            InitializeComponent();

            BindingContext = viewModel = new ServiceVisitPageViewModel();
            viewModel.ServiceVisit = selectedVisit;
            
            // initialise maps
            var center = new Position(selectedVisit.PositionLat, selectedVisit.PostionLong);
            Maps.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    center, Distance.FromMiles(1)));
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = center,
                Label = "Address",
                Address = selectedVisit.FullAddress
            };
            pin.Clicked += Pin_Clicked;
            Maps.Pins.Add(pin);

            // subscribe message
            MessagingCenter.Subscribe<ServiceVisitPageViewModel>(this, "updateFailedNotify", async (sender) =>
            {
                await DisplayAlert("Alert", "Update failed, please try again later.", "OK");
            });
        }

        /// <summary>
        /// This is used to handle when a Pin in map has been clicked
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">the event arguments</param>
        private void Pin_Clicked(object sender, EventArgs e)
        {
            viewModel.MapCommand.Execute("");
        }

        /// <summary>
        /// This is used to handle when the value of segControl has been changed
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">the event arguments</param>
        private void SegControl_ValueChanged(object sender, SegmentedControl.FormsPlugin.Abstractions.ValueChangedEventArgs e)
        {
            int newVal = e.NewValue;

            viewModel.SegControlCommand.Execute(newVal);
        }

        /// <summary>
        /// This is used to handle when clicked go to tast list button
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">the event argument</param>
        private void GoToTaskList_Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TaskListPage(viewModel.ServiceVisit.Id));
        }

        /// <summary>
        /// This is used to handle when Upload Site Pic button has been clicked
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">the event argument</param>
        async private void UploadSitePic_Button_Clicked(object sender, EventArgs e)
        {
            var actionSheet = await DisplayActionSheet("Please select an image source", "Cancel", null, "Take photo", "Photo Library");

            switch (actionSheet)
            {
                case "Cancel":
                    break;
                case "Take photo":
                    TakeAPhoto();
                    break;
                case "Photo Library":
                    ChooseOneFromLibrary();
                    break;
            }
        }

        /// <summary>
        /// This is used to call the function of taking a phone
        /// </summary>
        async private void TakeAPhoto()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "FES",
                SaveToAlbum = true,
                DefaultCamera = CameraDevice.Rear
            });

            if (file == null)
                return;

            viewModel.UpdateImageCommand.Execute(file);
        }

        /// <summary>
        /// This is used to call the function of choosing a phone form library
        /// </summary>
        async private void ChooseOneFromLibrary()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }

            var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
            });

            if (file == null)
                return;

            viewModel.UpdateImageCommand.Execute(file);
        }
    }
}