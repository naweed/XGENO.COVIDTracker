using System;
using COVID19Tracker.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("DMSans-Regular.ttf", Alias = "RegularFont")]
[assembly: ExportFont("DMSans-Medium.ttf", Alias = "MediumFont")]
[assembly: ExportFont("DMSans-Bold.ttf", Alias = "BoldFont")]

namespace COVID19Tracker
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new DashboardPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
