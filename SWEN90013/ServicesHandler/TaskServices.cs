using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SWEN90013.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using SWEN90013.ViewModels;
using Plugin.SecureStorage;
using Xamarin.Essentials;
using SWEN90013.Helpers;
using SWEN90013.Data;
using Plugin.SecureStorage;
using Plugin.Media.Abstractions;
using SWEN90013.ViewModels.TaskChecklist;
using SWEN90013.Enums;


namespace SWEN90013.ServicesHandler
{
    public class TaskServices
    {
        HttpClient _httpClient = new HttpClient();
        
        public TaskServices()
        {
        }

        public async Task<List<TaskClass>> GetTaskForServiceVisit(String serviceVisitID)
        {
            String url = Configuration.baseURL + "/tasks/" + serviceVisitID;
            try
            {
                //set token
                String token = CrossSecureStorage.Current.GetValue("Token");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    String json = await _httpClient.GetStringAsync(url);

                    Console.WriteLine(json);
                    //return null;
                    var result = JsonConvert.DeserializeObject<List<TaskClass>>(json);
                    
                    return result;
                }
                else
                {
                    var taskList = await TaskDBHelper.GetAllTasks();
                    List<TaskClass> tasks = new List<TaskClass>(taskList);
                    Console.WriteLine(tasks.Count);
                    return taskList;
                }
                    
            }
            catch (Exception e )
            {
                Console.WriteLine("Error in Task Service");
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<bool> AddNewTask(TaskViewModel taskViewModel, int ServiceVisitId)
        {
            String url = Configuration.baseURL + "/tasks/" + ServiceVisitId.ToString() + "/addNewTask";
            TaskClass taskClass = new TaskClass();
            taskClass.taskInformation = taskViewModel.TaskInfo;
            AddNewTaskInformation addNewTaskInformation = new AddNewTaskInformation(taskClass.taskInformation);
            var json = JsonConvert.SerializeObject(addNewTaskInformation);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            //set token
            String token = CrossSecureStorage.Current.GetValue("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
				var respond = await _httpClient.PostAsync(url, content);
				if (respond.StatusCode == System.Net.HttpStatusCode.OK) {

					Console.WriteLine("OK");
					return true;
				} else {
					return false;
				}
			}
            else
            {
                TaskDBInfo taskDBInfo = new TaskDBInfo(taskClass);
                taskDBInfo.AddNewTaskFlag = true;
                var result = await App.TaskDatabase.SaveTaskAsync(taskDBInfo);
                if (result != 0)
                {
                    Console.WriteLine(taskDBInfo.barcode);
                    return true;
                }
                return false;
            }
            
        }

        /// <summary>
        /// This is used to get all the contractors
        /// </summary>
        /// <returns>a list of all contractors</returns>
        public async Task<List<Contractor>> GetAllContractorList()
        {
            String url = Configuration.baseURL + "/contractors/getAllContractors";

            try
            {
				var current = Connectivity.NetworkAccess;
				if (current == NetworkAccess.Internet)
				{
					//set token
					String token = CrossSecureStorage.Current.GetValue("Token");
					_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

					String json = await _httpClient.GetStringAsync(url);
					List<Contractor> result = JsonConvert.DeserializeObject<List<Contractor>>(json);

					// update local db
					foreach (Contractor contractor in result) {
						await TaskDBHelper.UpdateContractor(contractor);
					}

					return result;
				}
				else
				{
					var contractorList = await TaskDBHelper.GetAllContractors();
					return contractorList;
				}

			}
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<bool> AddNewContractor(String newContractorName)
        {
            String url = Configuration.baseURL + "/contractors/addContractor";
			Contractor newContractor = new Contractor()
			{
				contractorName = newContractorName

			};
			var json = JsonConvert.SerializeObject(newContractor);
            
            var content = new StringContent(json, Encoding.UTF8, "application/json");

			var current = Connectivity.NetworkAccess;
			if (current == NetworkAccess.Internet)
			{
				//set token
				String token = CrossSecureStorage.Current.GetValue("Token");
				_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

				var respond = await _httpClient.PostAsync(url, content);

				if (respond.StatusCode == System.Net.HttpStatusCode.OK) {
					// update at local DB
					await TaskDBHelper.UpdateContractor(newContractor);
					return true;
				} else {
					return false;
				}
			}
			else {
				await TaskDBHelper.AddNewContractor(newContractor);
				return true;
			}


        }

        public async Task<bool> EditTaskInformation(int serviceVisitItemId, TaskInformation tasInfo)
        {
            String url = Configuration.baseURL + "/tasks/" + serviceVisitItemId.ToString()
                + "/editInformation";
            var json = JsonConvert.SerializeObject(tasInfo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                //set token
                String token = CrossSecureStorage.Current.GetValue("Token");
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
				var respond = await _httpClient.PutAsync(url, content);

				if (respond.StatusCode == System.Net.HttpStatusCode.OK) {
					return true;
				} else {
					return false;
				}
			}
            else
            {
                TaskDBInfo taskDBInfo = await App.TaskDatabase.GetTaskAsync(serviceVisitItemId);
                taskDBInfo.barcode = tasInfo.Barcode;
                taskDBInfo.contractor = tasInfo.Contractor;
                taskDBInfo.isMoving2012Standard = tasInfo.IsMoving2012Standard;
                taskDBInfo.maintainedStandard = tasInfo.MaintainedStandard;
                taskDBInfo.lastServicedBy = tasInfo.LastServicedBy;
                taskDBInfo.location = tasInfo.Location;
                taskDBInfo.taskTypeDescription = tasInfo.TaskTypeDescription;
                taskDBInfo.opNumber = tasInfo.OPNumber;
                taskDBInfo.EditTaskFlag = true;
                var result = await App.TaskDatabase.SaveTaskAsync(taskDBInfo, tasInfo.Barcode, tasInfo.OPNumber );
                return true;
            }

            
        }

        /// <summary>
        /// Send the test result to server
        /// </summary>
        /// <param name="serviceVisitId"></param>
        /// <param name="serviceVisitItemNumber"></param>
        /// <param name="status">status of the test</param>
        /// <param name="checklists">list of check items</param>
        /// <returns></returns>
        public async Task<bool> SendResult(int serviceVisitId, int serviceVisitItemNumber, TaskResultStatus status, List<CheckItemViewModel> checklists)
        {
            String url = Configuration.baseURL + "/tasks/" + serviceVisitItemNumber + "/SendResult";
            String user = CrossSecureStorage.Current.GetValue("UserName");
            TaskResult result = new TaskResult(status, DateTime.Now, user, serviceVisitItemNumber, checklists);
            var json = JsonConvert.SerializeObject(result);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
					//set token
					String token = CrossSecureStorage.Current.GetValue("Token");
					_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

					var respond = await _httpClient.PutAsync(url, content);

                    if (respond.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return true;
                    }
                    else
                    {
                        Console.Out.WriteLine("Send report failed from TaskService.cs. Http returned: " + respond.StatusCode);
                        return false;
                    }
                }
                else
                {
                    foreach (CheckItemViewModel checkItemViewModel in checklists)
                    {
                        CheckItem checkItem = new CheckItem(checkItemViewModel);
                        CheckItemDBInfo checkItemDBInfo = await App.CheckItemDatabase.GetChecklistAsync(serviceVisitItemNumber, checkItem.Id);
                        checkItemDBInfo.SendTotalResultFlag = true;
                        checkItemDBInfo.totalTestResult = status.ToString();
                        checkItemDBInfo.ServiceVisitId = serviceVisitId;
                        var DBresult = App.CheckItemDatabase.SaveCheckListInfoAsync(checkItemDBInfo);
                        
                    }
                    return true;
                }
                
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("Send report failed from TaskService.cs. Error message: " + e.Message);
                return false;
            }
        }


