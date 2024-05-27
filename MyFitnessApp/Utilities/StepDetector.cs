using MyFitnessApp.Models;

namespace MyFitnessApp.Utilities;

public class StepDetector
{
    private StepDetectorState state;
    
    public StepDetector(StepDetectorState initialState)
    {
        state = initialState ?? new StepDetectorState();
    }

    public bool AddMagnitude(double magnitude)
    {
        if (state.RecentMagnitudes.Count >= state.WindowSize)
        {
            var removed = state.RecentMagnitudes.Dequeue();
            state.Sum -= removed;
            state.SumSq -= removed * removed;
        }

        state.RecentMagnitudes.Enqueue(magnitude);
        state.Sum += magnitude;
        state.SumSq += magnitude * magnitude;

        if (state.RecentMagnitudes.Count == state.WindowSize)
        {
            double mean = state.Sum / state.WindowSize;
            double variance = (state.SumSq / state.WindowSize) - (mean * mean);
            double stdDev = Math.Sqrt(variance);

            double dynamicThreshold = mean + (stdDev * state.ThresholdMultiplier);
            

            if (magnitude > dynamicThreshold)
            {
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
}
