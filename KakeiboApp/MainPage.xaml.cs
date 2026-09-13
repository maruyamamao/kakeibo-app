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

            await UpdateBudgetAdjustmentAsync();
            await LoadMonthlyDataAsync();
        }

        // 前月の結果から、翌月の前借り・繰り越しを自動計算
        private async Task UpdateBudgetAdjustmentAsync()
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

            // 前月の予算がなければ何もしない
            if (previousBudget == null)
            {
                return;
            }

            // 前月の通常予算
            decimal previousBudgetAmount =
                previousBudget.Amount;

            // 前月に繰り越されていた金額
            decimal previousCarriedOverAmount =
                previousBudget.CarriedOverAmount;

            // 前月に前借りされていた金額
            decimal previousBorrowedAmount =
                previousBudget.BorrowedAmount;

            // 前月に実際に使える予算
            decimal previousAvailableBudget =
                previousBudgetAmount
                + previousCarriedOverAmount
                - previousBorrowedAmount;

            // 前月の差額
            decimal difference =
                previousAvailableBudget
                - previousTotalExpense;

            decimal carriedOverAmount = 0;
            decimal borrowedAmount = 0;

            // 余った場合 → 翌月へ繰り越し
            if (difference > 0)
            {
                carriedOverAmount = difference;
            }
            // 予算を超えた場合 → 翌月から前借り
            else if (difference < 0)
            {
                borrowedAmount = Math.Abs(difference);
            }

            // 翌月の予算情報を取得
            var currentBudget =
                await App.Database.GetBudgetAsync(
                    _currentMonth.Year,
                    _currentMonth.Month);

            // 翌月の予算がまだない場合
            if (currentBudget == null)
            {
                currentBudget = new Budget
                {
                    Year = _currentMonth.Year,
                    Month = _currentMonth.Month,
                    Amount = 0,
                    BorrowedAmount = borrowedAmount,
                    CarriedOverAmount = carriedOverAmount
                };

                await App.Database.AddBudgetAsync(currentBudget);
            }
            else
            {
                // 翌月の前借り・繰り越しを更新
                currentBudget.BorrowedAmount =
                    borrowedAmount;

                currentBudget.CarriedOverAmount =
                    carriedOverAmount;

                await App.Database.UpdateBudgetAsync(
                    currentBudget);
            }
        }

        // 現在の月の収支・予算を画面に表示
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

            // 収支残高
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

            // 前月からの繰り越し額
            decimal carriedOverAmount =
                budget?.CarriedOverAmount ?? 0;

            // 実際に今月使える予算
            decimal availableBudget =
                budgetAmount
                + carriedOverAmount
                - borrowedAmount;

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
            if (carriedOverAmount > 0 &&
                borrowedAmount > 0)
            {
                BudgetUsedLabel.Text =
                    $"繰り越し：¥{carriedOverAmount:N0} / " +
                    $"前借り：¥{borrowedAmount:N0} / " +
                    $"使用額：¥{totalExpense:N0}";
            }
            else if (carriedOverAmount > 0)
            {
                BudgetUsedLabel.Text =
                    $"繰り越し：¥{carriedOverAmount:N0} / " +
                    $"使用額：¥{totalExpense:N0}";
            }
            else if (borrowedAmount > 0)
            {
                BudgetUsedLabel.Text =
                    $"前借り：¥{borrowedAmount:N0} / " +
                    $"使用額：¥{totalExpense:N0}";
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

        // 翌月へ
        private async void OnNextMonthClicked(
            object? sender,
            EventArgs e)
        {
            _currentMonth =
                _currentMonth.AddMonths(1);

            await UpdateBudgetAdjustmentAsync();
            await LoadMonthlyDataAsync();
        }

        // 前月へ
        private async void OnPreviousMonthClicked(
            object? sender,
            EventArgs e)
        {
            _currentMonth =
                _currentMonth.AddMonths(-1);

            await UpdateBudgetAdjustmentAsync();
            await LoadMonthlyDataAsync();
        }

        // 収入登録
        private async void OnIncomeButtonClicked(
            object? sender,
            EventArgs e)
        {
            await Navigation.PushAsync(
                new IncomePage());
        }

        // 支出登録
        private async void OnExpenseButtonClicked(
            object? sender,
            EventArgs e)
        {
            await Navigation.PushAsync(
                new ExpensePage());
        }

        // 収支履歴
        private async void OnHistoryButtonClicked(
            object? sender,
            EventArgs e)
        {
            await Navigation.PushAsync(
                new HistoryPage());
        }

        // 月間予算設定
        private async void OnBudgetButtonClicked(
            object? sender,
            EventArgs e)
        {
            await Navigation.PushAsync(
                new BudgetPage(_currentMonth));
        }
        private async void OnCategorySummaryButtonClicked(
            object? sender,
            EventArgs e)
        {
            await Navigation.PushAsync(
                new CategorySummaryPage());
        }
    }
}