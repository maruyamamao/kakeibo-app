using KakeiboApp.Models;

namespace KakeiboApp
{
    public partial class HistoryPage : ContentPage
    {
        // 現在表示している月
        private DateTime _currentMonth = new DateTime(2026, 9, 1);

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

            // 現在の月の開始日
            var startDate = new DateTime(
                _currentMonth.Year,
                _currentMonth.Month,
                1);

            // 翌月の開始日
            var endDate = startDate.AddMonths(1);

            // 現在の月の収入だけ取得
            var monthlyIncomes = incomes
                .Where(x => x.Date >= startDate && x.Date < endDate)
                .OrderByDescending(x => x.Date)
                .ToList();

            // 現在の月の支出だけ取得
            var monthlyExpenses = expenses
                .Where(x => x.Date >= startDate && x.Date < endDate)
                .OrderByDescending(x => x.Date)
                .ToList();

            HistoryLayout.Children.Clear();

            // 月表示を更新
            MonthLabel.Text = $"{_currentMonth:yyyy年M月}";

            // ========================================
            // 収入
            // ========================================

            foreach (var income in monthlyIncomes)
            {
                var button = new Button
                {
                    Text =
                        $"＋ ¥{income.Amount:N0}　{income.Category}\n" +
                        $"{income.Date:yyyy/MM/dd}　{income.Memo}",

                    FontSize = 16,
                    FontAttributes = FontAttributes.Bold,

                    BackgroundColor =
                        Color.FromArgb("#F0FAF4"),

                    TextColor =
                        Color.FromArgb("#277A45"),

                    BorderColor =
                        Color.FromArgb("#C8E8D3"),

                    BorderWidth = 1,

                    CornerRadius = 14,
                    HeightRequest = 70,

                    HorizontalOptions =
                        LayoutOptions.Fill
                };

                button.Clicked += async (sender, e) =>
                {
                    await Navigation.PushAsync(
                        new EditPage(income.Id, "Income")
                    );
                };

                HistoryLayout.Children.Add(button);
            }

            // ========================================
            // 支出
            // ========================================

            foreach (var expense in monthlyExpenses)
            {
                var button = new Button
                {
                    Text =
                        $"－ ¥{expense.Amount:N0}　{expense.Category}\n" +
                        $"{expense.Date:yyyy/MM/dd}　{expense.Memo}",

                    FontSize = 16,
                    FontAttributes = FontAttributes.Bold,

                    BackgroundColor =
                        Color.FromArgb("#FFF6EF"),

                    TextColor =
                        Color.FromArgb("#C96A32"),

                    BorderColor =
                        Color.FromArgb("#F2D6BE"),

                    BorderWidth = 1,

                    CornerRadius = 14,
                    HeightRequest = 70,

                    HorizontalOptions =
                        LayoutOptions.Fill
                };

                button.Clicked += async (sender, e) =>
                {
                    await Navigation.PushAsync(
                        new EditPage(expense.Id, "Expense")
                    );
                };

                HistoryLayout.Children.Add(button);
            }

            // ========================================
            // データがない場合
            // ========================================

            if (!monthlyIncomes.Any() && !monthlyExpenses.Any())
            {
                var emptyLabel = new Label
                {
                    Text = "この月の収支はありません。",
                    FontSize = 16,
                    TextColor = Color.FromArgb("#7B8794"),
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    Margin = new Thickness(0, 15)
                };

                HistoryLayout.Children.Add(emptyLabel);
            }
        }

        // ========================================
        // 前月
        // ========================================

        private async void OnPreviousMonthClicked(
            object? sender,
            EventArgs e)
        {
            _currentMonth = _currentMonth.AddMonths(-1);

            await LoadHistoryAsync();
        }

        // ========================================
        // 翌月
        // ========================================

        private async void OnNextMonthClicked(
            object? sender,
            EventArgs e)
        {
            _currentMonth = _currentMonth.AddMonths(1);

            await LoadHistoryAsync();
        }
    }
}