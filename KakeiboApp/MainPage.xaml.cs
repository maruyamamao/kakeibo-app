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

            await LoadMonthlyDataAsync();
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
            var endDate = startDate.AddMonths(1);

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

            // 予算の残り
            decimal budgetRemaining =
                budgetAmount - totalExpense;

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
            BudgetUsedLabel.Text =
                $"使用額：¥{totalExpense:N0}";

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

            await LoadMonthlyDataAsync();
        }

        private async void OnNextMonthClicked(
            object? sender,
            EventArgs e)
        {
            _currentMonth =
                _currentMonth.AddMonths(1);

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