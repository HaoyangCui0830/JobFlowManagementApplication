using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using SWEN90013.ViewModels.TaskChecklist;
using Xamarin.Forms;

namespace SWEN90013.Views.TaskPages
{
    public partial class TaskTestComment : ContentPage
    {
        TestCommentPageViewModel viewModel;
        public TaskTestComment(int serviceVisitItemNumber)
        {
            viewModel = new TestCommentPageViewModel(serviceVisitItemNumber);
            Title = "Add Comment";
            InitializeComponent();
            BindingContext = viewModel;
        }

        /// <summary>
        /// This method will display a modal to let user choose whether to uupload a new picture or choose from library
        /// this method is triggered when the user clicks on the comment's image
        /// </summary>
        /// <param name="sender">object that send the event</param>
        /// <param name="e">event argument</param>
        async private void UploadCommentPic_Button_Clicked(object sender, EventArgs e)
        {
            var actionSheet = await DisplayActionSheet("Please select an image source", "Cancel", null, "Take photo", "Photo Library");

            switch (actionSheet)
            {
                case "Cancel":
                    break;
                case "Take photo":
                    await TakeAPhoto();
                    break;
                case "Photo Library":
                    await ChooseOneFromLibrary();
                    break;
            }
        }

        /// <summary>
        /// This method will let user take a new photo and retrieve the taken picture
        /// </summary>
        async private Task TakeAPhoto()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var mediaOptions = new StoreCameraMediaOptions()
            {
                Directory = "FES",
                SaveToAlbum = true,
                DefaultCamera = CameraDevice.Rear
            };

            var file = await CrossMedia.Current.TakePhotoAsync(mediaOptions);

            if (file == null)
                return;

            viewModel.UpdateImageCommand.Execute(file);
        }

        /// <summary>
        /// This method will let user choose an existing picture from the library
        /// </summary>
        async private Task ChooseOneFromLibrary()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }

            var mediaOptions = new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Medium
            };

            var file = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

            if (file != null)
            {
                viewModel.UpdateImageCommand.Execute(file);
            }
        }
    }
}
