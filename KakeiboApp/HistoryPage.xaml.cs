using KakeiboApp.Models;

namespace KakeiboApp
{
    public partial class HistoryPage : ContentPage
    {
        public HistoryPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await LoadHistoryAsync();
        }

        private async Task LoadHistoryAsync()
        {
            var incomes = await App.Database.GetIncomesAsync();
            var expenses = await App.Database.GetExpensesAsync();

            HistoryLayout.Children.Clear();

            // 収入を表示
            foreach (var income in incomes)
            {
                var button = new Button
                {
                    Text = $"💰 +¥{income.Amount:N0}　{income.Category}\n" +
                           $"{income.Date:yyyy/MM/dd}　{income.Memo}",
                    FontSize = 16
                };

                button.Clicked += async (sender, e) =>
                {
                    await Navigation.PushAsync(
                        new EditPage(income.Id, "Income")
                    );
                };

                HistoryLayout.Children.Add(button);
            }

            // 支出を表示
            foreach (var expense in expenses)
            {
                var button = new Button
                {
                    Text = $"💸 -¥{expense.Amount:N0}　{expense.Category}\n" +
                           $"{expense.Date:yyyy/MM/dd}　{expense.Memo}",
                    FontSize = 16
                };

                button.Clicked += async (sender, e) =>
                {
                    await Navigation.PushAsync(
                        new EditPage(expense.Id, "Expense")
                    );
                };

                HistoryLayout.Children.Add(button);
            }
        }
    }
}