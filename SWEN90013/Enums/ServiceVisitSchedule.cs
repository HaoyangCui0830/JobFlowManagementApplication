using System;
using System.ComponentModel;
using System.Reflection;

namespace SWEN90013.Enums
{
    public enum ServiceVisitSchedule
    {
        [Description("All")]
        All,
        [Description("All Scheduled")]
        AllScheduled,
        [Description("Today Scheduled")]
        TodayScheduled,
        [Description("Weekly Scheduled")]
        WeeklyScheduled,
        [Description("Monthly Scheduled")]
        MonthlyScheduled,
        [Description("Unscheduled")]
        Unscheduled
    }

    public static class ServiceVisitScheduleMethods
    {
        public static String GetDescription(this ServiceVisitSchedule status)
        {
            FieldInfo field = status.GetType().GetField(status.ToString());
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute == null ? status.ToString() : attribute.Description;
        }

        public static Boolean IsTodayScheduled(DateTime schedule)
        {
            var today = DateTime.Now;
            if ((schedule - today).TotalDays.Equals(0))
            {
                return true;
            }
            return false;
        }

        public static Boolean IsWeeklyScheduled(DateTime schedule)
        {
            var today = DateTime.Now;
            double difference = (schedule-today).TotalDays;
            if (difference >= 0 && difference <= 7)
            {
                return true;
            }
            return false;
        }

        public static Boolean IsMonthlyScheduled(DateTime schedule)
        {
            var today = DateTime.Now;
            double difference = (schedule - today).TotalDays;
            if (difference >= 0 && difference <= 31)
            {
                return true;
            }
            return false;
        }
    }
}
