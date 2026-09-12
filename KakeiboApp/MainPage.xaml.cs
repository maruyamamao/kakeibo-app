using KakeiboApp.Models;

namespace KakeiboApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await LoadIncomeAsync();
            await LoadExpenseAsync();
        }

        private async Task LoadIncomeAsync()
        {
            var incomes = await App.Database.GetIncomesAsync();

            decimal totalIncome = 0;

            foreach (var income in incomes)
            {
                totalIncome += income.Amount;
            }

            IncomeLabel.Text = $"¥{totalIncome:N0}";
        }

        private async Task LoadExpenseAsync()
        {
            var expenses = await App.Database.GetExpensesAsync();

            decimal totalExpense = 0;

            foreach (var expense in expenses)
            {
                totalExpense += expense.Amount;
            }

            ExpenseLabel.Text = $"¥{totalExpense:N0}";
        }

        private async void OnIncomeButtonClicked(object? sender, EventArgs e)
        {
            await Navigation.PushAsync(new IncomePage());
        }

        private async void OnExpenseButtonClicked(object? sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExpensePage());
        }
    }
}