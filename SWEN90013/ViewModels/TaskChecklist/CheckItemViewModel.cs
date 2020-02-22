using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SWEN90013.Enums;
using SWEN90013.Models;
using SWEN90013.Views.TaskPages;
using Xamarin.Forms;

namespace SWEN90013.ViewModels.TaskChecklist
{
    public class CheckItemViewModel : INotifyPropertyChanged
    {
        #region Constructor
        public CheckItemViewModel()
        {
            ChangePassStatusCommand = new Command(ChangePassStatus);
            ChangeFailStatusCommand = new Command(ChangeFailStatus);
            UpdateCommentCommand = new Command(UpdateComment);
        }

        public CheckItemViewModel(CheckItem checkItem)
        {
            Id = checkItem.Id;
            SiteEquipmentLineNumber = checkItem.SiteEquipmentLineNumber;
            Description = checkItem.Description;
            Comment = new CommentViewModel(checkItem.PhotoURL, checkItem.Comment);
			Status = checkItem.Status;
            if(checkItem.UnitEntryValue != null)
            {
                Fields = new ObservableCollection<CheckItemFieldViewModel>();
                foreach (UnitEntryValue entry in checkItem.UnitEntryValue)
                {
                    Fields.Add(new CheckItemFieldViewModel(entry));
                }
            }
        }

        public CheckItemViewModel(int id, int stepNumber, string description, ChecklistStatus status, CommentViewModel comment, ObservableCollection<CheckItemFieldViewModel> fields)
        {
            Id = id;
            StepNumber = stepNumber;
            Description = description;
            Status = status;
            Comment = comment;
            Fields = fields;
            ChangePassStatusCommand = new Command(ChangePassStatus);
            ChangeFailStatusCommand = new Command(ChangeFailStatus);
            UpdateCommentCommand = new Command(UpdateComment);
        }
        #endregion

        #region Variables
        private int _id;
        private int _siteEquipmentLineNumber;
        private int _stepNumber;
        private string _description;
        private ChecklistStatus _status;
        private CommentViewModel _comment;
        private ObservableCollection<CheckItemFieldViewModel> _fields;
        #endregion

        #region Properties
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }

        public int SiteEquipmentLineNumber
        {
            get { return _siteEquipmentLineNumber; }
            set
            {
                _siteEquipmentLineNumber = value;
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public ChecklistStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(ChecklistPassed));
                OnPropertyChanged(nameof(ChecklistFailed));
            }
        }

        public CommentViewModel Comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(CommentExist));
                OnPropertyChanged(nameof(Comment));
            }
        }

        public ObservableCollection<CheckItemFieldViewModel> Fields
        {
            get { return _fields; }
            set
            {
                _fields = value;
                OnPropertyChanged(nameof(Fields));
            }
        }

        public int StepNumber
        {
            get { return _stepNumber; }
            set
            {
                _stepNumber = value;
                OnPropertyChanged(nameof(StepNumber));
            }
        }

        public Boolean CommentExist
        {
            get
            {
				return (Comment != null) && (!String.IsNullOrEmpty(Comment.Description)) && (!String.IsNullOrEmpty(Comment.ImageUrl));
            }
        }

        public Boolean ChecklistPassed
        {
            get
            {
                return Status == ChecklistStatus.Passed;
            }
        }

        public Boolean ChecklistFailed
        {
            get
            {
                return Status == ChecklistStatus.Failed;
            }
        }

        public Boolean ContainFields
        {
            get
            {
                return Fields != null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Commands
        public ICommand ChangePassStatusCommand { get; set; }
        public ICommand ChangeFailStatusCommand { get; set; }
        public ICommand UpdateCommentCommand { get; set; }
        #endregion

        #region Methods
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// This method updates the check item status based on user input on the pass check box
        /// </summary>
        /// <param name="obj">The checkbox object</param>
        public void ChangePassStatus(object obj)
        {
            var arg = (Plugin.InputKit.Shared.Controls.CheckBox)obj;
            bool isChecked = arg.IsChecked;

            if (isChecked)
            {
                Status = ChecklistStatus.Passed;
            }
            else
            {
                if (!ChecklistFailed)
                {
                    Status = ChecklistStatus.NoTest;
                }
            }
        }

        /// <summary>
        /// This method updates the check item status based on user input on the fail check box
        /// </summary>
        /// <param name="obj">The checkbox object</param>
        public void ChangeFailStatus(object obj)
        {
            var arg = (Plugin.InputKit.Shared.Controls.CheckBox)obj;
            bool isChecked = arg.IsChecked;

            if (isChecked)
            {
                Status = ChecklistStatus.Failed;
            }
            else
            {
                if (!ChecklistPassed)
                {
                    Status = ChecklistStatus.NoTest;
                }
            }
        }

        /// <summary>
        /// This method navigate the user to the comment page for a particular check item
        /// </summary>
        public void UpdateComment()
        {
            Application.Current.MainPage.Navigation.PushAsync(new TaskCheckItemComment(this.Id, this.Comment));
        }
        #endregion
    }
}
