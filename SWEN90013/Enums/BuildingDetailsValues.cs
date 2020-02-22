using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SWEN90013.Enums
{
    class BuildingDetailsValues
    {
        public enum BuildingDetailsEra
        {
            [Description("Pre-1994")]
            Pre_1994,
            [Description("Post-1994")]
            Post_1994
        }

        public enum BuildingDetailsSize
        {
            [Description("Under 200sqm")]
            Under_200sqm,
            [Description("Over 200sqm")]
            Over_200sqm
        }

        public enum BuildingDetailsClass
        {
            [Description("Class 1a")]
            Class_1a,
            [Description("Class 1b")]
            Class_1b,
            [Description("Class 2")]
            Class_2,
            [Description("Class 3")]
            Class_3,
            [Description("Class 4")]
            Class_4,
            [Description("Class 5")]
            Class_5,
            [Description("Class 6")]
            Class_6,
            [Description("Class 7a")]
            Class_7a,
            [Description("Class 7b")]
            Class_7b,
            [Description("Class 8")]
            Class_8,
            [Description("Class 9a")]
            Class_9a,
            [Description("Class 9b")]
            Class_9b,
            [Description("Class 10a")]
            Class_10a,
            [Description("Class 10b")]
            Class_10b,
        }

        public IList<string> BuildingDetailsEraList = new List<string> {
            "Pre-1994",
            "Post-1994"
        };

        public IList<string> BuildingDetailsSizeList = new List<string> {
            "Under 200sqm",
            "Over 200sqm"
        };

        public IList<string> BuildingDetailsClassList = new List<string>
        {
            "Class 1a",
            "Class 1b",
            "Class 2",
            "Class 3",
            "Class 4",
            "Class 5",
            "Class 6",
            "Class 7a",
            "Class 7b",
            "Class 8",
            "Class 9a",
            "Class 9b",
            "Class 10a",
            "Class 10b"
        };

        public int GetBuildingDetailsEraIndex (string description)
        {
            return BuildingDetailsEraList.IndexOf(description);
        }

        public string GetBuildingDetailsEraDesciption (int index)
        {
            if (index >= 0 && index < BuildingDetailsEraList.Count)
            {
                return BuildingDetailsEraList[index];
            }

            return "";
        }

        public int GetBuildingDetailsSizeIndex (string description)
        {
            return BuildingDetailsSizeList.IndexOf(description);
        }

        public string GetBuildingDetailsSizeDescription (int index)
        {
            if (index >= 0 && index < BuildingDetailsSizeList.Count)
            {
                return BuildingDetailsSizeList[index];
            }

            return "";
        }

        public int GetBuildingDetailsClassIndex (string description)
        {
            return BuildingDetailsClassList.IndexOf(description);
        }

        public string GetBuildingDetailsClassDescription (int index)
        {
            if (index >= 0 && index < BuildingDetailsClassList.Count)
            {
                return BuildingDetailsClassList[index];
            }

            return "";
        }
        
    }
}
