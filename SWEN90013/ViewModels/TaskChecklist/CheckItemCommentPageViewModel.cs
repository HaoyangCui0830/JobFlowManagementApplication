using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Media.Abstractions;
using SWEN90013.ServicesHandler;
using SWEN90013.Views.TaskPages;
using Xamarin.Forms;

namespace SWEN90013.ViewModels.TaskChecklist
{
    public class CheckItemCommentPageViewModel : INotifyPropertyChanged
    {
        #region Constructors
        public CheckItemCommentPageViewModel()
        {
            UpdateCommentCommand = new Command(UpdateComment);
            UpdateImageCommand = new Command<MediaFile>((MediaFile NewImage) => UpdateImageCommandAct(NewImage));
            mediaServices = new MediaServices();
            IsBusy = false;
        }

        public CheckItemCommentPageViewModel(int id, CommentViewModel model)
        {
            _id = id;
            ImageUrl = !String.IsNullOrWhiteSpace(model.ImageUrl) ? model.ImageUrl : "upload_image";
            Comment = model.Description;
            UpdateCommentCommand = new Command(UpdateComment);
            UpdateImageCommand = new Command<MediaFile>((MediaFile NewImage) => UpdateImageCommandAct(NewImage));
            mediaServices = new MediaServices();
            IsBusy = false;
        }
        #endregion

        #region variables
        private int _id;
        private string _comment;
        private string _imageUrl;
        private MediaServices mediaServices;
        private bool _isBusy;
        #endregion

        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;

        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                _imageUrl = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public string Comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands
        public ICommand UpdateCommentCommand { get; set; }
        public ICommand UpdateImageCommand { get; set; }
        #endregion

        #region Methods
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// This method will first signal the test list page about the updated comment and then redirect the user back to test list page
        /// </summary>
        void UpdateComment()
        {
            //do not let the user save when it is currently uploading an image
            if (IsBusy)
            {
                return;
            }
            //make sure the comment is not empty and the user have uploaded an image
            if (String.IsNullOrWhiteSpace(Comment) || ImageUrl == "upload_image")
            {
                Application.Current.MainPage.DisplayAlert("Reminder", "Please make sure that you have uploaded an image and insert some description", "OK");
                return;
            }
            CommentViewModel updatedComment = new CommentViewModel(ImageUrl, Comment);
            MessagingCenter.Send<CheckItemCommentPageViewModel, Tuple<int, CommentViewModel>>(this, "UpdateCheckItemComment", new Tuple<int, CommentViewModel>(_id, updatedComment));
            Application.Current.MainPage.Navigation.PopAsync();
        }

        /// <summary>
        /// This method will receive the new image file and update the display
        /// </summary>
        /// <param name="newImage">The new image for this comment</param>
        async Task UpdateImageCommandAct(MediaFile newImage)
        {
            IsBusy = true;
            string newUrl = await mediaServices.UploadImage(newImage);
            if (!String.IsNullOrEmpty(newUrl))
            {
                ImageUrl = newUrl;
                IsBusy = false;
            }
            else
            {
                IsBusy = false;
                _ = Application.Current.MainPage.DisplayAlert("Something Went Wrong", "An error occurs when trying to uplodad the new image. Please try again", "OK");
            }
        }
        #endregion
    }
}
