using KakeiboApp.Models;

namespace KakeiboApp
{
    public partial class BudgetPage : ContentPage
    {
        private readonly DateTime _currentMonth;

        public BudgetPage(DateTime currentMonth)
        {
            InitializeComponent();

            _currentMonth = currentMonth;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            MonthLabel.Text =
                $"{_currentMonth:yyyy年M月}の予算";

            await LoadBudgetAsync();
        }

        private async Task LoadBudgetAsync()
        {
            var budget = await App.Database.GetBudgetAsync(
                _currentMonth.Year,
                _currentMonth.Month);

            if (budget != null)
            {
                BudgetEntry.Text = budget.Amount.ToString();
            }
        }

        private async void OnSaveClicked(
            object? sender,
            EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(BudgetEntry.Text))
            {
                await DisplayAlert(
                    "エラー",
                    "予算金額を入力してください。",
                    "OK");

                return;
            }

            if (!decimal.TryParse(
                BudgetEntry.Text,
                out decimal amount))
            {
                await DisplayAlert(
                    "エラー",
                    "正しい金額を入力してください。",
                    "OK");

                return;
            }

            if (amount < 0)
            {
                await DisplayAlert(
                    "エラー",
                    "予算は0円以上で入力してください。",
                    "OK");

                return;
            }

            var budget = await App.Database.GetBudgetAsync(
                _currentMonth.Year,
                _currentMonth.Month);

            if (budget == null)
            {
                budget = new Budget
                {
                    Year = _currentMonth.Year,
                    Month = _currentMonth.Month,
                    Amount = amount
                };

                await App.Database.AddBudgetAsync(budget);
            }
            else
            {
                budget.Amount = amount;

                await App.Database.UpdateBudgetAsync(budget);
            }

            await DisplayAlert(
                "完了",
                $"{_currentMonth:yyyy年M月}の予算を¥{amount:N0}に設定しました！",
                "OK");

            await Navigation.PopAsync();
        }
    }
}