using System;
using SWEN90013.Models;

namespace SWEN90013.ViewModels
{
    public class TimelineDefectViewModel 
    {
        private String _comment;

        public String Comment
        {
            get
            {
                return _comment;
            }
            set
            {
                _comment = value;
            }
        }

        private String _imageUrl;

        public String ImageUrl
        {
            get
            {
                return _imageUrl;
            }
            set
            {
                _imageUrl = value;
            }
        }

        public TimelineDefectViewModel()
        {

        }

        public TimelineDefectViewModel(DefectReport defect)
        {
            _comment = defect.comment;
            _imageUrl = defect.photoURL;
        }
    }
}
