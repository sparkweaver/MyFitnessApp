using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFitnessApp.Pages;

namespace MyFitnessApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [RelayCommand]
    async Task OpenExercisePage()
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
}
