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
            await DisplayAlert("登録完了", "収入を登録しました！", "OK");

            await Navigation.PopAsync();
        }
    }
}