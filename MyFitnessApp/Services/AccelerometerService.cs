using MyFitnessApp.Models;
using MyFitnessApp.Utilities;
using System.Diagnostics;
using System.Text.Json;

namespace MyFitnessApp.Services;

public class AccelerometerService
{
    public event EventHandler<bool>? StepDetectedEvent;
    private StepDetector? stepDetector;

    private static void SaveDetectorState(StepDetectorState state)
    {
        try 
        {
            string serializedState = JsonSerializer.Serialize(state);
            Preferences.Set("StepDetectorState", serializedState);
        }
        catch (Exception e)
        {
            Debug.WriteLine($"Error saving state: {e.Message}");
        }
    }

    private static StepDetectorState LoadDetectorState()
    {
        try
        {
            string serializedState = Preferences.Get("StepDetectorState", "");
            return !string.IsNullOrEmpty(serializedState) ?
                JsonSerializer.Deserialize<StepDetectorState>(serializedState) :
                new StepDetectorState();
        }
        catch (Exception e) 
        {
            Debug.WriteLine($"Error loading state: {e.Message}");
            return new StepDetectorState();
        }
    }

    public void StartListening()
    {
        if (Accelerometer.Default.IsSupported)
        {
            var loadedState = LoadDetectorState();
            stepDetector = new StepDetector(loadedState);
            Accelerometer.ReadingChanged += OnAccelerometerChanged;
            Accelerometer.Start(SensorSpeed.UI);
        }
    }

    public void StopListening()
    {
        if (Accelerometer.Default.IsSupported) 
        {
            Accelerometer.Stop();
            Accelerometer.ReadingChanged -= OnAccelerometerChanged;
            var currentState = stepDetector.GetState();
            SaveDetectorState(currentState);
        }
    }

    private void OnAccelerometerChanged(object? sender, AccelerometerChangedEventArgs? e)
    {
        if (e != null)
        {
            var data = e.Reading;
            double magnitude = Math.Sqrt(data.Acceleration.X * data.Acceleration.X +
                                         data.Acceleration.Y * data.Acceleration.Y +
                                         data.Acceleration.Z * data.Acceleration.Z);

            var isStep = stepDetector.AddMagnitude(magnitude);
            if (isStep)
            {
                StepDetectedEvent?.Invoke(this, true);
            }
        }
    }
}
