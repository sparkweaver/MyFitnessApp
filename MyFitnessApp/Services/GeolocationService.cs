using Location = Microsoft.Maui.Devices.Sensors.Location;

namespace MyFitnessApp.Services;

public class GeolocationService
{
    public event EventHandler<Location>? LocationChangedEvent;
    private CancellationTokenSource? cancelTokenSource;
    private bool isChecking;
    private bool isListening;

    // Section 1: Manual Location Request
    public async Task<Location?> GetCurrentLocation()
    {
        try
        {
            isChecking = true;
            
            GeolocationRequest request = new (GeolocationAccuracy.Best, TimeSpan.FromSeconds(5));
            
            cancelTokenSource = new CancellationTokenSource();

            Location? location = await Geolocation.Default.GetLocationAsync(request, cancelTokenSource.Token);
            
            return location;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            isChecking = false;
        }
        
        return null;
    }

    public void CancelRequest() 
    { 
        if (isChecking && cancelTokenSource != null && cancelTokenSource.IsCancellationRequested == false)
        {
            cancelTokenSource.Cancel();
        } 
    }

    // Section 2: Event-based Location Updates
    public async Task StartListening()
    {
        try
        {
            if (isListening)
            {
                return;
            }

            isListening = true;

            Geolocation.LocationChanged += GeolocationLocationChanged;
            GeolocationListeningRequest request = new(GeolocationAccuracy.Best);
            var success = await Geolocation.StartListeningForegroundAsync(request);

            if (!success)
            {
                isListening = false;
            }
        }
        catch (Exception ex)
        {
            isListening = false;
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public void StopListening()
    {
        try
        {
            if (!isListening)
            {
                return;
            }

            Geolocation.LocationChanged -= GeolocationLocationChanged;
            Geolocation.StopListeningForeground();

            isListening = false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void GeolocationLocationChanged(object? sender, GeolocationLocationChangedEventArgs? ev)
    {
        if (ev != null)
        {
            LocationChangedEvent?.Invoke(this, ev.Location);
        }
    }
}
