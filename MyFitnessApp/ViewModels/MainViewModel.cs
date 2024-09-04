using MyFitnessApp.Pages;
using System.ComponentModel;
using System.Windows.Input;

namespace MyFitnessApp.ViewModels;

public class MainViewModel
{
    public ICommand OpenExercisePageCommand { private set;  get; }
    public ICommand OpenDietPageCommand { private set; get; }

    public MainViewModel()
    {
        OpenExercisePageCommand = new Command(async () => await OpenExercisePage());
        OpenDietPageCommand = new Command(async () => await OpenDietPage());
    }

    private async Task OpenExercisePage()
    {
        try
        {
            await Shell.Current.GoToAsync(nameof(ExercisePage));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private async Task OpenDietPage()
    {
        try
        {
            await Shell.Current.GoToAsync(nameof(DietPage));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
