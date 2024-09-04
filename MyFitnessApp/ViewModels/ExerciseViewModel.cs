using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFitnessApp.Managers;
using MyFitnessApp.Models;
using MyFitnessApp.Pages;
using MyFitnessApp.Services;
using MyFitnessApp.Utilities;
using System.Diagnostics;
using System.Text.Json;

namespace MyFitnessApp.ViewModels;

public partial class ExerciseViewModel : ObservableObject
{
    private AccelerometerService accelerometerService = new();
    private StateService stateService = new();
    private StepDetectorState state;
    private StepDetector detector;
    private ExerciseManager manager;

    [ObservableProperty]
    bool atStart;

    [ObservableProperty]
    bool isRunning;

    [ObservableProperty]
    string distance;

    [ObservableProperty]
    int steps;

    public ExerciseViewModel() {
        state = stateService.LoadState();
        detector = new StepDetector(state);
        manager = new ExerciseManager(accelerometerService, detector);
        manager.StepDetected += OnStepDetected;
        AtStart = true;
        IsRunning = false;
        Distance = string.Empty;
        Steps = 0;
    }

    private static StepDetectorState LoadDetectorState()
    {
        try
        {
            string serializedState = Preferences.Get("StepDetectorState", "");
            if (!string.IsNullOrEmpty(serializedState))
            {
                return JsonSerializer.Deserialize<StepDetectorState>(serializedState) ?? new StepDetectorState();
            }
            else
            {
                return new StepDetectorState();
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Error loading state: {e.Message}");
            return new StepDetectorState();
        }
    }

    private void OnStepDetected(object? sender, bool isStep)
    {
        Steps++;
    }

    [RelayCommand]
    async Task OpenSettingsPage()
    {
        try
        {
            manager.StopExercise();

            var parameter = new Dictionary<string, object> 
            {
                {"StepDetectorState", state }
            };

            await Shell.Current.GoToAsync(nameof(SettingsPage), parameter);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error opening settings page: {ex.Message}");
        }
    }

    [RelayCommand]
    void Start()
    {
        try
        {
            manager.StartExercise();
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
            manager.StopExercise();
            stateService.SaveState(state);
            IsRunning = false;
        } 
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
