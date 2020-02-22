using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace SWEN90013.Enums
{
    class TaskDetailsValues
    {
        public enum TaskContractor
        {
            [Description("BT Air Conditioning")]
            BT_Air_Conditioning,
            [Description("CA AirCon Servicing")]
            CA_AirCon_Servicing,
            [Description("DA High Pressure Cleaninf")]
            DA_High_Pressure_Cleaning,
            [Description("EK Specialised Servicing")]
            EK_Specialised_Servicing,
            [Description("MN Fire Door Service")]
            MN_Fire_Door_Service,
            [Description("Fire Equipment Service")]
            Fire_Equipment_Service
        }

        public enum TaskLocation
        {
            [Description("Classroom")]
            Classroom,
            [Description("Laboratory")]
            Laboratory,
            [Description("Men's Room")]
            Mens_Room,
            [Description("Throughout")]
            Throughout,
            [Description("Women's Room")]
            Womens_Room
        }

        public enum OwnerMovingTo2012Standard
        {
            [Description("Yes")]
            Yes,
            [Description("No")]
            No
        }

        public enum MaintainedByWhatStandard
        {
            [Description("Earlier than 2005")]
            Earlier_than_2005,
            [Description("Later than 2005")]
            Later_than_2005
        }

        public IList<string> TaskContractorList = new List<string>
        {
            "BT Air Conditioning",
            "CA AirCon Servicing",
            "DA High Pressure Cleaninf",
            "EK Specialised Servicing",
            "MN Fire Door Service",
            "Fire Equipment Service"

        };

        public IList<string> TaskLocationList = new List<string>
        {
            "Classroom","Laboratory","Men's Room",
            "Throughout","Women's Room"
        };

        public IList<string> TaskOwnerMovingTo2012StandardList = new List<string>
        {
            "Yes","No"
        };

        public IList<string> TaskMaintainedByWhatStandardList = new List<string>
        {
            "1996","2006", "2012"
        };

        public int GetTaskContractorIndex(string description)
        {
            return TaskContractorList.IndexOf(description);
        }

        public string GetTaskContractorDescription(int index)
        {
            if (index >= 0 && index < TaskContractorList.Count)
            {
                return TaskContractorList[index];
            }
            return "";
        }

        public int GetTaskLocationIndex(string description)
        {
            return TaskLocationList.IndexOf(description);
        }

        public string GetTaskLocationDescription(int index)
        {
            if (index >= 0 && index < TaskLocationList.Count)
            {
                return TaskLocationList[index];
            }
            return "";
        }

        public int GetTaskOwnerMovingTi2012StandardIndex(string description)
        {
            return TaskOwnerMovingTo2012StandardList.IndexOf(description);
        }

        public string GetTaskOwnerMovingTi2012StandardDescription(int index)
        {
            if (index >= 0 && index < TaskOwnerMovingTo2012StandardList.Count)
            {
                return TaskOwnerMovingTo2012StandardList[index];
            }
            return "";
        }

        public int GetTaskMaintainedByWhatStandardIndex(string description)
        {
            return TaskMaintainedByWhatStandardList.IndexOf(description);
        }

        public string GetTaskMaintainedByWhatStandardDescription(int index)
        {
            if (index >= 0 && index < TaskMaintainedByWhatStandardList.Count)
            {
                return TaskMaintainedByWhatStandardList[index];
            }
            return "";
        }

    }
}

