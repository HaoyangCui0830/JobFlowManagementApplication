using System;
using SWEN90013.Enums;

namespace SWEN90013.Models
{
    public class ServiceVisitSubmission
    {
        #region constructors
        public ServiceVisitSubmission()
        {
        }
        #endregion

        #region properties
        public bool customerSignature { get; set; }
        public bool techSignature { get; set; }
        public string reviewNotes { get; set; }
        public string lastUpdatedBy { get; set; }
        public string serviceVisitStatus { get; set; }
        #endregion
    }
}
