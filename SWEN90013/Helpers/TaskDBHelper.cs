using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SWEN90013.Data;
using SWEN90013.Models;

namespace SWEN90013.Helpers
{
    public class TaskDBHelper
    {
        public TaskDBHelper()
        {
        }

        public static async Task<List<TaskClass>> GetAllTasks()
        {
            var taskDB = await App.TaskDatabase.GetTasksAsync();
            List<TaskDBInfo> taskList = new List<TaskDBInfo>(taskDB);
            List<TaskClass> result = new List<TaskClass>();
            foreach (TaskDBInfo taskInfo in taskList)
            {
                if (taskInfo.AddNewTaskFlag == false)
                {
                    TaskClass tc = new TaskClass();
                    tc = taskInfo.getTask();
                    // TODO add checklist and defect report

                    var checkItemDB = await App.CheckItemDatabase.GetChecklistAsync(taskInfo.serviceVisitItemNumber);
                    List<CheckItemDBInfo> checkItemList = new List<CheckItemDBInfo>(checkItemDB);
                    List<CheckItem> checkItems = new List<CheckItem>();
                    foreach (CheckItemDBInfo checkItemDBInfo in checkItemList)
                    {
                        CheckItem citem = new CheckItem();
                        citem = checkItemDBInfo.GetCheckItem();
                        checkItems.Add(citem);
                    }
                    tc.checkLists = checkItems;

                    var defectDB = await App.DefectReportDatabase.GetDefectReportsAsync(taskInfo.serviceVisitItemNumber);
                    List<DefectReportDBInfo> defectList = new List<DefectReportDBInfo>(defectDB);
                    List<DefectReport> defects = new List<DefectReport>();
                    foreach (DefectReportDBInfo defectInfo in defectList)
                    {
                        DefectReport dr = new DefectReport();
                        dr = defectInfo.getDefectReport();
                        defects.Add(dr);
                    }

                    tc.defectReport = defects;
                    result.Add(tc);
                }
                
            }
            return result;
        }

        public static async Task<List<Contractor>> GetAllContractors()
        {
            var contractorDB = await App.ContractorDatabase.GetContractorsAsync();
            List<ContractorDBInfo> contractorDBList = new List<ContractorDBInfo>(contractorDB);
            List<Contractor> result = new List<Contractor>();
            foreach (ContractorDBInfo contractorInfo in contractorDBList)
            {
                result.Add(contractorInfo.getContractor());
            }
            return result;
        }

        public static async Task<int> AddNewContractor(Contractor contractor)
        {
            ContractorDBInfo contractorDBInfo = new ContractorDBInfo(contractor);
            contractorDBInfo.isNewAdded = true;

            var result = await App.ContractorDatabase.AddNewContractorAsync(contractorDBInfo);
            return result;
        }

        public static async Task<int> UpdateContractor(Contractor contractor)
        {
            ContractorDBInfo contractorDBInfo = new ContractorDBInfo(contractor);
            contractorDBInfo.isNewAdded = false;

            var result = await App.ContractorDatabase.AddNewContractorAsync(contractorDBInfo);
            return result;
        }
    }
}
