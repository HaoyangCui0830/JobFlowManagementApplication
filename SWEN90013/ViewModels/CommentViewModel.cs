using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SWEN90013.Models;

namespace SWEN90013.ViewModels
{
    public class CommentViewModel 
    {
        #region Constructor
        public CommentViewModel()
        {
        }

        public CommentViewModel(string imageUrl, string description)
        {
            ImageUrl = imageUrl;
            Description = description;
        }

        public CommentViewModel(DefectReport defectReport)
        {
            ImageUrl = defectReport.photoURL;
            Description = defectReport.comment;
            Author = defectReport.author;
        }
        #endregion

        #region Variables
        private string _imageUrl;
        private string _description;
        private string _author;
        #endregion

        #region Properties

        public string ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                _imageUrl = value;
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
            }
        }

        public string Author
        {
            get { return _author; }
            set
            {
                _author = value;
            }
        }

        #endregion

    }
}
