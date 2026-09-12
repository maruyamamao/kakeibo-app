using KakeiboApp.Models;

namespace KakeiboApp
{
    public partial class MainPage : ContentPage
    {
        private DateTime _currentMonth = new DateTime(2026, 9, 1);

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
            var incomes = await App.Database.GetIncomesAsync();
            var expenses = await App.Database.GetExpensesAsync();

            var startDate = new DateTime(
                _currentMonth.Year,
                _currentMonth.Month,
                1);

            var endDate = startDate.AddMonths(1);

            var monthlyIncomes = incomes
                .Where(x => x.Date >= startDate && x.Date < endDate)
                .ToList();

            var monthlyExpenses = expenses
                .Where(x => x.Date >= startDate && x.Date < endDate)
                .ToList();

            decimal totalIncome = monthlyIncomes.Sum(x => x.Amount);
            decimal totalExpense = monthlyExpenses.Sum(x => x.Amount);
            decimal balance = totalIncome - totalExpense;

            MonthLabel.Text =
                $"{_currentMonth:yyyy年M月}の収支";

            IncomeLabel.Text =
                $"¥{totalIncome:N0}";

            ExpenseLabel.Text =
                $"¥{totalExpense:N0}";

            BalanceLabel.Text =
                $"¥{balance:N0}";
        }

        private async void OnPreviousMonthClicked(
            object? sender,
            EventArgs e)
        {
            _currentMonth = _currentMonth.AddMonths(-1);

            await LoadMonthlyDataAsync();
        }

        private async void OnNextMonthClicked(
            object? sender,
            EventArgs e)
        {
            _currentMonth = _currentMonth.AddMonths(1);

            await LoadMonthlyDataAsync();
        }

        private async void OnIncomeButtonClicked(
            object? sender,
            EventArgs e)
        {
            await Navigation.PushAsync(new IncomePage());
        }

        private async void OnExpenseButtonClicked(
            object? sender,
            EventArgs e)
        {
            await Navigation.PushAsync(new ExpensePage());
        }

        private async void OnHistoryButtonClicked(
            object? sender,
            EventArgs e)
        {
            await Navigation.PushAsync(new HistoryPage());
        }
    }
}