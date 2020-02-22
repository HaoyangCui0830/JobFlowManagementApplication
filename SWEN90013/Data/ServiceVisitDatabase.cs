using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using SWEN90013.Data;
using SWEN90013.Models;

namespace SWEN90013.Data
{
    public class ServiceVisitDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public ServiceVisitDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<ServiceVisitDBInfo>().Wait();
        }

        public Task<List<ServiceVisitDBInfo>> GetServiceVisitsAsync()
        {
            
            return _database.Table<ServiceVisitDBInfo>().ToListAsync();
        }

        public Task<ServiceVisitDBInfo> GetServiceVisitAsync(int id)
        {
            return _database.Table<ServiceVisitDBInfo>()
                            .Where(i => i.ServiceVisitId == id)
                            .FirstOrDefaultAsync();
        }

        public Task<ServiceVisitDBInfo> GetServiceVisitAsyncBySiteId(int siteId)
        {
            return _database.Table<ServiceVisitDBInfo>()
                            .Where(i => i.SiteId == siteId)
                            .FirstOrDefaultAsync();
        }

        public async Task<int> SaveServiceVisitAsync(ServiceVisitDBInfo serviceVisit)
        {
            ServiceVisitDBInfo queryResult  = await _database.Table<ServiceVisitDBInfo>()
                            .Where(i => i.ServiceVisitId ==serviceVisit.ServiceVisitId)
                            .FirstOrDefaultAsync();
            if (queryResult != null)
            {
                return await _database.UpdateAsync(serviceVisit);
            }
            else
            {
           // ServiceVisitDBInfo result1 = await _database.Table<ServiceVisitDBInfo>()
           //                 .Where(i => i.ServiceVisitId ==serviceVisit.ServiceVisitId)
            //                .FirstOrDefaultAsync();
            //ServiceVisitDBInfo result2 = await _database.Table<ServiceVisitDBInfo>()
            //                .Where(i => i.ServiceVisitId == 1)
            //                .FirstOrDefaultAsync();
            
                return await _database.InsertAsync(serviceVisit);
            }
        }

		public Task<int> UpdateServiceVisitAsync(ServiceVisitDBInfo serviceVisit)
		{
			return _database.UpdateAsync(serviceVisit);
		}


		public Task<int> DeleteServiceVisitAsync(ServiceVisitDBInfo serviceVisit)
        {
            return _database.DeleteAsync(serviceVisit);
        }

    }
}
