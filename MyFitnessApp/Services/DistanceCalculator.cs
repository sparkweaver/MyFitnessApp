using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFitnessApp.Services;

public class DistanceCalculator
{
    private Location? _lastLocation;
    private double _totalDistance;

    public DistanceCalculator() 
    {
        Reset();
    }

    public void UpdateTotalDistance(Location location)
    {
        if (_lastLocation != null)
        {
            double distance = Location.CalculateDistance(_lastLocation, location, DistanceUnits.Kilometers);
            _totalDistance += distance;
        }
        
        _lastLocation = location;
    }

    public double GetTotalDistance() 
    {
        return _totalDistance; 
    }
    
    public void Reset()
    {
        _lastLocation = null;
        _totalDistance = 0;
    }
}
