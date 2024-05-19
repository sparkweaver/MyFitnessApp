namespace MyFitnessApp.Utilities;

public class DistanceCalculator
{
    private Location? lastLocation;
    private double totalDistance;

    public DistanceCalculator()
    {
        Reset();
    }

    public void UpdateTotalDistance(Location? location)
    {
        if (lastLocation != null)
        {
            double distance = Location.CalculateDistance(lastLocation, location, DistanceUnits.Kilometers);
            totalDistance += distance;
        }

        lastLocation = location;
    }

    public double GetTotalDistance()
    {
        return totalDistance;
    }

    public void Reset()
    {
        lastLocation = null;
        totalDistance = 0;
    }
}
