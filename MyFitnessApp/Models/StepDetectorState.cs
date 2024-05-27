
namespace MyFitnessApp.Models;

public class StepDetectorState
{
    public Queue<double> RecentMagnitudes { get; set; } = new Queue<double>();
    public int WindowSize { get; set; } = 50;
    public double ThresholdMultiplier { get; set; } = 1.5;
    public double Sum { get; set; } = 0;
    public double SumSq { get; set; } = 0;
}
