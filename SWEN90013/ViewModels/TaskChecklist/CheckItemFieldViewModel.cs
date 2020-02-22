using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SWEN90013.Models;

namespace SWEN90013.ViewModels.TaskChecklist
{
    public class CheckItemFieldViewModel : INotifyPropertyChanged
    {
        #region Constuctors
        public CheckItemFieldViewModel()
        {
        }

        public CheckItemFieldViewModel(UnitEntryValue entryValue)
        {
			Id = entryValue.Id;
            Description = entryValue.ValueName;
            FieldType = entryValue.Unit;
            Value = entryValue.Value;
        }

		#endregion

		#region Variables
		private int _id;
        private String _fieldType;
        private String _description;
        private String _value;
        #endregion

        #region Properties

		public int Id
		{
			get { return _id; }
			set
			{
				_id = value;
				OnPropertyChanged(nameof(FieldType));
			}
		}

		public String FieldType
        {
            get { return _fieldType; }
            set
            {
                _fieldType = value;
                OnPropertyChanged(nameof(FieldType));
            }
        }

        public String Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public String Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Methods

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
