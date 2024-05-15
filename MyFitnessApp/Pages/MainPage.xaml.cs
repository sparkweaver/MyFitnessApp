using MyFitnessApp.Services;

namespace MyFitnessApp
{
    public partial class MainPage : ContentPage
    {
        private GeolocationService _geolocationService;
        private DistanceCalculator _distanceCalculator;

        public MainPage()
        {
            InitializeComponent();
            _geolocationService = new GeolocationService();
            _distanceCalculator = new DistanceCalculator();
            _geolocationService.LocationChangedEvent += OnLocationChanged;
        }

        private void StartButton_Clicked(object sender, EventArgs e)
        {
            _distanceCalculator.Reset();
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

        private void OnLocationChanged(object sender, Location location)
        {
            _distanceCalculator.UpdateTotalDistance(location);
            var totalDistance = _distanceCalculator.GetTotalDistance();
            Console.WriteLine($"Total Distance Traveled: {totalDistance} km");
        }
    }
}
