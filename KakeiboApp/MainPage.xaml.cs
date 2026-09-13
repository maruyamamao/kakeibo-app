using KakeiboApp.Models;

namespace KakeiboApp
{
    public partial class MainPage : ContentPage
    {
        private DateTime _currentMonth =
            new DateTime(2026, 9, 1);

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await UpdateBorrowedAmountAsync();
            await LoadMonthlyDataAsync();
        }

        // 前月の予算超過分を翌月の前借りとして登録
        private async Task UpdateBorrowedAmountAsync()
        {
            var expenses =
                await App.Database.GetExpensesAsync();

            // 前月
            var previousMonth =
                _currentMonth.AddMonths(-1);

            var startDate = new DateTime(
                previousMonth.Year,
                previousMonth.Month,
                1);

            var endDate =
                startDate.AddMonths(1);

            // 前月の支出
            var previousMonthExpenses = expenses
                .Where(x =>
                    x.Date >= startDate &&
                    x.Date < endDate)
                .ToList();

            decimal previousTotalExpense =
                previousMonthExpenses.Sum(x => x.Amount);

            // 前月の予算
            var previousBudget =
                await App.Database.GetBudgetAsync(
                    previousMonth.Year,
                    previousMonth.Month);

            decimal previousBudgetAmount =
                previousBudget?.Amount ?? 0;

            // 前月の予算超過額
            decimal borrowedAmount =
                previousTotalExpense - previousBudgetAmount;

            // マイナスにはしない
            if (borrowedAmount < 0)
            {
                borrowedAmount = 0;
            }

            // 翌月の前借り額として保存
            await App.Database.SetBorrowedAmountAsync(
                _currentMonth.Year,
                _currentMonth.Month,
                borrowedAmount);
        }

        private async Task LoadMonthlyDataAsync()
        {
            // 収入・支出を取得
            var incomes =
                await App.Database.GetIncomesAsync();

            var expenses =
                await App.Database.GetExpensesAsync();

            // 現在表示している月の開始日
            var startDate = new DateTime(
                _currentMonth.Year,
                _currentMonth.Month,
                1);

            // 次の月の開始日
            var endDate =
                startDate.AddMonths(1);

            // 今月の収入
            var monthlyIncomes = incomes
                .Where(x =>
                    x.Date >= startDate &&
                    x.Date < endDate)
                .ToList();

            // 今月の支出
            var monthlyExpenses = expenses
                .Where(x =>
                    x.Date >= startDate &&
                    x.Date < endDate)
                .ToList();

            // 合計金額
            decimal totalIncome =
                monthlyIncomes.Sum(x => x.Amount);

            decimal totalExpense =
                monthlyExpenses.Sum(x => x.Amount);

            decimal balance =
                totalIncome - totalExpense;

            // 今月の予算を取得
            var budget =
                await App.Database.GetBudgetAsync(
                    _currentMonth.Year,
                    _currentMonth.Month);

            // 予算が登録されていなければ0円
            decimal budgetAmount =
                budget?.Amount ?? 0;

            // 前月からの前借り額
            decimal borrowedAmount =
                budget?.BorrowedAmount ?? 0;

            // 実際に今月使える予算
            decimal availableBudget =
                budgetAmount - borrowedAmount;

            // 予算の残り
            decimal budgetRemaining =
                availableBudget - totalExpense;

            // 月
            MonthLabel.Text =
                $"{_currentMonth:yyyy年M月}の収支";

            // 収入
            IncomeLabel.Text =
                $"¥{totalIncome:N0}";

            // 支出
            ExpenseLabel.Text =
                $"¥{totalExpense:N0}";

            // 残高
            BalanceLabel.Text =
                $"¥{balance:N0}";

            // 予算
            BudgetLabel.Text =
                $"予算：¥{budgetAmount:N0}";

            // 使用額
            if (borrowedAmount > 0)
            {
                BudgetUsedLabel.Text =
                    $"前借り：¥{borrowedAmount:N0} / 使用額：¥{totalExpense:N0}";
            }
            else
            {
                BudgetUsedLabel.Text =
                    $"使用額：¥{totalExpense:N0}";
            }

            // 残り
            BudgetRemainingLabel.Text =
                $"残り：¥{budgetRemaining:N0}";
        }

        private async void OnPreviousMonthClicked(
            object? sender,
            EventArgs e)
        {
            _currentMonth =
                _currentMonth.AddMonths(-1);

            await UpdateBorrowedAmountAsync();
            await LoadMonthlyDataAsync();
        }

        private async void OnNextMonthClicked(
            object? sender,
            EventArgs e)
        {
            _currentMonth =
                _currentMonth.AddMonths(1);

            await UpdateBorrowedAmountAsync();
            await LoadMonthlyDataAsync();
        }

        private async void OnIncomeButtonClicked(
            object? sender,
            EventArgs e)
        {
            await Navigation.PushAsync(
                new IncomePage());
        }

        private async void OnExpenseButtonClicked(
            object? sender,
            EventArgs e)
        {
            await Navigation.PushAsync(
                new ExpensePage());
        }

        private async void OnHistoryButtonClicked(
            object? sender,
            EventArgs e)
        {
            await Navigation.PushAsync(
                new HistoryPage());
        }

        private async void OnBudgetButtonClicked(
            object? sender,
            EventArgs e)
        {
            await Navigation.PushAsync(
                new BudgetPage(_currentMonth));
        }
    }
}