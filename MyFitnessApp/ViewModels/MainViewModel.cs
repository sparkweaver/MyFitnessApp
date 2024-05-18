using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyFitnessApp.Pages;

namespace MyFitnessApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [RelayCommand]
    async Task Navigate()
    {
        await Shell.Current.GoToAsync(nameof(DistancePage));
    }
}
