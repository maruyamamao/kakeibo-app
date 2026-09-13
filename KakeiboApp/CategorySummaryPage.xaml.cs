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
            var expenses = await App.Database.GetExpensesAsync();

            var startDate = new DateTime(
                _currentMonth.Year,
                _currentMonth.Month,
                1);

            var endDate = startDate.AddMonths(1);

            var monthlyExpenses = expenses
                .Where(x => x.Date >= startDate && x.Date < endDate)
                .ToList();

            var categorySummary = monthlyExpenses
                .GroupBy(x => x.Category)
                .Select(group => new
                {
                    Category = group.Key,
                    Amount = group.Sum(x => x.Amount)
                })
                .OrderByDescending(x => x.Amount)
                .ToList();

            ChartList.Children.Clear();
            CategoryList.Children.Clear();

            decimal maxAmount = categorySummary.Any()
                ? categorySummary.Max(x => x.Amount)
                : 0;

            foreach (var item in categorySummary)
            {
                // =========================
                // グラフ：カテゴリ名＋金額
                // =========================

                var categoryLabel = new Label
                {
                    Text = item.Category,
                    FontSize = 15,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.FromArgb("#334155")
                };

                var amountLabel = new Label
                {
                    Text = $"¥{item.Amount:N0}",
                    FontSize = 15,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.FromArgb("#5B8DB8"),
                    HorizontalOptions = LayoutOptions.End
                };

                var headerGrid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition(GridLength.Star),
                        new ColumnDefinition(GridLength.Auto)
                    }
                };

                headerGrid.Add(categoryLabel);
                headerGrid.Add(amountLabel);

                Grid.SetColumn(amountLabel, 1);


                // =========================
                // グラフ：背景
                // =========================

                var barBackground = new Border
                {
                    BackgroundColor = Color.FromArgb("#EAF2F8"),
                    StrokeThickness = 0,
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = 10
                    },
                    HeightRequest = 20
                };


                // =========================
                // グラフ：棒
                // =========================

                double barWidth = 0;

                if (maxAmount > 0)
                {
                    barWidth =
                        (double)(item.Amount / maxAmount) * 250;
                }

                var categoryBar = new Border
                {
                    BackgroundColor = Color.FromArgb("#7DB7E8"),
                    StrokeThickness = 0,
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = 10
                    },
                    HeightRequest = 20,
                    WidthRequest = Math.Max(barWidth, 5),
                    HorizontalOptions = LayoutOptions.Start
                };


                // =========================
                // グラフ全体
                // =========================

                var barGrid = new Grid
                {
                    HeightRequest = 20
                };

                barGrid.Children.Add(barBackground);
                barGrid.Children.Add(categoryBar);


                var chartItem = new VerticalStackLayout
                {
                    Spacing = 6
                };

                chartItem.Children.Add(headerGrid);
                chartItem.Children.Add(barGrid);

                ChartList.Children.Add(chartItem);


                // =========================
                // カテゴリ詳細カード
                // =========================

                var detailBorder = new Border
                {
                    BackgroundColor = Color.FromArgb("#FFFFFF"),
                    Stroke = Color.FromArgb("#DDE7F0"),
                    StrokeThickness = 1,
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = 15
                    },
                    Padding = 16
                };

                var detailLayout = new VerticalStackLayout
                {
                    Spacing = 5
                };

                var detailCategoryLabel = new Label
                {
                    Text = item.Category,
                    FontSize = 16,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.FromArgb("#334155")
                };

                var detailAmountLabel = new Label
                {
                    Text = $"¥{item.Amount:N0}",
                    FontSize = 23,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.FromArgb("#5B8DB8")
                };

                detailLayout.Children.Add(detailCategoryLabel);
                detailLayout.Children.Add(detailAmountLabel);

                detailBorder.Content = detailLayout;

                CategoryList.Children.Add(detailBorder);
            }


            // =========================
            // 合計
            // =========================

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