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

    public void CalibrateLowerBound()
    {
        if (state.RecentMagnitudes.Count > 0)
        {
            lowerBound = ComputeBoundary(state.Sum, state.SumSq, state.RecentMagnitudes.Count);
        }
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
            double dynamicThreshold = ComputeBoundary(
                state.Sum, 
                state.SumSq, 
                state.RecentMagnitudes.Count, 
                state.ThresholdMultiplier
                );

            if (magnitude > Math.Max(lowerBound, dynamicThreshold))
            {
                lastStepTime = DateTime.UtcNow;
                return true;
            }
        }

        return false;
    }

    private static double ComputeBoundary(double sum, double sumSq, int count, double multiplier = 1)
    {
        double mean = sum / count;
        double variance = (sumSq / count) - (mean * mean);
        double stdDev = Math.Sqrt(variance);
        return mean + (stdDev * multiplier);
    }

    public StepDetectorState GetState() 
    {
        return state; 
    }

    public void SetState(StepDetectorState newState)
    {
        state = newState;
    }
}
