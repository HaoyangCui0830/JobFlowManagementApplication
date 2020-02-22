using System;
using System.Collections.Generic;
using SWEN90013.Enums;

namespace SWEN90013.ViewModels.TaskChecklist
{
    public class ChecklistViewModel
    {
        #region Constructors
        public ChecklistViewModel()
        {
        }
        #endregion

        #region variables

        private List<CheckItemViewModel> _checkItems;

        #endregion

        #region Properties
        public List<CheckItemViewModel> CheckItems
        {
            get { return _checkItems; }
            set
            {
                _checkItems = value;
            }
        }
        #endregion
    }
}
