using MyFitnessApp.Models;

namespace MyFitnessApp.Utilities;

public class StepDetector
{
    private StepDetectorState state;

    private DateTime lastStepTime = DateTime.MinValue;
    private TimeSpan stepDebounce = TimeSpan.FromSeconds(0.3);
    
    private double lowerBound = 1; 
    
    public StepDetector(StepDetectorState initialState)
    {
        state = initialState ?? new StepDetectorState();
    }

    public bool AddMagnitude(double magnitude)
    {
        if (DateTime.UtcNow - lastStepTime < stepDebounce)
        {
            return false;
        }

        if (state.RecentMagnitudes.Count >= state.WindowSize)
        {
            var removed = state.RecentMagnitudes.Dequeue();
            state.Sum -= removed;
            state.SumSq -= removed * removed;
        }

        state.RecentMagnitudes.Enqueue(magnitude);
        state.Sum += magnitude;
        state.SumSq += magnitude * magnitude;

        if (state.RecentMagnitudes.Count >= Math.Min(20, state.WindowSize))
        {
            double mean = state.Sum / state.WindowSize;
            double variance = (state.SumSq / state.WindowSize) - (mean * mean);
            double stdDev = Math.Sqrt(variance);

            double dynamicThreshold = mean + (stdDev * state.ThresholdMultiplier);

            if (magnitude > Math.Max(lowerBound, dynamicThreshold))
            {
                lastStepTime = DateTime.UtcNow;
                return true;
            }
        }

        return false;
    }

    public StepDetectorState GetState() 
    {
        return state; 
    }

    public void SetState(StepDetectorState newState)
    {
        state = newState;
    }

    public void SetLowerBound(double newLowerBound)
    {
        lowerBound = newLowerBound * 1.05;
    }
}
