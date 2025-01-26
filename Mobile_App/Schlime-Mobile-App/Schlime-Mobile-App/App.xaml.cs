using Microsoft.Extensions.Configuration;
using Schlime_Mobile_App.Config;
using Schlime_Mobile_App.Repos;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace Schlime_Mobile_App
{
    public partial class App : Application
    {
        public static Settings Settings { get; private set; }
        public static UserTypeRepo UserTypeRepo { get; private set; }
        public static FullFarmRepo FarmRepo { get; set; }
        public static CancellationTokenSource CancellationToken { get; private set; }
        public App()
        {
            InitializeComponent();
            Application.Current.UserAppTheme = AppTheme.Dark;

            var a = Assembly.GetExecutingAssembly();
            var stream = a.GetManifestResourceStream("Schlime_Mobile_App.appsettings.json");

            var test = a.GetManifestResourceNames();

            var config = new ConfigurationBuilder()
                        .AddJsonStream(stream)
                        .Build();
            Settings = config.GetRequiredSection(nameof(Settings)).Get<Settings>();
            

            UserTypeRepo = new UserTypeRepo();

            FarmRepo = new FullFarmRepo();

            CancellationToken = new CancellationTokenSource();

            MainPage = new AppShell();
        }
    }
}
