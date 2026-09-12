using KakeiboApp.Models;

namespace KakeiboApp
{
    public partial class EditPage : ContentPage
    {
        private readonly int _id;
        private readonly string _type;

        public EditPage(int id, string type)
        {
            InitializeComponent();

            _id = id;
            _type = type;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            if (_type == "Income")
            {
                var income = await App.Database.GetIncomeAsync(_id);

                if (income == null)
                {
                    await DisplayAlert("エラー", "収入データが見つかりません。", "OK");
                    await Navigation.PopAsync();
                    return;
                }

                AmountEntry.Text = income.Amount.ToString();
                DatePicker.Date = income.Date;
                CategoryPicker.SelectedItem = income.Category;
                MemoEditor.Text = income.Memo;
            }
            else if (_type == "Expense")
            {
                var expense = await App.Database.GetExpenseAsync(_id);

                if (expense == null)
                {
                    await DisplayAlert("エラー", "支出データが見つかりません。", "OK");
                    await Navigation.PopAsync();
                    return;
                }

                AmountEntry.Text = expense.Amount.ToString();
                DatePicker.Date = expense.Date;
                CategoryPicker.SelectedItem = expense.Category;
                MemoEditor.Text = expense.Memo;
            }
        }
        private async void OnSaveClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AmountEntry.Text))
            {
                await DisplayAlert("エラー", "金額を入力してください。", "OK");
                return;
            }

            if (!decimal.TryParse(AmountEntry.Text, out decimal amount))
            {
                await DisplayAlert("エラー", "正しい金額を入力してください。", "OK");
                return;
            }

            if (_type == "Income")
            {
                var income = await App.Database.GetIncomeAsync(_id);

                if (income == null)
                {
                    await DisplayAlert("エラー", "収入データが見つかりません。", "OK");
                    return;
                }

                income.Amount = amount;
                income.Date = DatePicker.Date;
                income.Category = CategoryPicker.SelectedItem?.ToString() ?? "";
                income.Memo = MemoEditor.Text ?? "";

                await App.Database.UpdateIncomeAsync(income);
            }
            else if (_type == "Expense")
            {
                var expense = await App.Database.GetExpenseAsync(_id);

                if (expense == null)
                {
                    await DisplayAlert("エラー", "支出データが見つかりません。", "OK");
                    return;
                }

                expense.Amount = amount;
                expense.Date = DatePicker.Date;
                expense.Category = CategoryPicker.SelectedItem?.ToString() ?? "";
                expense.Memo = MemoEditor.Text ?? "";

                await App.Database.UpdateExpenseAsync(expense);
            }

            await DisplayAlert("完了", "収支を更新しました！", "OK");

            await Navigation.PopAsync();
        }
        private async void OnDeleteClicked(object? sender, EventArgs e)
        {
            bool result = await DisplayAlert(
                "確認",
                "この収支を削除しますか？",
                "削除",
                "キャンセル");

            if (!result)
                return;

            if (_type == "Income")
            {
                var income = await App.Database.GetIncomeAsync(_id);

                if (income == null)
                {
                    await DisplayAlert("エラー", "収入データが見つかりません。", "OK");
                    return;
                }

                await App.Database.DeleteIncomeAsync(income);
            }
            else if (_type == "Expense")
            {
                var expense = await App.Database.GetExpenseAsync(_id);

                if (expense == null)
                {
                    await DisplayAlert("エラー", "支出データが見つかりません。", "OK");
                    return;
                }

                await App.Database.DeleteExpenseAsync(expense);
            }

            await DisplayAlert("完了", "収支を削除しました！", "OK");

            await Navigation.PopAsync();
        }
    }
}