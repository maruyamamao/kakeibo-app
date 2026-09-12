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
            await _database.CreateTableAsync<Expense>();
        }

        // 収入を追加
        public async Task<int> AddIncomeAsync(Income income)
        {
            return await _database.InsertAsync(income);
        }

        // 収入を取得
        public async Task<List<Income>> GetIncomesAsync()
        {
            return await _database.Table<Income>().ToListAsync();
        }

        // 支出を追加
        public async Task<int> AddExpenseAsync(Expense expense)
        {
            return await _database.InsertAsync(expense);
        }

        // 支出を取得
        public async Task<List<Expense>> GetExpensesAsync()
        {
            return await _database.Table<Expense>().ToListAsync();
        }
    }
}