using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SWEN90013.Data;
using SWEN90013.Models;

namespace SWEN90013.Helpers
{
    public class ServiceVisitDBHelper
    {
        public ServiceVisitDBHelper()
        {
        }

        public static async Task<List<ServiceVisit>> GetAllServiceVisits()
        {
            var serviceVisitDB = await App.ServiceVisitsDatabase.GetServiceVisitsAsync();
            List<ServiceVisitDBInfo> serviceVisitDBList = new List<ServiceVisitDBInfo>(serviceVisitDB);
            List<ServiceVisit> result = new List<ServiceVisit>();
            foreach (ServiceVisitDBInfo svInfo in serviceVisitDBList)
            {
                result.Add(svInfo.getServiceVisit());
            }
            return result;
        }

        public static async Task<ServiceVisit> GetServiceVisit(int serviceVisitId)
        {
            ServiceVisitDBInfo serviceVisitDB = await App.ServiceVisitsDatabase.GetServiceVisitAsync(serviceVisitId);
           // ServiceVisitDBInfo serviceVisitDBInfo = new ServiceVisitDBInfo(serviceVisitDB);
            return serviceVisitDB.getServiceVisit();
        }

        public static async Task<ServiceVisit> GetServiceVisitBySiteId(int siteId)
        {
            var serviceVisitDB = await App.ServiceVisitsDatabase.GetServiceVisitAsyncBySiteId(siteId);
            ServiceVisitDBInfo serviceVisitDBInfo = (ServiceVisitDBInfo)(serviceVisitDB);
            return serviceVisitDBInfo.getServiceVisit();
        }

        public static async Task<int> SaveServiceVisit(ServiceVisit serviceVisit)
        {
            ServiceVisitDBInfo serviceVisitDBInfo = await App.ServiceVisitsDatabase.GetServiceVisitAsync(serviceVisit.ServiceVisitId);
            var result = await App.ServiceVisitsDatabase.SaveServiceVisitAsync(serviceVisitDBInfo);
            return result;
        }

        public static async Task<int> SaveServiceVisitDeleteSchedule(ServiceVisit serviceVisit)
        {
            ServiceVisitDBInfo serviceVisitDBInfo = await App.ServiceVisitsDatabase.GetServiceVisitAsync(serviceVisit.ServiceVisitId);
            serviceVisitDBInfo.ScheduledDate = null;
            serviceVisitDBInfo.deleteSchedule = true;
            Console.WriteLine("delete executed" + serviceVisitDBInfo.deleteSchedule);
            var result = await App.ServiceVisitsDatabase.UpdateServiceVisitAsync(serviceVisitDBInfo);
            return result;
        }

        public static async Task<int> SaveServiceVisitReschedule(ServiceVisit serviceVisit, DateTime dateTime)
        {
            ServiceVisitDBInfo serviceVisitDBInfo = await App.ServiceVisitsDatabase.GetServiceVisitAsync(serviceVisit.ServiceVisitId);
            serviceVisitDBInfo.ScheduledDate = dateTime;
            serviceVisitDBInfo.updateReschedule = true;
            var result = await App.ServiceVisitsDatabase.UpdateServiceVisitAsync(serviceVisitDBInfo);
            return result;
        }

        public static async Task<int> SaveServiceVisitMemo(ServiceVisit serviceVisit, Memos memo)
        {
            ServiceVisitDBInfo serviceVisitDBInfo = await App.ServiceVisitsDatabase.GetServiceVisitAsync(serviceVisit.ServiceVisitId);
            serviceVisitDBInfo.ContactMemo = memo.ContactMemo;
            serviceVisitDBInfo.FSMMemo = memo.FSMMemo;
            serviceVisitDBInfo.InductionMemo = memo.InductionMemo;
            serviceVisitDBInfo.OHSMemo = memo.OHSMemo;
            serviceVisitDBInfo.ServiceMemo = memo.ServiceMemo;
            serviceVisitDBInfo.updateMemos = true;
            var result = await App.ServiceVisitsDatabase.UpdateServiceVisitAsync(serviceVisitDBInfo);
            return result;
        }

        public static async Task<int> SaveServiceVisitBuilding(ServiceVisit serviceVisit, Building building)
        {
            ServiceVisitDBInfo serviceVisitDBInfo = await App.ServiceVisitsDatabase.GetServiceVisitAsync(serviceVisit.ServiceVisitId);
            serviceVisitDBInfo.BuildingClass = building.BuildingClass;
            serviceVisitDBInfo.BuildingEra = building.BuildingEra;
            serviceVisitDBInfo.BuildingParts = building.BuildingParts;
            serviceVisitDBInfo.BuildingSize = building.BuildingSize;
            serviceVisitDBInfo.updateBuildingDetails = true;
            var result = await App.ServiceVisitsDatabase.UpdateServiceVisitAsync(serviceVisitDBInfo);
            return result;
        }

    }
}
