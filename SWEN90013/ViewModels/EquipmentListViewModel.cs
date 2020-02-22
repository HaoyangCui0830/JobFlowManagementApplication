using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using SWEN90013.Models;

namespace SWEN90013.ViewModels
{
    public class EquipmentListViewModel : INotifyPropertyChanged
    {
        public EquipmentListViewModel()
        {
            GetEquipmentList();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        

        private String _searchEquipmentTxt;
        public String SearchEquipmentTxt
        {
            get { return _searchEquipmentTxt; }
            set
            {
                if (_searchEquipmentTxt == value)
                    return;
                _searchEquipmentTxt = value;
                OnPropertyChanged();
                UpdateEquipmentList();
            }
        }

        private IList<String> _fullEquipmentList;
        public IList<String> FullEquipmentList
        {
            get { return _fullEquipmentList; }
            set
            {
                if (_fullEquipmentList == value)
                    return;
                _fullEquipmentList = value;
                OnPropertyChanged();
            }
        }

        private IList<String> _fullEquipmentIDList;
        public IList<String> FullEquipmentIDList
        {
            get { return _fullEquipmentIDList; }
            set
            {
                if (_fullEquipmentIDList == value)
                    return;
                _fullEquipmentIDList = value;
                OnPropertyChanged();
            }
        }

        private IList<String> _showingEquipmentList;
        public IList<String> ShowingEquipmentList
        {
            get { return _showingEquipmentList; }
            set
            {
                if (_showingEquipmentList == value)
                    return;
                _showingEquipmentList = value;
                OnPropertyChanged();
            }
        }

        public void UpdateEquipmentList()
        {
            ShowingEquipmentList = new List<String>();
            if (SearchEquipmentTxt == null)
            {
                for (int i = 0, len = FullEquipmentList.Count; i < len; ++i)
                {
                    ShowingEquipmentList.Add(FullEquipmentList[i]);
                }
            }
            else if (SearchEquipmentTxt.Length == 0)
            {
                for (int i = 0, len = FullEquipmentList.Count; i < len; ++i)
                {
                    ShowingEquipmentList.Add(FullEquipmentList[i]);
                }
            }
            else
            {
                for (int i = 0, len = FullEquipmentList.Count; i < len; ++i)
                {
                    if (FullEquipmentList[i].Contains(SearchEquipmentTxt))
                    {
                        ShowingEquipmentList.Add(FullEquipmentList[i]);
                    }
                }
            }

            OnPropertyChanged(nameof(ShowingEquipmentList));
        }

        private List<EquipmentInformation> allEquipmentInformation = new List<EquipmentInformation>();

        /// <summary>
        /// This function is used to get the contractor list from server
        /// </summary>
        private void GetEquipmentList()
        {

            string json = File.ReadAllText("Equipment.json");
            _fullEquipmentList = new List<String>();
            _fullEquipmentIDList = new List<String>();
            
            allEquipmentInformation = JsonConvert.DeserializeObject<List<EquipmentInformation>>(json);
            if (allEquipmentInformation != null)
            {
                foreach (EquipmentInformation eqInfo in allEquipmentInformation)
                {
                    _fullEquipmentList.Add(eqInfo.Description);
                    _fullEquipmentIDList.Add(eqInfo.EquipmentTypeID);
                }
            }
            
            ShowingEquipmentList = _fullEquipmentList;
        }

        /// <summary>
        /// Get equipment ID by its description
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public String FindEquipmentIDByDescription(String description)
        {
            String equipmentID = "";
            foreach (EquipmentInformation eqInfo in allEquipmentInformation)
            {
                if (eqInfo.Description.Equals(description))
                {
                    return eqInfo.EquipmentTypeID;
                }
            }
            return equipmentID;
        }

        /// <summary>
        /// Get equipment name by its name for display
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public String FindEquipmentDescriptionByID(String id)
        {
            String equipmentDescription = "";
            foreach (EquipmentInformation eqInfo in allEquipmentInformation)
            {
                if (eqInfo.EquipmentTypeID.Equals(id))
                {
                    return eqInfo.Description;
                }
            }
            return equipmentDescription;
        }

    }
}
