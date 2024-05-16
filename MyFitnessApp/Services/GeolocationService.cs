using Location = Microsoft.Maui.Devices.Sensors.Location;

namespace MyFitnessApp.Services;

public class GeolocationService
{
    public event EventHandler<Location>? LocationChangedEvent;
    private CancellationTokenSource? cancelTokenSource;
    private bool isCheckingLocation;

    // Section 1: Manual Location Request
    public async Task GetCurrentLocation()
    {
        try
        {
            isCheckingLocation = true;

            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
            cancelTokenSource = new CancellationTokenSource();

            Location? location = await Geolocation.Default.GetLocationAsync(request, cancelTokenSource.Token);

            if (location != null)
                Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            isCheckingLocation = false;
        }
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
            Geolocation.LocationChanged += Geolocation_LocationChanged;
            var request = new GeolocationListeningRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(3));
            var success = await Geolocation.StartListeningForegroundAsync(request);

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
            string status = "Stopped listening for foreground location updates";
            Console.WriteLine(status);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void Geolocation_LocationChanged(object sender, GeolocationLocationChangedEventArgs e)
    {
        var location = e.Location;
        Console.WriteLine($"Location changed: Latitude {location.Latitude}, Longitude {location.Longitude}");
        LocationChangedEvent?.Invoke(this, location);
    }
}
