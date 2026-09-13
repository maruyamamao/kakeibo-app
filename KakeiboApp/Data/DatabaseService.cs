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
            await _database.CreateTableAsync<Budget>();
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

        public async Task<Income?> GetIncomeAsync(int id)
        {
            return await _database
                .Table<Income>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<int> AddExpenseAsync(Expense expense)
        {
            return await _database.InsertAsync(expense);
        }

        public async Task<List<Expense>> GetExpensesAsync()
        {
            return await _database.Table<Expense>().ToListAsync();
        }

        public async Task<Expense?> GetExpenseAsync(int id)
        {
            return await _database
                .Table<Expense>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task<int> UpdateIncomeAsync(Income income)
        {
            return await _database.UpdateAsync(income);
        }

        public async Task<int> UpdateExpenseAsync(Expense expense)
        {
            return await _database.UpdateAsync(expense);
        }
        public async Task<int> DeleteIncomeAsync(Income income)
        {
            return await _database.DeleteAsync(income);
        }

        public async Task<int> DeleteExpenseAsync(Expense expense)
        {
            return await _database.DeleteAsync(expense);
        }
        public async Task<Budget?> GetBudgetAsync(int year, int month)
        {
            return await _database
                .Table<Budget>()
                .Where(x => x.Year == year && x.Month == month)
                .FirstOrDefaultAsync();
        }

        public async Task<int> AddBudgetAsync(Budget budget)
        {
            return await _database.InsertAsync(budget);
        }

        public async Task<int> UpdateBudgetAsync(Budget budget)
        {
            return await _database.UpdateAsync(budget);
        }
       
        // 前借り額を設定
        public async Task<int> SetBorrowedAmountAsync(
            int year,
            int month,
            decimal borrowedAmount)
        {
            var budget = await GetBudgetAsync(year, month);

            if (budget == null)
            {
                budget = new Budget
                {
                    Year = year,
                    Month = month,
                    Amount = 0,
                    BorrowedAmount = borrowedAmount
                };

                return await AddBudgetAsync(budget);
            }

            budget.BorrowedAmount = borrowedAmount;

            return await UpdateBudgetAsync(budget);
        }
    }
}