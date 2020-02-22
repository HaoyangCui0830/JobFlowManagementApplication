using System;
using SWEN90013.ViewModels;

namespace SWEN90013.Models
{
    public class DefectReport
    {
        #region constructor
        public DefectReport()
        {
        }

        public DefectReport(CommentViewModel comment, String author)
        {
            this.comment = comment.Description;
            this.photoURL = comment.ImageUrl;
            this.author = author;
        }
        #endregion

        #region properties
        public String comment { get; set; }
        public String photoURL { get; set; }
        public String author { get; set; }
        #endregion
    }
}
