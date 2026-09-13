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

            // グラフをクリア
            ChartList.Children.Clear();

            // カテゴリ一覧をクリア
            CategoryList.Children.Clear();

            // 最大金額を取得
            decimal maxAmount =
                categorySummary.Any()
                    ? categorySummary.Max(x => x.Amount)
                    : 0;

            // カテゴリごとに表示
            foreach (var item in categorySummary)
            {
                // =========================
                // 横棒グラフ
                // =========================

                var categoryLabel = new Label
                {
                    Text = item.Category,
                    FontSize = 16,
                    FontAttributes = FontAttributes.Bold
                };

                var amountLabel = new Label
                {
                    Text = $"¥{item.Amount:N0}",
                    FontSize = 16,
                    HorizontalOptions = LayoutOptions.End
                };

                var headerGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition(
                            GridLength.Star),

                        new ColumnDefinition(
                            GridLength.Auto)
                    }
                };

                headerGrid.Add(categoryLabel);
                headerGrid.Add(amountLabel);
                Grid.SetColumn(amountLabel, 1);

                // 棒グラフの背景
                var barBackground = new Border
                {
                    BackgroundColor =
                        Color.FromArgb("#E0E0E0"),

                    StrokeThickness = 0,

                    StrokeShape =
                        new RoundRectangle
                        {
                            CornerRadius = 10
                        },

                    HeightRequest = 20
                };

                // 金額に応じた棒の長さ
                double barWidth = 0;

                if (maxAmount > 0)
                {
                    barWidth =
                        (double)(item.Amount / maxAmount)
                        * 250;
                }

                var bar = new Border
                {
                    BackgroundColor =
                        Color.FromArgb("#4CAF50"),

                    StrokeThickness = 0,

                    StrokeShape =
                        new RoundRectangle
                        {
                            CornerRadius = 10
                        },

                    HeightRequest = 20,

                    WidthRequest = Math.Max(
                        barWidth,
                        5),

                    HorizontalOptions =
                        LayoutOptions.Start
                };

                var barGrid = new Grid
                {
                    HeightRequest = 20
                };

                barGrid.Children.Add(
                    barBackground);

                barGrid.Children.Add(
                    bar);

                var chartItem = new VerticalStackLayout
                {
                    Spacing = 5
                };

                chartItem.Children.Add(
                    headerGrid);

                chartItem.Children.Add(
                    barGrid);

                ChartList.Children.Add(
                    chartItem);


                // =========================
                // カテゴリ一覧
                // =========================

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

                var categoryNameLabel = new Label
                {
                    Text = item.Category,
                    FontSize = 18,
                    FontAttributes =
                        FontAttributes.Bold
                };

                var categoryAmountLabel = new Label
                {
                    Text = $"¥{item.Amount:N0}",
                    FontSize = 24
                };

                layout.Children.Add(
                    categoryNameLabel);

                layout.Children.Add(
                    categoryAmountLabel);

                border.Content = layout;

                CategoryList.Children.Add(
                    border);
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