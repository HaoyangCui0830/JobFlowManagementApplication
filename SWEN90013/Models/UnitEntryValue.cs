using System;
using SWEN90013.ViewModels.TaskChecklist;

namespace SWEN90013.Models
{
    public class UnitEntryValue
    {
        #region Constructors
        public UnitEntryValue()
        {
        }

        public UnitEntryValue(CheckItemFieldViewModel field)
        {
			Id = field.Id;
            ValueName = field.Description;
            Unit = field.FieldType;
            Value = field.Value;
        }
		#endregion

		#region properties
		public int Id { get; set; }
		public String ValueName { get; set; }
        public String Value { get; set; }
        public String Unit { get; set; }
        #endregion
    }
}
