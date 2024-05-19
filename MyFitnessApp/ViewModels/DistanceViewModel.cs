using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFitnessApp.Services;
using MyFitnessApp.Utilities;

namespace MyFitnessApp.ViewModels;

public partial class DistanceViewModel : ObservableObject
{
    private readonly GeolocationService geolocationService;
    private readonly DistanceCalculator distanceCalculator;

    [ObservableProperty]
    bool atStart;

    [ObservableProperty]
    bool isRunning;

    [ObservableProperty]
    string distance;

    public DistanceViewModel() {
        distanceCalculator = new DistanceCalculator();
        geolocationService = new GeolocationService();
        geolocationService.LocationChangedEvent += OnLocationChanged;
        AtStart = true;
        IsRunning = false;
        Distance = "";
    }

    private void UpdateDistance(Location? locaiton)
    {
        distanceCalculator.UpdateTotalDistance(locaiton);

        double kilometers = distanceCalculator.GetTotalDistance();
        double roundedKilometers = Math.Round(kilometers, 2);

        if(kilometers < 1)
        {
            int meters = (int)Math.Round(kilometers * 1000);
            Distance = $"{meters} m";
        }
        else
        {
            Distance = $"{roundedKilometers} km";
        }
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
        IsRunning = true;

        if (AtStart)
        {
            AtStart = false;
        }
    }

    [RelayCommand]
    async Task Stop()
    {
        await EdgeSessionLocation();
        geolocationService.StopListening();
        IsRunning = false;
    }
}
