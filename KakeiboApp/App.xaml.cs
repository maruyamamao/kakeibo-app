using KakeiboApp.Data;

namespace KakeiboApp
{
    public partial class App : Application
    {
        public static DatabaseService Database { get; } = new DatabaseService();

        public App()
        {
            InitializeComponent();

            InitializeDatabase();
        }

        private async void InitializeDatabase()
        {
            await Database.InitializeAsync();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}