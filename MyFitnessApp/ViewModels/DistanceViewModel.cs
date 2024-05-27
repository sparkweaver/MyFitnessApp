using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFitnessApp.Services;

namespace MyFitnessApp.ViewModels;

public partial class DistanceViewModel : ObservableObject
{
    private AccelerometerService accelerometerService = new AccelerometerService();

    [ObservableProperty]
    bool atStart;

    [ObservableProperty]
    bool isRunning;

    [ObservableProperty]
    string distance;

    [ObservableProperty]
    int steps;

    public DistanceViewModel() {
        accelerometerService.StepDetectedEvent += OnStepDetected;
        AtStart = true;
        IsRunning = false;
        Distance = string.Empty;
        Steps = 0;
    }

    private void OnStepDetected(object? sender, bool isStep)
    {
        Steps++;
    }

    [RelayCommand]
    void Start()
    {
        try
        {
            accelerometerService.StartListening();
            Steps = 0;
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
    void Stop()
    {
        try
        {
            accelerometerService.StopListening();
            IsRunning = false;
        } 
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
