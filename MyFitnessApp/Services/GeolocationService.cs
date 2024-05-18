using Location = Microsoft.Maui.Devices.Sensors.Location;

namespace MyFitnessApp.Services;

public class GeolocationService
{
    public event EventHandler<Location>? LocationChangedEvent;
    private CancellationTokenSource? cancelTokenSource;
    private bool isCheckingLocation;

    // Section 1: Manual Location Request
    public async Task<Location?> GetCurrentLocation()
    {
        try
        {
            isCheckingLocation = true;

            GeolocationRequest request = new (GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
            cancelTokenSource = new CancellationTokenSource();

            Location? location = await Geolocation.Default.GetLocationAsync(request, cancelTokenSource.Token);
            isCheckingLocation = false;

            return location;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return null;
    }

    public void CancelRequest()
    {
        if (isCheckingLocation && cancelTokenSource != null && cancelTokenSource.IsCancellationRequested == false)
        {
            cancelTokenSource.Cancel();
        }
    }

    // Section 2: Event-based Location Updates
    public async void StartListening()
    {
        try
        {
            StopListening();
            Geolocation.LocationChanged += Geolocation_LocationChanged;
            GeolocationListeningRequest request = new (GeolocationAccuracy.Best, TimeSpan.FromSeconds(3));
            var success = await Geolocation.StartListeningForegroundAsync(request);

            //TODO Remove
            string status = success
                ? "Started listening for foreground location updates"
                : "Couldn't start listening";
            Console.WriteLine(status);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public void StopListening()
    {
        try
        {
            Geolocation.LocationChanged -= Geolocation_LocationChanged;
            Geolocation.StopListeningForeground();
            
            //TODO Remove
            string status = "Stopped listening for foreground location updates";
            Console.WriteLine(status);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void Geolocation_LocationChanged(object? sender, GeolocationLocationChangedEventArgs? e)
    {
        //TODO remove
        var location = e.Location;
        Console.WriteLine($"Location changed: Latitude {location.Latitude}, Longitude {location.Longitude}");
        LocationChangedEvent?.Invoke(this, location);
    }
}
