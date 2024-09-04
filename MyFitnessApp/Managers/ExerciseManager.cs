using MyFitnessApp.Models;
using MyFitnessApp.Services;
using MyFitnessApp.Utilities;
using System.Diagnostics;
using System.Text.Json;

namespace MyFitnessApp.Managers;

public class ExerciseManager
{
    public event EventHandler<bool>? StepDetected;

    private StepDetector stepDetector;
    private AccelerometerService accelerometerService;

    public ExerciseManager(AccelerometerService service, StepDetector detector)
    {
        accelerometerService = service;
        stepDetector = detector;
    }

    public void StartExercise()
    {
        accelerometerService.StartMonitoring();
        accelerometerService.AccelerometerDataReceived += OnAccelerometerDataReceived;
    }

    public void StopExercise()
    {
        accelerometerService.StopMonitoring();
        accelerometerService.AccelerometerDataReceived -= OnAccelerometerDataReceived;
    }

    private void OnAccelerometerDataReceived(object? sender, AccelerometerData data)
    {
        double magnitude = Math.Sqrt(data.Acceleration.X * data.Acceleration.X +
                                     data.Acceleration.Y * data.Acceleration.Y +
                                     data.Acceleration.Z * data.Acceleration.Z);

        if (stepDetector.AddMagnitude(magnitude))
        {
            StepDetected?.Invoke(this, true);
        }
    }
}
