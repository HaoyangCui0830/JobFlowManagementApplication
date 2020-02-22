using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SWEN90013.Data
{
    public class ContractorDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public ContractorDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<ContractorDBInfo>().Wait();
        }

        public Task<List<ContractorDBInfo>> GetContractorsAsync()
        {
            return _database.Table<ContractorDBInfo>().ToListAsync();
        }

        public async Task<int> AddNewContractorAsync(ContractorDBInfo Contractor)
        {
            ContractorDBInfo queryResult = await _database.Table<ContractorDBInfo>()
                .Where(i => i.contractorName == Contractor.contractorName)
                .FirstOrDefaultAsync();

            if (queryResult != null)
            {
                Console.WriteLine("Update");
                return await _database.UpdateAsync(Contractor);
            }
            else
            {
                return await _database.InsertAsync(Contractor);
            }
        }
    }
}
