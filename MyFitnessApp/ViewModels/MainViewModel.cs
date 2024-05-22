using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFitnessApp.Pages;

namespace MyFitnessApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [RelayCommand]
    async Task OpenDistancePage()
    {
        try 
        {
            await Shell.Current.GoToAsync(nameof(DistancePage));
        } 
        catch (Exception ex) 
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
