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
        Distance = string.Empty;
    }

    private void UpdateDistance(Location? locaiton)
    {
        distanceCalculator.UpdateTotalDistance(locaiton);

        double kilometers = distanceCalculator.GetTotalDistance();
        double roundedKilometers = Math.Round(kilometers, 2);

        if(kilometers <= 1)
        {
            int meters = (int)(kilometers * 1000);
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

    private async Task EdgeSessionLocation()
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
        try
        {
            distanceCalculator.Reset();
            await EdgeSessionLocation();
            await geolocationService.StartListening();
            IsRunning = true;

            if (AtStart)
            {
                AtStart = false;
            }
        }
        catch (Exception ex) 
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    [RelayCommand]
    async Task Stop()
    {
        try
        {
            await EdgeSessionLocation();
            geolocationService.StopListening();
            IsRunning = false;
        } 
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
