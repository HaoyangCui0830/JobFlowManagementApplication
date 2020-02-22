using System;
using System.ComponentModel;
using System.Reflection;

namespace SWEN90013.Enums
{
    public enum ChecklistStatus
    {
        [Description("Passed")]
        Passed,
        [Description("Failed")]
        Failed,
		[Description("No Test")]
		NoTest,
	}

    static class ChecklistStatusMethods
    {
        public static String GetColor(this ChecklistStatus status)
        {
            switch (status)
            {
                case ChecklistStatus.NoTest
                    : return "#000000";
                case ChecklistStatus.Passed
                    : return "#808080";
                case ChecklistStatus.Failed
                    : return "#C00000";
                default
                    : return "#000000";
            }
        }

        /// <summary>
        /// convert a ChecklistStatus enum to its string representation
        /// </summary>
        /// <param name="status">target status</param>
        /// <returns>string representation of a checklist status enum</returns>
        public static String GetDescription(this ChecklistStatus status)
        {
            FieldInfo field = status.GetType().GetField(status.ToString());
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute == null ? status.ToString() : attribute.Description;
        }
    }
}
