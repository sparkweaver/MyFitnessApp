using Location = Microsoft.Maui.Devices.Sensors.Location;

namespace MyFitnessApp.Services;

public class GeolocationService
{
    public event EventHandler<Location>? LocationChangedEvent;
    private CancellationTokenSource? cancelTokenSource;
    private readonly SemaphoreSlim semaphore = new (1, 1);
    private bool isListening;

    // Section 1: Manual Location Request
    public async Task<Location?> GetCurrentLocation()
    {
        await semaphore.WaitAsync();
        try
        {
            GeolocationRequest request = new (GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
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
            semaphore.Release();
        }
        
        return null;
    }

    // TODO for future lifecycle
    public async Task StopAll()
    {
        if (cancelTokenSource != null && cancelTokenSource.IsCancellationRequested == false)
        {
            cancelTokenSource.Cancel();
        }

        if (isListening)
        {
            await StopListening();
        }
    }

    // Section 2: Event-based Location Updates
    public async Task StartListening()
    {
        await semaphore.WaitAsync();
        try
        {
            if (isListening)
            {
                return;
            }

            Geolocation.LocationChanged += GeolocationLocationChanged;
            GeolocationListeningRequest request = new(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
            var success = await Geolocation.StartListeningForegroundAsync(request);

            if (success)
            {
                isListening = true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            semaphore.Release();
        }
    }

    public async Task StopListening()
    {
        await semaphore.WaitAsync();
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
        finally
        {
            semaphore.Release();
        }
    }

    private void GeolocationLocationChanged(object? sender, GeolocationLocationChangedEventArgs? e)
    {
        if (e != null)
        {
            LocationChangedEvent?.Invoke(this, e.Location);
        }
    }
}
