using MyFitnessApp.Services;

namespace MyFitnessApp
{
    public partial class MainPage : ContentPage
    {
        private GeolocationService _geolocationService;

        public MainPage()
        {
            InitializeComponent();
            _geolocationService = new GeolocationService();
        }

        private void StartButton_Clicked(object sender, EventArgs e)
        {
            _geolocationService.StartListening();
        }

        private void StopButton_Clicked(object sender, EventArgs e)
        {
            _geolocationService.StopListening();
        }

        private async void ShowCachedLocationButton_Clicked(object sender, EventArgs e)
        {
            var location = await _geolocationService.GetCachedLocation();
            await DisplayAlert("Cached Location", location, "OK");
        }

        private async void ShowCurrentLocationButton_Clicked(object sender, EventArgs e)
        {
            await _geolocationService.GetCurrentLocation();
        }
    }
}
