using System;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWEN90013.Data
{
    public class TaskDatabase
    {

        readonly SQLiteAsyncConnection _database;

        public TaskDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<TaskDBInfo>().Wait();
        }

        public Task<List<TaskDBInfo>> GetTasksAsync()
        {

            return _database.Table<TaskDBInfo>().ToListAsync();
        }

        public Task<TaskDBInfo> GetTaskAsync(int serviceVisitId, int siteId, String taskDescription)
        {
            return _database.Table<TaskDBInfo>()
                            .Where(i => (i.serviceVisitID == serviceVisitId && i.siteID == siteId && i.taskTypeDescription.Equals(taskDescription)))
                            .FirstOrDefaultAsync();
        }

        public Task<TaskDBInfo> GetTaskAsync(int serviceVisitItemId)
        {
            return _database.Table<TaskDBInfo>()
                            .Where(i => i.serviceVisitItemNumber == serviceVisitItemId)
                            .FirstOrDefaultAsync();
        }

        public async Task<int> SaveTaskAsync(TaskDBInfo Task)
        {
            TaskDBInfo queryResult = await _database.Table<TaskDBInfo>()
                            .Where(i => i.serviceVisitID == Task.serviceVisitID &&
                            i.siteID == Task.siteID && i.taskTypeDescription.Equals(Task.taskTypeDescription))
                            .FirstOrDefaultAsync();
            if (queryResult != null)
            {
                Console.WriteLine("Update");
                Console.WriteLine(Task.taskTypeDescription);
                return await _database.UpdateAsync(Task);
            }
            else
            {
                try
                {
                    Console.WriteLine("Insert");
                Console.WriteLine(Task.taskTypeDescription);
                return await _database.InsertAsync(Task);
                }
                catch(Exception e) { Console.WriteLine(e.Message); Console.WriteLine(e.StackTrace); Console.WriteLine(e.Data);
                    Console.WriteLine(e.Source); Console.WriteLine(e.TargetSite); Console.WriteLine(e.InnerException);
                    Console.WriteLine(e.GetBaseException());
                    Console.WriteLine(e.HelpLink);
                }
                return await _database.InsertAsync(Task);
            }
        }

        public async Task<int> SaveTaskAsync(TaskDBInfo Task, int barcode, int OPNumber)
        {
            TaskDBInfo queryResult = await _database.Table<TaskDBInfo>()
                            .Where(i => i.serviceVisitID == Task.serviceVisitID &&
                            i.barcode == barcode)
                            .FirstOrDefaultAsync();
            if (queryResult != null)
            {
                Console.WriteLine("Update");
                Console.WriteLine(Task.taskTypeDescription);
                return await _database.UpdateAsync(Task);
            }
            else
            {
                try
                {
                    Console.WriteLine("Insert");
                Console.WriteLine(Task.taskTypeDescription);
                return await _database.InsertAsync(Task);
                }
                catch(Exception e) { Console.WriteLine(e.Message); Console.WriteLine(e.StackTrace); Console.WriteLine(e.Data);
                    Console.WriteLine(e.Source); Console.WriteLine(e.TargetSite); Console.WriteLine(e.InnerException);
                    Console.WriteLine(e.GetBaseException());
                    Console.WriteLine(e.HelpLink);
                }
                return await _database.InsertAsync(Task);
            }
        }

        public Task<int> UpdateTaskAsync(TaskDBInfo Task)
        {
            return _database.UpdateAsync(Task);
        }


        public Task<int> DeleteTaskAsync(TaskDBInfo Task)
        {
            return _database.DeleteAsync(Task);
        }
    }
}
