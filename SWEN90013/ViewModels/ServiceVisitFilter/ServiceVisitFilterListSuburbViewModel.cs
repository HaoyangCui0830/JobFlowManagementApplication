using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SWEN90013.ViewModels
{
    public class ServiceVisitFilterListSuburbViewModel : INotifyPropertyChanged
    {
        private String _searchedTerm;
        private List<ServiceVisitFilterSuburbViewModel> _suburbs;
        public List<ServiceVisitFilterSuburbViewModel> AllSuburbs;
        private List<String> SelectedSuburbs;

        public event PropertyChangedEventHandler PropertyChanged;

        public ServiceVisitFilterListSuburbViewModel(List<String> siteSuburbs, List<String> selectedSuburbs)
        {
            this.SelectedSuburbs = selectedSuburbs;
            this.InitializeSuburb(siteSuburbs);
        }

        //term that the user wants to search service visits by
        public string SearchedTerm
        {
            get { return _searchedTerm; }
            set
            {
                _searchedTerm = value;
                OnPropertyChanged(nameof(SearchedTerm));
                UpdateSuburbResult();
            }
        }

        // Populate the suburbs data
        public void InitializeSuburb(List<String> siteSuburbs)
        {
            Suburbs = new List<ServiceVisitFilterSuburbViewModel>();
            
            siteSuburbs.ForEach((item) =>
            {
                var isInserted = false;
                Suburbs.ForEach((suburbItem) =>
                {
                    var suburbViewModel = (ServiceVisitFilterSuburbViewModel)suburbItem;
                    if (item.Equals(suburbViewModel.SuburbName))
                    {
                        isInserted = true;
                    }
                });

                // to avoid inserting the same suburb more than once
                if (!isInserted)
                {
                    var newSuburb = new ServiceVisitFilterSuburbViewModel();
                    newSuburb.IsSelected = false;
                    newSuburb.SuburbName = (String)item;

                    Suburbs.Add(newSuburb);
                }
            });

            foreach(String selectedSuburb in this.SelectedSuburbs)
            {
                foreach (ServiceVisitFilterSuburbViewModel suburb in this.Suburbs)
                {
                    if(suburb.SuburbName.Equals(selectedSuburb)) {
                        suburb.IsSelected = true;
                    }
                }
            }

            this.AllSuburbs = new List<ServiceVisitFilterSuburbViewModel>();
            this.Suburbs.ForEach((item) =>
            {
                this.AllSuburbs.Add(item);
            });
        }

        public List<ServiceVisitFilterSuburbViewModel> Suburbs
        {
            get { return _suburbs; }
            set
            {

                _suburbs = value;

                OnPropertyChanged(nameof(Suburbs));
            }
        }

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        // To be called when typing the suburb name on search bar
        public void UpdateSuburbResult()
        {
            this.Suburbs = new List<ServiceVisitFilterSuburbViewModel>();
            if (!String.IsNullOrWhiteSpace(_searchedTerm))
            {
                this.Suburbs = new List<ServiceVisitFilterSuburbViewModel>();
                this.AllSuburbs.ForEach((item) =>
                {
                    ServiceVisitFilterSuburbViewModel suburb = item;
                    if (suburb.SuburbName.Contains(_searchedTerm))
                    {
                        this.Suburbs.Add(suburb);
                    }
                });
            }
            else
            {
                this.AllSuburbs.ForEach((item) =>
                {
                    ServiceVisitFilterSuburbViewModel suburb = item;
                    this.Suburbs.Add(suburb);
                });
            }
        }

        // To be called when clicking on each suburb
        public void SelectSuburb(String suburb)
        {
            if (suburb.Equals("All"))
            {
                this.AllSuburbs.ForEach((suburbItem) =>
                {
                    var suburbViewModel = (ServiceVisitFilterSuburbViewModel)suburbItem;
                    if (suburb.Equals(suburbViewModel.SuburbName))
                    {
                        suburbItem.IsSelected = true;
                    }
                    else
                    {
                        suburbItem.IsSelected = false;
                    }
                });
            }
            else
            {
                this.AllSuburbs.ForEach((suburbItem) =>
                {
                    var suburbViewModel = (ServiceVisitFilterSuburbViewModel)suburbItem;
                    if (suburb.Equals(suburbViewModel.SuburbName))
                    {
                        suburbItem.IsSelected = !suburbItem.IsSelected;
                    }
                    if (suburbItem.SuburbName.Equals("All"))
                    {
                        suburbItem.IsSelected = false;
                    }
                });
            }
            UpdateSuburbResult();
        }
    }
}