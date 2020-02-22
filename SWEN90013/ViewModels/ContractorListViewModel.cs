using SWEN90013.ServicesHandler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SWEN90013.ViewModels
{
    public class ContractorListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private TaskServices _taskService = new TaskServices();
        private bool _isBusy = false;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value)
                    return;
                _isBusy = value;
                OnPropertyChanged();
            }
        }
        private String _searchCompanyTxt;
        public String SearchCompanyTxt
        {
            get { return _searchCompanyTxt; }
            set
            {
                if (_searchCompanyTxt == value)
                    return;
                _searchCompanyTxt = value;
                OnPropertyChanged();
                UpdateContractorList();
            }
        }

        private IList<String> _fullCompanyList;
        public IList<string> FullCompanyList
        {
            get { return _fullCompanyList; }
            set
            {
                if (_fullCompanyList == value)
                    return;
                _fullCompanyList = value;
                OnPropertyChanged();
            }
        }

        private IList<String> _showingCompanyList;
        public IList<String> ShowingCompanyList
        {
            get { return _showingCompanyList; }
            set
            {
                if (_showingCompanyList == value)
                    return;
                _showingCompanyList = value;
                OnPropertyChanged();
            }
        }
 
        public ContractorListViewModel()
        {
            GetCompanyList();
        }

        public void UpdateContractorList()
        {
            ShowingCompanyList = new List<String>();
            if (SearchCompanyTxt == null)
            {
                for (int i = 0, len = FullCompanyList.Count; i < len; ++i)
                {
                    ShowingCompanyList.Add(FullCompanyList[i]);
                }
            }
            else if (SearchCompanyTxt.Length == 0)
            {
                for (int i = 0, len = FullCompanyList.Count; i < len; ++i)
                {
                    ShowingCompanyList.Add(FullCompanyList[i]);
                }
            }
            else
            {
                for (int i = 0, len = FullCompanyList.Count; i < len; ++i)
                {
                    if (FullCompanyList[i].Contains(SearchCompanyTxt))
                    {
                        ShowingCompanyList.Add(FullCompanyList[i]);
                    }
                }
            }

            OnPropertyChanged(nameof(ShowingCompanyList));
        }

        /// <summary>
        /// This function is used to get the contractor list from server
        /// </summary>
        private async void GetCompanyList()
        {
            IsBusy = true;
            var contractorLs = await _taskService.GetAllContractorList();
            
            if (contractorLs == null)
            {
                MessagingCenter.Send<ContractorListViewModel>(this, "GetContractorListFailed");
                IsBusy = true;
                return;
            }

            _fullCompanyList = new List<String>();
            for (int i = 0, len = contractorLs.Count; i < len; ++i)
            {
                _fullCompanyList.Add(contractorLs[i].contractorName);
            }

            ShowingCompanyList = _fullCompanyList;
            IsBusy = false;
        }

        /// <summary>
        /// This is used to refresh the contractor list, when adding a new contractor
        /// </summary>
        /// <param name="newContractor">name of the new contractor</param>
        public void addNewContractor(String newContractor)
        {
            // empty the search
            FullCompanyList.Add(newContractor);
            SearchCompanyTxt = "";
            UpdateContractorList();
        }
    }
}