        /// <summary>
        /// Send the test result to server
        /// </summary>
        /// <param name="serviceVisitId"></param>
        /// <param name="serviceVisitItemNumber"></param>
        /// <param name="status">status of the test</param>
        /// <param name="checklists">list of check items</param>
        /// <returns></returns>
        public async Task<bool> SendResult(int serviceVisitId, int serviceVisitItemNumber, TaskResultStatus status, TaskResult taskResult)
        {
            String url = Configuration.baseURL + "/tasks/" + serviceVisitId + "/SendResult";
            String user = CrossSecureStorage.Current.GetValue("UserName");
            TaskResult result = new TaskResult(status, DateTime.Now, user, serviceVisitItemNumber, taskResult.Checklist);
            var json = JsonConvert.SerializeObject(result);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
				//set token
				String token = CrossSecureStorage.Current.GetValue("Token");
				_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

				var respond = await _httpClient.PutAsync(url, content);

                if (respond.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    Console.Out.WriteLine("Send report failed from TaskService.cs. Http returned: " + respond.StatusCode);
                    return false;
                }
                
               

            }
            catch (Exception e)
            {
                Console.Out.WriteLine("Send report failed from TaskService.cs. Error message: " + e.Message);
                return false;
            }
        }
        /// <summary>
        /// Adding a new defect report to server
        /// </summary>
        /// <param name="comment">defect report to be added</param>
        /// <returns></returns>
        public async Task<bool> AddDefectReport(int serviceVisitItemNumber, CommentViewModel comment)
        {
            String url = Configuration.baseURL + "/tasks/" + serviceVisitItemNumber + "/addDefectReport";

            String author = CrossSecureStorage.Current.GetValue("UserName");
            DefectReport defectReport = new DefectReport(comment, author);

            var json = JsonConvert.SerializeObject(defectReport);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
				//set token
				String token = CrossSecureStorage.Current.GetValue("Token");
				_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

				var respond = await _httpClient.PostAsync(url, content);

                if (respond.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    Console.Out.WriteLine("Add defect report failed from TaskService.cs. Http returned: " + respond.StatusCode);
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("Add defect report failed from TaskService.cs. Error message: " + e.Message);
                return false;
            }
        }
    }
}
