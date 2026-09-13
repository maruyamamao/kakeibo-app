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

        /// <summary>
        /// 編集対象のデータを読み込む
        /// </summary>
        private async Task LoadDataAsync()
        {
            // 収入の場合
            if (_type == "Income")
            {
                var income =
                    await App.Database.GetIncomeAsync(_id);

                if (income == null)
                {
                    await DisplayAlert(
                        "エラー",
                        "収入データが見つかりません。",
                        "OK");

                    await Navigation.PopAsync();

                    return;
                }

                AmountEntry.Text =
                    income.Amount.ToString();

                DatePicker.Date =
                    income.Date;

                CategoryPicker.SelectedItem =
                    income.Category;

                MemoEditor.Text =
                    income.Memo;
            }

            // 支出の場合
            else if (_type == "Expense")
            {
                var expense =
                    await App.Database.GetExpenseAsync(_id);

                if (expense == null)
                {
                    await DisplayAlert(
                        "エラー",
                        "支出データが見つかりません。",
                        "OK");

                    await Navigation.PopAsync();

                    return;
                }

                AmountEntry.Text =
                    expense.Amount.ToString();

                DatePicker.Date =
                    expense.Date;

                CategoryPicker.SelectedItem =
                    expense.Category;

                MemoEditor.Text =
                    expense.Memo;
            }
        }

        /// <summary>
        /// 変更を保存
        /// </summary>
        private async void OnSaveClicked(
            object? sender,
            EventArgs e)
        {
            // 金額が空の場合
            if (string.IsNullOrWhiteSpace(
                AmountEntry.Text))
            {
                await DisplayAlert(
                    "エラー",
                    "金額を入力してください。",
                    "OK");

                return;
            }

            // 金額が数字として正しく入力されているか
            if (!decimal.TryParse(
                AmountEntry.Text,
                out decimal amount))
            {
                await DisplayAlert(
                    "エラー",
                    "正しい金額を入力してください。",
                    "OK");

                return;
            }

            // 金額がマイナスの場合
            if (amount < 0)
            {
                await DisplayAlert(
                    "エラー",
                    "金額は0円以上で入力してください。",
                    "OK");

                return;
            }

            // 収入の場合
            if (_type == "Income")
            {
                var income =
                    await App.Database.GetIncomeAsync(_id);

                if (income == null)
                {
                    await DisplayAlert(
                        "エラー",
                        "収入データが見つかりません。",
                        "OK");

                    return;
                }

                income.Amount = amount;

                income.Date =
                    DatePicker.Date;

                income.Category =
                    CategoryPicker.SelectedItem?
                    .ToString() ?? "";

                income.Memo =
                    MemoEditor.Text ?? "";

                await App.Database
                    .UpdateIncomeAsync(income);
            }

            // 支出の場合
            else if (_type == "Expense")
            {
                var expense =
                    await App.Database.GetExpenseAsync(_id);

                if (expense == null)
                {
                    await DisplayAlert(
                        "エラー",
                        "支出データが見つかりません。",
                        "OK");

                    return;
                }

                expense.Amount = amount;

                expense.Date =
                    DatePicker.Date;

                expense.Category =
                    CategoryPicker.SelectedItem?
                    .ToString() ?? "";

                expense.Memo =
                    MemoEditor.Text ?? "";

                await App.Database
                    .UpdateExpenseAsync(expense);
            }

            await DisplayAlert(
                "完了",
                "収支を更新しました！",
                "OK");

            await Navigation.PopAsync();
        }

        /// <summary>
        /// 収支を削除
        /// </summary>
        private async void OnDeleteClicked(
            object? sender,
            EventArgs e)
        {
            bool result =
                await DisplayAlert(
                    "確認",
                    "この収支を削除しますか？",
                    "削除",
                    "キャンセル");

            if (!result)
            {
                return;
            }

            // 収入の場合
            if (_type == "Income")
            {
                var income =
                    await App.Database.GetIncomeAsync(_id);

                if (income == null)
                {
                    await DisplayAlert(
                        "エラー",
                        "収入データが見つかりません。",
                        "OK");

                    return;
                }

                await App.Database
                    .DeleteIncomeAsync(income);
            }

            // 支出の場合
            else if (_type == "Expense")
            {
                var expense =
                    await App.Database.GetExpenseAsync(_id);

                if (expense == null)
                {
                    await DisplayAlert(
                        "エラー",
                        "支出データが見つかりません。",
                        "OK");

                    return;
                }

                await App.Database
                    .DeleteExpenseAsync(expense);
            }

            await DisplayAlert(
                "完了",
                "収支を削除しました！",
                "OK");

            await Navigation.PopAsync();
        }
    }
}