using System;
using SWEN90013.Models;
using SQLite;

namespace SWEN90013.Data
{
    public class ContractorDBInfo
    {
        #region constractor
        public ContractorDBInfo(Contractor contractor)
        {
            this.contractorName = contractor.contractorName;
        }

        public ContractorDBInfo() { }
        #endregion

        #region properties
        [PrimaryKey]
        public String contractorName { get; set; }

        public Boolean isNewAdded { get; set; }
        #endregion

        public Contractor getContractor()
        {
            Contractor contractor = new Contractor();
            contractor.contractorName = this.contractorName;

            return contractor;
        }
    }
}