using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFitnessApp.Pages;
using System.Diagnostics;

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

    [RelayCommand]
    async Task OpenSettingsPage()
    {
        try
        {
            await Shell.Current.GoToAsync(nameof(SettingsPage));
        }
        catch (Exception ex)
        {
            Console.Write($"Error opening settings page: {ex.Message}");
        }
    }
}
