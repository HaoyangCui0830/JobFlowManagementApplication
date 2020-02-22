using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using SWEN90013.Data;
using SWEN90013.Models;

namespace SWEN90013.Data
{
    public class DefectReportDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public DefectReportDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<DefectReportDBInfo>().Wait();
        }

        public async Task<List<DefectReportDBInfo>> GetDefectReportsAsync(int serviceVisitItemNumber)
        {
            return await _database.Table<DefectReportDBInfo>()
                           .Where(i => i.serviceVisitItemNumber == serviceVisitItemNumber)
                           .ToListAsync();
        }

        public async Task SaveDefectReportInfoAsync(DefectReportDBInfo defectDBInfo)
        {
            DefectReportDBInfo queryResult = await _database.Table<DefectReportDBInfo>()
                            .Where(i => i.serviceVisitItemNumber == defectDBInfo.serviceVisitItemNumber && i.photoURL.Equals(defectDBInfo.photoURL)
                            && i.comment.Equals(defectDBInfo.comment) && i.author.Equals(defectDBInfo.author))
                            .FirstOrDefaultAsync();
            if (queryResult == null)
            {
                await _database.InsertAsync(defectDBInfo);
            }
        }
    }
}
