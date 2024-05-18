using MyFitnessApp.Services;
using MyFitnessApp.ViewModels;

namespace MyFitnessApp;

public partial class MainPage : ContentPage
{
    private GeolocationService _geolocationService;
    private DistanceCalculator _distanceCalculator;

    public MainPage(MainViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
        _distanceCalculator = new DistanceCalculator();
        _geolocationService = new GeolocationService();
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
