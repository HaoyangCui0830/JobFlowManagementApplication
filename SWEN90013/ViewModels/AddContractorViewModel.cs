using SWEN90013.ServicesHandler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SWEN90013.ViewModels
{
    public class AddContractorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private TaskServices _taskService = new TaskServices();
        public INavigation Navigation { get; set; }
        public AddContractorViewModel()
        {
            AddContractorCommand = new Command(async () => await AddContractorCommandAct());
        }

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
        private String _contractorName;
        public String ContractorName
        {
            get { return _contractorName; }
            set
            {
                if (_contractorName == value)
                    return;
                _contractorName = value;
                OnPropertyChanged();
            }
        }

        private IList<String> _existContractorList = new List<String>();
        public IList<String> ExistContractorList
        {
            get { return _existContractorList; }
            set
            {
                if (_existContractorList == value)
                    return;
                _existContractorList = value;
                OnPropertyChanged();
            }
        }
        public ICommand AddContractorCommand { get; set; }
        async Task AddContractorCommandAct()
        {
            // prevent empty name
            if (ContractorName.Length == 0)
            {
                MessagingCenter.Send(this, "emptyContractor");
                return;
            }
            // prevend same name
            for (int i = 0, len = ExistContractorList.Count; i < len; ++i)
            {
                if (ContractorName.Equals(ExistContractorList[i]))
                {
                    MessagingCenter.Send(this, "duplicateContractor");
                    return;
                }
            }

            if (IsBusy) return;
            IsBusy = true;
            
            var isSucceed = await _taskService.AddNewContractor(ContractorName);

            IsBusy = false;

            if (isSucceed)
            {
                MessagingCenter.Send<AddContractorViewModel, String>(this, "AddContractor", ContractorName);
                await Navigation.PopAsync();
            }
            else
            {
                MessagingCenter.Send(this, "AddContractorFailed");
            }
        }
    }
}
