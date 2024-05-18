using MyFitnessApp.Pages;

namespace MyFitnessApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(DistancePage), typeof(DistancePage));
        }
    }
}
