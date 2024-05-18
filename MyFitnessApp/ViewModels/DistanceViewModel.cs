using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFitnessApp.Services;

namespace MyFitnessApp.ViewModels;

public partial class DistanceViewModel : ObservableObject
{
    private readonly GeolocationService geolocationService;
    private readonly DistanceCalculator distanceCalculator;

    [ObservableProperty]
    double distance;

    public DistanceViewModel() {
        distanceCalculator = new DistanceCalculator();
        geolocationService = new GeolocationService();
        geolocationService.LocationChangedEvent += OnLocationChanged;
    }

    private void UpdateDistance(Location? locaiton)
    {
        distanceCalculator.UpdateTotalDistance(locaiton);
        Distance = distanceCalculator.GetTotalDistance();
    }

    private void OnLocationChanged(object? sender, Location? locaiton)
    {
        UpdateDistance(locaiton);
    }

    async private Task EdgeSessionLocation()
    {
        try
        {
            Location? locaiton = await geolocationService.GetCurrentLocation();
            UpdateDistance(locaiton);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
        }
    } 

    [RelayCommand]
    async Task Start()
    {
        distanceCalculator.Reset();
        await EdgeSessionLocation();
        geolocationService.StartListening();
    }

    [RelayCommand]
    async Task Stop()
    {
        await EdgeSessionLocation();
        geolocationService.StopListening();
    }
}
