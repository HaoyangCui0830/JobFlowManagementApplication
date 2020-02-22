using System;
namespace SWEN90013.Models
{
    public class Memos
    {
        #region constructor
        public Memos()
        {
        }
        #endregion constructor

        #region properties
        public string ContactMemo { get; set; }
        public string InductionMemo { get; set; }
        public string OHSMemo { get; set; }
        public string FSMMemo { get; set; }
        public string ServiceMemo { get; set; }
        #endregion
    }
}
