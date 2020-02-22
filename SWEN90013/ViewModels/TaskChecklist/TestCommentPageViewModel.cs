using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Media.Abstractions;
using SWEN90013.ServicesHandler;
using Xamarin.Forms;

namespace SWEN90013.ViewModels.TaskChecklist
{
    public class TestCommentPageViewModel : INotifyPropertyChanged
    {
        #region Constructors
        public TestCommentPageViewModel()
        {
            ImageUrl = "upload_image";
            AddCommentCommand = new Command<Task>(async (Task task) => await AddComment());
            UpdateImageCommand = new Command<MediaFile>(async (MediaFile NewImage) => await UpdateImageCommandActAsync(NewImage));
            taskService = new TaskServices();
            mediaServices = new MediaServices();
            IsBusy = false;
        }

        public TestCommentPageViewModel(int serviceVisitItemNumber)
        {
			_serviceVisitItemNumber = serviceVisitItemNumber;
            ImageUrl = "upload_image";
            AddCommentCommand = new Command<Task>(async (Task task) => await AddComment());
            UpdateImageCommand = new Command<MediaFile>(async (MediaFile NewImage) => await UpdateImageCommandActAsync(NewImage));
            taskService = new TaskServices();
            mediaServices = new MediaServices();
            IsBusy = false;
        }
        #endregion

        #region variables
        private string _comment;
        private string _imageUrl;
        private int _serviceVisitItemNumber;
        private TaskServices taskService;
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
        public ICommand AddCommentCommand { get; set; }
        public ICommand UpdateImageCommand { get; set; }
        #endregion

        #region Methods
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// This method will first signal the test list page about the new comment and then redirect the user back to test list page
        /// </summary>
        async Task AddComment()
        {
            //do not let the user save when it is currently uploading an image
            if(IsBusy)
            {
                return;
            }
			//make sure the comment is not empty and the user have uploaded an image
			if (String.IsNullOrWhiteSpace(Comment) || ImageUrl == "upload_image")
            {
                await Application.Current.MainPage.DisplayAlert("Reminder", "Please make sure that you have uploaded an image and insert some description", "OK");
                return;
            }
            CommentViewModel newComment = new CommentViewModel(ImageUrl, Comment);
            var result = await taskService.AddDefectReport(_serviceVisitItemNumber, newComment);
            if(result)
            {
                MessagingCenter.Send<TestCommentPageViewModel, CommentViewModel>(this, "AddTestComment", newComment);
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Something Went Wrong", "An error occurs when trying to add the comment. Please try again", "OK");
            }
        }

        /// <summary>
        /// This method will receive the new image file and update the display
        /// </summary>
        /// <param name="newImage">the new image file</param>
        async Task UpdateImageCommandActAsync(MediaFile newImage)
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
