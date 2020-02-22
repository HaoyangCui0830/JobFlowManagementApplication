using System;
using System.ComponentModel;
using System.Reflection;

using Xamarin.Forms;

namespace SWEN90013.Enums
{
    public enum TaskResultStatus
    {
        [Description("Passed")]
        Passed,
        [Description("Failed")]
        Failed,
        [Description("NoTest")]
        NoTest
    }

    static class TaskResultStatusMethods
    {
        public static String GetColor(this TaskResultStatus status)
        {
            switch (status)
            {
                //ready and scheduled - orange shade
                case TaskResultStatus.Failed
                       :
                    return "Red";
                case TaskResultStatus.Passed
                       :
                    return "Green";

                default
                       :
                    return "White";
            }
        }

        public static String GetResultIconUrl(this TaskResultStatus status)
        {
            switch (status)
            {
                case TaskResultStatus.Failed
                        :
                    return "result-failed.png";
                case TaskResultStatus.Passed
                        :
                    return "result-passed.png";
                default
                        :
                    return "";
            }
        }

        public static Color GetBgColor(this TaskResultStatus status)
        {
            switch (status)
            {
                //ready and scheduled - orange shade
                case TaskResultStatus.Failed
                       :
                    return Color.FromRgba(255/255, 0, 0, 0.2);
                case TaskResultStatus.Passed
                       :
                    return Color.FromRgba(46/255d, 204/255d, 113/255d, 0.2);

                default
                       :
                    return Color.FromRgba(255, 255, 255, 0);
            }
        }

        /// <summary>
        /// convert a TaskResultStatus enum to its string representation
        /// </summary>
        /// <param name="status">target status</param>
        /// <returns>string representation of a checklist status enum</returns>
        public static String GetDescription(this TaskResultStatus status)
        {
            FieldInfo field = status.GetType().GetField(status.ToString());
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute == null ? status.ToString() : attribute.Description;
        }
    }
}
