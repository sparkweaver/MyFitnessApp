using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFitnessApp.Managers;
using MyFitnessApp.Pages;

namespace MyFitnessApp.ViewModels;

public partial class ExerciseViewModel : ObservableObject
{
    private ExerciseManager manager = new();

    [ObservableProperty]
    bool atStart;

    [ObservableProperty]
    bool isRunning;

    [ObservableProperty]
    string distance;

    [ObservableProperty]
    int steps;

    public ExerciseViewModel() {
        manager.StepDetectedEvent += OnStepDetected;
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
    async Task OpenSettingsPage()
    {
        try
        {
            manager.StopExercise();
            await Shell.Current.GoToAsync(nameof(SettingsPage));
        }
        catch (Exception ex)
        {
            Console.Write($"Error opening settings page: {ex.Message}");
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
            IsRunning = false;
        } 
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
