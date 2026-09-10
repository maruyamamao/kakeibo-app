using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using KakeiboApp.Models;

namespace KakeiboApp.Data
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            string dbPath = Path.Combine(
                FileSystem.AppDataDirectory,
                "kakeibo.db3"
            );

            _database = new SQLiteAsyncConnection(dbPath);
        }

        public async Task InitializeAsync()
        {
            await _database.CreateTableAsync<Income>();
        }

        public async Task<int> AddIncomeAsync(Income income)
        {
            return await _database.InsertAsync(income);
        }

        public async Task<List<Income>> GetIncomesAsync()
        {
            return await _database.Table<Income>().ToListAsync();
        }
    }
}