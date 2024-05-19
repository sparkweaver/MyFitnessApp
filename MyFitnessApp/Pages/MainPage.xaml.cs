using MyFitnessApp.ViewModels;

namespace MyFitnessApp.Pages;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }
}
