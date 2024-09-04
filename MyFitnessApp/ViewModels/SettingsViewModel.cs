using MyFitnessApp.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using System.Windows.Input;

namespace MyFitnessApp.ViewModels;

public class SettingsViewModel : IQueryAttributable, INotifyPropertyChanged
{
    private StepDetectorState? detectorState;
    public StepDetectorState? DetectorState 
    {
        get 
        {
            return detectorState;
        }
        set 
        { 
            if(detectorState != value)
            {
                detectorState = value;
                OnPropertyChanged(nameof(DetectorState));
                OnPropertyChanged(nameof(WindowSize));
                OnPropertyChanged(nameof(ThresholdMultiplier));
            }
        }
    }

    public int WindowSize
    {
        get 
        {
            if (detectorState == null) { return 0; }
            return detectorState.WindowSize;
        }
        set
        {
            if (DetectorState != null && DetectorState.WindowSize != value)
            {
                DetectorState.WindowSize = value;
                OnPropertyChanged(nameof(WindowSize));
            }
        }
    }

    public double ThresholdMultiplier
    {
        get
        {
            if (detectorState == null) { return 0; }
            return detectorState.ThresholdMultiplier;
        }
        set
        {
            if (DetectorState != null && DetectorState.ThresholdMultiplier != value)
            {
                DetectorState.ThresholdMultiplier = value;
                OnPropertyChanged(nameof(ThresholdMultiplier));
            }
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        DetectorState = query["StepDetectorState"] as StepDetectorState;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected override void OnDisappearing() 
    {
    }
}
