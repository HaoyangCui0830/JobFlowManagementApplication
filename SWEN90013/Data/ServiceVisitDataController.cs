using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SWEN90013.ServicesHandler;
using Xamarin.Essentials;

namespace SWEN90013.Data
{
    public class ServiceVisitDataController
    {
        public ServiceVisitDataController()
        {
        }

        public static async System.Threading.Tasks.Task submitAllLocalUpdateAsync()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                var serviceVisitDB = await App.ServiceVisitsDatabase.GetServiceVisitsAsync();
                List<ServiceVisitDBInfo> serviceVisitDBList = new List<ServiceVisitDBInfo>(serviceVisitDB);
                foreach (ServiceVisitDBInfo svDBInfo in serviceVisitDBList)
                {
                    Console.WriteLine(svDBInfo.ServiceVisitId);
                    Console.WriteLine("Memo" + svDBInfo.updateMemos);
                    Console.WriteLine("building" + svDBInfo.updateBuildingDetails);
                    Console.WriteLine("resche" + svDBInfo.updateReschedule);
                    Console.WriteLine("deletesche" + svDBInfo.deleteSchedule);
                    if (svDBInfo.updateMemos == true)
                    {
                        await new ServiceVisitServices().UpdateMemo(svDBInfo.SiteId, svDBInfo.ServiceVisitId, svDBInfo.getServiceVisit().Memos);
                        svDBInfo.updateMemos = false;
                        await Task.Delay(500);
                    }
                    if (svDBInfo.updateBuildingDetails == true)
                    {
                        await new ServiceVisitServices().UpdateBuildingDetails(svDBInfo.SiteId, svDBInfo.getServiceVisit().Building);
                        svDBInfo.updateBuildingDetails = false;
                        await Task.Delay(500);
                    }
                    if (svDBInfo.updateReschedule == true)
                    {
                        new ServiceVisitServices().RescheduleServiceVisitSchedule(svDBInfo.ServiceVisitId.ToString(), (System.DateTime)svDBInfo.getServiceVisit().ScheduledDate);
                        svDBInfo.updateReschedule = false;
                        await Task.Delay(500);
                    }
                    if (svDBInfo.deleteSchedule == true)
                    {
                        _ = new ServiceVisitServices().DeleteServiceVisitSchedule(svDBInfo.ServiceVisitId.ToString());
                        svDBInfo.deleteSchedule = false;
                    }
                }

                var contractorDB = await App.ContractorDatabase.GetContractorsAsync();
                List<ContractorDBInfo> contractorDBList = new List<ContractorDBInfo>(contractorDB);
                foreach (ContractorDBInfo contractorDBInfo in contractorDBList)
                {
                    if (contractorDBInfo.isNewAdded == true)
                    {
                        await new TaskServices().AddNewContractor(contractorDBInfo.contractorName);
                        contractorDBInfo.isNewAdded = false;
                    }
                }
            }
            
        }
    }
}
