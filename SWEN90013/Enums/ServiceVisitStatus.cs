using System;
using System.ComponentModel;
using System.Reflection;

namespace SWEN90013.Enums
{
    public enum ServiceVisitStatus
    {
        [Description("All")]
        All,
        [Description("Ready")]
        Ready,
        [Description("Not Ready")]
        NotReady,
        [Description("Scheduled")]
        Scheduled,
        [Description("In Progress")]
        InProgress,
        [Description("Inspected")]
        Inspected,
        [Description("Re-visit Required")]
        RevisitRequired,
        [Description("Vacant")]
        Vacant,
        [Description("Access Review")]
        AccessReview,
        [Description("Field Review")]
        FieldReview,
        [Description("Office Review")]
        OfficeReview,
        [Description("Pending External")]
        PendingExternal,
        [Description("Completed")]
        Completed
    }

	static class ServiceVisitStatusMethods
    {
		public static String GetColor(this ServiceVisitStatus status)
        {
            switch (status)
            {
                //ready and scheduled - orange shade
                case ServiceVisitStatus.Ready
                       : return "#f8cb64";
                case ServiceVisitStatus.Scheduled
                       : return "#FFA500";
                case ServiceVisitStatus.InProgress
                       : return "#FF7F50";
                //not ready - grey shade
                case ServiceVisitStatus.NotReady
                       : return "#778899";
                case ServiceVisitStatus.Completed
                       : return "#808080";
                //revisit or pending - red shade
                case ServiceVisitStatus.RevisitRequired
                       : return "#FF0000";
                case ServiceVisitStatus.Vacant
                       : return "#DC143C";
                case ServiceVisitStatus.PendingExternal
                       : return "#B22222";
                //blue - in reviews
                case ServiceVisitStatus.AccessReview
                       : return "#009ACD";
                case ServiceVisitStatus.FieldReview
                       : return "#0EBFE9";
                case ServiceVisitStatus.OfficeReview
                       : return "#4F94CD";
                // default - blue
                default
                       : return "#578fbe";
            }
        }

        public static String GetDescription(this ServiceVisitStatus status)
        {
            FieldInfo field = status.GetType().GetField(status.ToString());
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute == null ? status.ToString() : attribute.Description;
        }
	}
}
