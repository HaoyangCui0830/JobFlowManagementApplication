using System;
using SQLite;
using SWEN90013.Models;

namespace SWEN90013.Data
{
    public class DefectReportDBInfo
    {
        #region constractor
        public DefectReportDBInfo()
        {
        }

        public DefectReportDBInfo(DefectReport defectReport, int serviceVisitItemNumber)
        {
            this.serviceVisitItemNumber = serviceVisitItemNumber;
            this.comment = defectReport.comment;
            this.photoURL = defectReport.photoURL;
            this.author = defectReport.author;
        }
        #endregion

        #region properties
        public int serviceVisitItemNumber { get; set; }
        public String comment { get; set; }
        public String photoURL { get; set; }
        public String author { get; set; }
        #endregion

        public DefectReport getDefectReport()
        {
            DefectReport defect = new DefectReport();
            defect.comment = this.comment;
            defect.photoURL = this.photoURL;
            defect.author = this.author;

            return defect;
        }
    }
}
