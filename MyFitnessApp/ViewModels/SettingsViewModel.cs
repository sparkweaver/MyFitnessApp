using MyFitnessApp.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using System.Windows.Input;

namespace MyFitnessApp.ViewModels;

public class SettingsViewModel : INotifyPropertyChanged
{
    private StepDetectorState detectorState;

    public StepDetectorState DetectorState
    {
        get => detectorState;
        set 
        {
            if(detectorState != value)
            {
                detectorState = value;
                OnPropertyChanged(nameof(DetectorState));
            }
        }
    }

    public int WindowSize 
    {
        get => DetectorState.WindowSize;
        set 
        {
            if (DetectorState.WindowSize != value)
            {
                DetectorState.WindowSize = value;
                OnPropertyChanged(nameof(WindowSize));
            }
        }
    }

    public double ThresholdMultiplier
    {
        get => DetectorState.ThresholdMultiplier;
        set
        {
            if(DetectorState.ThresholdMultiplier != value)
            {
                DetectorState.ThresholdMultiplier = value;
                OnPropertyChanged(nameof(ThresholdMultiplier));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public ICommand SaveSettingsCommand { get; }
    public ICommand ResetStateCommand { get; }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public SettingsViewModel() 
    {
        SaveSettingsCommand = new Command(SaveSettings);
        ResetStateCommand = new Command(ResetState);
        LoadDetectorState();
    }

    private void LoadDetectorState()
    {
        string serializedState = Preferences.Get("StepDetectorState", "");
        if (!string.IsNullOrEmpty(serializedState))
        {
            DetectorState = JsonSerializer.Deserialize<StepDetectorState>(serializedState) ?? new StepDetectorState();
        }
        else
        {
            DetectorState = new StepDetectorState();
        }
    }

    public void SaveSettings()
    {
        try
        {
            string serializedState = JsonSerializer.Serialize(DetectorState);
            Preferences.Set("StepDetectorState", serializedState);
        }
        catch (Exception e) 
        {
            Debug.WriteLine($"Error saving state: {e.Message}");
        }
    }

    public void ResetState()
    {
        Preferences.Remove("StepDetectorState");
    }
}
