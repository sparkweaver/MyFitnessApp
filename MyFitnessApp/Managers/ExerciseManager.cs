using MyFitnessApp.Models;
using MyFitnessApp.Utilities;
using System.Diagnostics;
using System.Text.Json;

namespace MyFitnessApp.Managers;

public class ExerciseManager
{
    public event EventHandler<bool>? StepDetectedEvent;

    private StepDetectorState stepDetectorState;
    private StepDetector stepDetector;

    public ExerciseManager()
    {
        stepDetectorState = LoadDetectorState();
        stepDetector = new StepDetector(stepDetectorState);
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

    private static void SaveDetectorState(StepDetectorState state)
    {
        try
        {
            string serializedState = JsonSerializer.Serialize(state);
            Preferences.Set("StepDetectorState", serializedState);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error saving state: {e.Message}");
        }
    }

    public void StartExercise()
    {
        if (Accelerometer.Default.IsSupported)
        {
            var loadedState = LoadDetectorState();
            stepDetector = new StepDetector(loadedState);
            Accelerometer.ReadingChanged += OnAccelerometerChange;
            Accelerometer.Start(SensorSpeed.UI);
        }
    }

    public void StopExercise()
    {
        if (Accelerometer.Default.IsSupported)
        {
            Accelerometer.Stop();
            Accelerometer.ReadingChanged -= OnAccelerometerChange;
            var currentState = stepDetector.GetState();
            SaveDetectorState(currentState);
        }
    }

    private void OnAccelerometerChange(object? sender, AccelerometerChangedEventArgs? e)
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
