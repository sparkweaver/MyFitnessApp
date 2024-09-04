using MyFitnessApp.Models;
using System.Text.Json;

namespace MyFitnessApp.Services;

public class StateService
{
    public bool SaveState(StepDetectorState currentState)
    {
        try
        {
            string serializedState = JsonSerializer.Serialize(currentState);
            Preferences.Set("StepDetectorState", serializedState);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public StepDetectorState LoadState()
    {
        try
        {
            string serializedState = Preferences.Get("StepDetectorState", "");
            if (!string.IsNullOrEmpty(serializedState))
            {
                return JsonSerializer.Deserialize<StepDetectorState>(serializedState) ?? new StepDetectorState();
            }
            else
            {
                return new StepDetectorState();
            }
        }
        catch
        {
            return new StepDetectorState();
        }
    }
}
