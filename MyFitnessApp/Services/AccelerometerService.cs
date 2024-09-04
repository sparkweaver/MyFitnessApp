namespace MyFitnessApp.Services;

public class AccelerometerService
{
    public event EventHandler<AccelerometerData>? AccelerometerDataReceived;

    public void StartMonitoring()
    {
        if (Accelerometer.Default.IsSupported && !Accelerometer.Default.IsMonitoring)
        {
            Accelerometer.Default.ReadingChanged += AccelerometerReadingChanged;
            Accelerometer.Default.Start(SensorSpeed.UI);
        }
    }

    public void StopMonitoring()
    {
        if (Accelerometer.Default.IsMonitoring)
        {
            Accelerometer.Default.Stop();
            Accelerometer.Default.ReadingChanged -= AccelerometerReadingChanged;
        }
    }

    private void AccelerometerReadingChanged(object? sender, AccelerometerChangedEventArgs e)
    {
        AccelerometerDataReceived?.Invoke(this, e.Reading);
    }
}
