using KakeiboApp.Models;

namespace KakeiboApp
{
    public partial class IncomePage : ContentPage
    {
        public IncomePage()
        {
            InitializeComponent();
        }

        private async void OnRegisterClicked(object? sender, EventArgs e)
        {
            // 金額が入力されているか確認
            if (string.IsNullOrWhiteSpace(AmountEntry.Text))
            {
                await DisplayAlert("エラー", "金額を入力してください。", "OK");
                return;
            }

            // 金額が数字か確認
            if (!decimal.TryParse(AmountEntry.Text, out decimal amount))
            {
                await DisplayAlert("エラー", "正しい金額を入力してください。", "OK");
                return;
            }

            // Incomeオブジェクトを作成
            var income = new Income
            {
                Amount = amount,
                Date = DatePicker.Date,
                Category = CategoryPicker.SelectedItem?.ToString() ?? "",
                Memo = MemoEditor.Text ?? ""
            };

            // SQLiteに保存
            await App.Database.AddIncomeAsync(income);

            // 登録完了
            await DisplayAlert(
                "登録完了",
                $"¥{income.Amount:N0} の収入を登録しました！",
                "OK"
            );

            // 前の画面へ戻る
            await Navigation.PopAsync();
        }
    }
}