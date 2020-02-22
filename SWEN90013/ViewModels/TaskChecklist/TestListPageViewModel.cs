using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using SWEN90013.Enums;
using SWEN90013.ServicesHandler;
using SWEN90013.Views.TaskPages;
using Xamarin.Forms;

namespace SWEN90013.ViewModels.TaskChecklist
{
    public class TestListPageViewModel : INotifyPropertyChanged
    {
        #region Constructor
        public TestListPageViewModel()
        {
            _taskServices = new TaskServices();
            SaveTaskTestCommand = new Command<Task>(async (Task task) => await SaveTaskTest());
        }

        public TestListPageViewModel(List<CheckItemViewModel> checkItems, List<CommentViewModel> comments, TaskResultStatus? lastResult, TaskResultStatus thisResult, int serviceVisitId, int serviceVisitItemNumber)
        {
            CheckItems = new ObservableCollection<CheckItemViewModel>();
            Comments = new ObservableCollection<CommentViewModel>(comments);
            LastResult = lastResult;
            ThisResult = thisResult;
            _serviceVisitId = serviceVisitId;
            _serviceVisitItemNumber = serviceVisitItemNumber;

            //need to create each one so that if user click back without saving, the changes are not saved
            foreach (CheckItemViewModel item in checkItems)
            {
                CheckItemViewModel tempItem = new CheckItemViewModel();
                tempItem.Id = item.Id;
                tempItem.StepNumber = item.StepNumber;
                tempItem.Description = item.Description;
                tempItem.Status = item.Status;
                tempItem.Comment = item.Comment;
                tempItem.Fields = new ObservableCollection<CheckItemFieldViewModel>();
                if(item.Fields != null)
                {
                    foreach (CheckItemFieldViewModel field in item.Fields)
                    {
                        CheckItemFieldViewModel tempField = new CheckItemFieldViewModel();
                        tempField.Description = field.Description;
                        tempField.Value = field.Value;
                        tempField.FieldType = field.FieldType;
                        tempItem.Fields.Add(tempField);
                    }
                }
                CheckItems.Add(tempItem);
            }

            _taskServices = new TaskServices();

            //instantiate commands
            NavigateToAddCommentCommand = new Command(NavigateToAddComment);
            ChangePassStatusCommand = new Command(ChangePassStatus);
            ChangeFailStatusCommand = new Command(ChangeFailStatus);
            SaveTaskTestCommand = new Command<Task>(async (Task task) => await SaveTaskTest());
        }
        #endregion

        #region Variables
        private ObservableCollection<CheckItemViewModel> _checkItems;
        private ObservableCollection<CommentViewModel> _comments;
        private TaskResultStatus? _lastResult;
        private TaskResultStatus _thisResult;
        private int _serviceVisitId;
        private int _serviceVisitItemNumber;
        private TaskServices _taskServices;
        #endregion

        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<CheckItemViewModel> CheckItems
        {
            get { return _checkItems; }
            set
            {
                _checkItems = value;
                OnPropertyChanged(nameof(CheckItems));
            }
        }

        public ObservableCollection<CommentViewModel> Comments
        {
            get { return _comments; }
            set
            {
                _comments = value;
                OnPropertyChanged(nameof(Comments));
            }
        }

        public TaskResultStatus? LastResult
        {
            get { return _lastResult; }
            set
            {
                _lastResult = value;
                OnPropertyChanged(nameof(LastResult));
            }
        }

        public TaskResultStatus ThisResult
        {
            get { return _thisResult; }
            set
            {
                _thisResult = value;
                OnPropertyChanged(nameof(ThisResult));
                OnPropertyChanged(nameof(TestPassed));
                OnPropertyChanged(nameof(TestFailed));
            }
        }

        public Boolean TestPassed
        {
            get
            {
                return ThisResult == TaskResultStatus.Passed;
            }
        }

        public Boolean TestFailed
        {
            get
            {
                return ThisResult == TaskResultStatus.Failed;
            }
        }

        public String LastResultText
        {
            get { return _lastResult.HasValue? _lastResult.Value.GetDescription() : "N/A"; }
        }

        public String LastResultColor
        {
            get { return _lastResult.HasValue? _lastResult.Value.GetColor() : "White"; }
        }
        #endregion

        #region Commands
        public ICommand NavigateToAddCommentCommand { get; set; }
        public ICommand ChangePassStatusCommand { get; set; }
        public ICommand ChangeFailStatusCommand { get; set; }
        public ICommand SaveTaskTestCommand { get; set; }
        #endregion

        #region Methods
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// This method navigate the user to the new comment page for task
        /// </summary>
        public void NavigateToAddComment()
        {
            Application.Current.MainPage.Navigation.PushAsync(new TaskTestComment(_serviceVisitItemNumber));
        }

        /// <summary>
        /// This method updates the check item status based on user input on the pass check box
        /// </summary>
        /// <param name="obj">The checkbox object that has been changed</param>
        public void ChangePassStatus(Object obj)
        {
            var arg = (Plugin.InputKit.Shared.Controls.CheckBox)obj;
            bool isChecked = arg.IsChecked;

            if (isChecked)
            {
                ThisResult = TaskResultStatus.Passed;
            }
            else
            {
                if (!TestFailed)
                {
                    ThisResult = TaskResultStatus.NoTest;
                }
            }
        }

        /// <summary>
        /// This method updates the check item status based on user input on the fail check box
        /// </summary>
        /// <param name="obj">The checkbox object that has been changed</param>
        public void ChangeFailStatus(Object obj)
        {
            var arg = (Plugin.InputKit.Shared.Controls.CheckBox)obj;
            bool isChecked = arg.IsChecked;

            if (isChecked)
            {
                ThisResult = TaskResultStatus.Failed;
            }
            else
            {
                if (!TestPassed)
                {
                    ThisResult = TaskResultStatus.NoTest;
                }
            }
        }

        /// <summary>
        /// This method update the comment for a specified check box
        /// </summary>
        /// <param name="id">check item id</param>
        /// <param name="comment">updated comment</param>
        public void UpdateCheckItemComment(int id, CommentViewModel comment)
        {
            var targetCheckItem = CheckItems.Where(c => c.Id == id).FirstOrDefault();
            if(targetCheckItem != null)
            {
                targetCheckItem.Comment = comment;
            }
        }

        /// <summary>
        /// This method add a new comment for the task
        /// </summary>
        /// <param name="comment">new comment</param>
        public void AddTestComment(CommentViewModel comment)
        {
            Comments.Add(comment);
        }

        public async Task SaveTaskTest()
        {
            var result = await _taskServices.SendResult(_serviceVisitId, _serviceVisitItemNumber, ThisResult, CheckItems.ToList());
            if(result)
            {
                await Application.Current.MainPage.DisplayAlert("Save successful", "The test result has been successfully saved", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Save failed", "An error occured when trying to save the test result. Plesae try again", "OK");
            }
        }
        #endregion
    }
}
