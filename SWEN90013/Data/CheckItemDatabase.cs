using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace SWEN90013.Data
{
    public class CheckItemDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public CheckItemDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<CheckItemDBInfo>().Wait();
        }

        public async Task<List<CheckItemDBInfo>> GetChecklistAsync(int serviceVisitItemNumber)
        {
            return await _database.Table<CheckItemDBInfo>()
                           .Where(i => i.serviceVisitItemNumber== serviceVisitItemNumber)
                           .ToListAsync();
        }

        public async Task<CheckItemDBInfo> GetChecklistAsync(int serviceVisitItemNumber, int id)
        {
            return await _database.Table<CheckItemDBInfo>()
                           .Where(i => i.serviceVisitItemNumber == serviceVisitItemNumber && i.id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task SaveCheckListInfoAsync(CheckItemDBInfo checkItemDBInfo)
        {
            CheckItemDBInfo queryResult = await _database.Table<CheckItemDBInfo>()
                .Where(i => i.serviceVisitItemNumber == checkItemDBInfo.serviceVisitItemNumber &&
                i.id == checkItemDBInfo.id).FirstOrDefaultAsync();
            if (queryResult == null)
            {
                await _database.InsertAsync(checkItemDBInfo);
            }
        }

        public Task<List<CheckItemDBInfo>> GetAllCheckListAsync()
        {
            return _database.Table<CheckItemDBInfo>().ToListAsync();
        }
    }
}
