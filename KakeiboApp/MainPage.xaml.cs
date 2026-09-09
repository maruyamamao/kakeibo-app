namespace KakeiboApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnIncomeButtonClicked(object? sender, EventArgs e)
        {
            await Navigation.PushAsync(new IncomePage());
        }
    }
}