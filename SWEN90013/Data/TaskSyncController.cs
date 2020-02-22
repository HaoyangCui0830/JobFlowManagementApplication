using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.SecureStorage;
using SWEN90013.Enums;
using SWEN90013.Models;
using SWEN90013.ServicesHandler;
using SWEN90013.ViewModels;
using SWEN90013.ViewModels.TaskChecklist;
using Xamarin.Essentials;

namespace SWEN90013.Data
{
    public class TaskSyncController
    {
        public TaskSyncController()
        {
        }

        public static async Task submitAllLocalTaskChanges()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                var taskDB = await App.TaskDatabase.GetTasksAsync();
                List<TaskDBInfo> taskDBList = new List<TaskDBInfo>(taskDB);
                foreach (TaskDBInfo taskDBInfo in taskDBList)
                {
                    if (taskDBInfo.EditTaskFlag == true)
                    {
                        TaskInformation taskInformation = new TaskInformation();
                        taskInformation.Barcode = taskDBInfo.barcode;
                        taskInformation.Contractor = taskDBInfo.contractor;
                        taskInformation.isMoving2012Standard = taskDBInfo.isMoving2012Standard;
                        taskInformation.maintainedStandard = taskDBInfo.maintainedStandard;
                        taskInformation.LastServicedBy = taskDBInfo.lastServicedBy;
                        taskInformation.OPNumber = taskDBInfo.opNumber;
                        taskInformation.Location = taskDBInfo.location;
                        taskInformation.TaskTypeDescription = taskDBInfo.taskTypeDescription;
                        await new TaskServices().EditTaskInformation(taskDBInfo.serviceVisitItemNumber, taskInformation);
                        taskDBInfo.EditTaskFlag = false;
                        await Task.Delay(500);
                    }

                    if (taskDBInfo.AddNewTaskFlag == true)
                    {
                        TaskInformation taskInformation = new TaskInformation();
                        taskInformation.Barcode = taskDBInfo.barcode;
                        taskInformation.Contractor = taskDBInfo.contractor;
                        taskInformation.isMoving2012Standard = taskDBInfo.isMoving2012Standard;
                        taskInformation.maintainedStandard = taskDBInfo.maintainedStandard;
                        taskInformation.LastServicedBy = taskDBInfo.lastServicedBy;
                        taskInformation.OPNumber = taskDBInfo.opNumber;
                        taskInformation.Location = taskDBInfo.location;
                        taskInformation.TaskTypeDescription = taskDBInfo.taskTypeDescription;
                        TaskViewModel taskViewModel = new TaskViewModel();
                        taskViewModel.TaskInfo = taskInformation;
                        await new TaskServices().AddNewTask(taskViewModel, taskDBInfo.serviceVisitID);
                        taskDBInfo.AddNewTaskFlag = false;
                        await Task.Delay(500);
                    }

                }

                var checkDB = await App.CheckItemDatabase.GetAllCheckListAsync();
                List<CheckItemDBInfo> checkItemDBInfos = new List<CheckItemDBInfo>(checkDB);
                List<int> serviceVisitItemNumberList = new List<int>();
                foreach (CheckItemDBInfo checkItemDBInfo in checkItemDBInfos)
                {
                    if((checkItemDBInfo.SendTotalResultFlag == true) && (serviceVisitItemNumberList.Contains(checkItemDBInfo.serviceVisitItemNumber)))
                    {
                        serviceVisitItemNumberList.Add(checkItemDBInfo.serviceVisitItemNumber);
                    }
                }

                foreach (int serviceVisitItemNumber in serviceVisitItemNumberList)
                {
                    List<CheckItem> checklists= new List<CheckItem>();
                    int serviceVisitId = 0;
                    int localserviceVisitItemNumber = 0;
                    String status = "";
                    foreach (CheckItemDBInfo checkItemDBInfo in checkItemDBInfos)
                    {
                        if ((checkItemDBInfo.SendTotalResultFlag == true) && (checkItemDBInfo.serviceVisitItemNumber == serviceVisitItemNumber))
                        {
                            checkItemDBInfo.SendTotalResultFlag = false;
                            checklists.Add(checkItemDBInfo.GetCheckItem());
                            serviceVisitId = checkItemDBInfo.ServiceVisitId;
                            localserviceVisitItemNumber = checkItemDBInfo.serviceVisitItemNumber;
                            status = checkItemDBInfo.totalTestResult;
                        }
                    }
                    String user = CrossSecureStorage.Current.GetValue("UserName");
                    TaskResult taskResult = new TaskResult((TaskResultStatus)Enum.Parse(typeof(TaskResultStatus),status), DateTime.Now, user, localserviceVisitItemNumber,checklists);
                    await new TaskServices().SendResult(serviceVisitId, localserviceVisitItemNumber, (TaskResultStatus)Enum.Parse(typeof(TaskResultStatus), status), taskResult);
                    await Task.Delay(500);
                }

            }
        }
    }
}
