using System;
using SQLite;
using SWEN90013.Models;

namespace SWEN90013.Data
{
    public class UnitEntryValueDBInfo
    {
        public UnitEntryValueDBInfo(int checkListId, UnitEntryValue unitEntryValue)
        {
            this.checkListID = checkListId;
			this.Id = unitEntryValue.Id;
            this.Value = unitEntryValue.Value;
            this.ValueName = unitEntryValue.ValueName;
            this.Unit = unitEntryValue.Unit;
        }

        [PrimaryKey]
        public int checkListID { get; set; }

		public int Id { get; set; }
        public String ValueName { get; set; }
        public String Value { get; set; }
        public String Unit { get; set; }

        public UnitEntryValue GetUnitEntryValue()
        {
            UnitEntryValue unitEntryValue = new UnitEntryValue();
			unitEntryValue.Id = this.Id;
            unitEntryValue.Value = this.Value;
            unitEntryValue.ValueName = this.ValueName;
            unitEntryValue.Unit = this.Unit;
            return unitEntryValue;
        }
    }
}
