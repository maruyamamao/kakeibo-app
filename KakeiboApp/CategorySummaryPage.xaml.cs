using Microsoft.Maui.Controls.Shapes;

namespace KakeiboApp
{
    public partial class CategorySummaryPage : ContentPage
    {
        private DateTime _currentMonth =
            new DateTime(2026, 9, 1);

        public CategorySummaryPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await LoadCategorySummaryAsync();
        }

        private async Task LoadCategorySummaryAsync()
        {
            var expenses =
                await App.Database.GetExpensesAsync();

            var startDate = new DateTime(
                _currentMonth.Year,
                _currentMonth.Month,
                1);

            var endDate =
                startDate.AddMonths(1);

            // 今月の支出だけ取得
            var monthlyExpenses = expenses
                .Where(x =>
                    x.Date >= startDate &&
                    x.Date < endDate)
                .ToList();

            // カテゴリごとに集計
            var categorySummary = monthlyExpenses
                .GroupBy(x => x.Category)
                .Select(group => new
                {
                    Category = group.Key,
                    Amount = group.Sum(x => x.Amount)
                })
                .OrderByDescending(x => x.Amount)
                .ToList();

            // 画面をクリア
            CategoryList.Children.Clear();

            // カテゴリ別に表示
            foreach (var item in categorySummary)
            {
                var border = new Border
                {
                    StrokeShape =
                        new RoundRectangle
                        {
                            CornerRadius = 15
                        },
                    Padding = 15
                };

                var layout = new VerticalStackLayout
                {
                    Spacing = 5
                };

                var categoryLabel = new Label
                {
                    Text = item.Category,
                    FontSize = 18,
                    FontAttributes = FontAttributes.Bold
                };

                var amountLabel = new Label
                {
                    Text = $"¥{item.Amount:N0}",
                    FontSize = 24
                };

                layout.Children.Add(categoryLabel);
                layout.Children.Add(amountLabel);

                border.Content = layout;

                CategoryList.Children.Add(border);
            }

            // 合計
            decimal totalExpense =
                monthlyExpenses.Sum(x => x.Amount);

            MonthLabel.Text =
                $"{_currentMonth:yyyy年M月}";

            TotalExpenseLabel.Text =
                $"¥{totalExpense:N0}";
        }

        private async void OnPreviousMonthClicked(
            object? sender,
            EventArgs e)
        {
            _currentMonth =
                _currentMonth.AddMonths(-1);

            await LoadCategorySummaryAsync();
        }

        private async void OnNextMonthClicked(
            object? sender,
            EventArgs e)
        {
            _currentMonth =
                _currentMonth.AddMonths(1);

            await LoadCategorySummaryAsync();
        }
    }
}