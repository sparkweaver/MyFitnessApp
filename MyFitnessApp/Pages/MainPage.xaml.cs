using MyFitnessApp.ViewModels;

namespace MyFitnessApp;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }
}
