using MyFitnessApp.Services;
using MyFitnessApp.ViewModels;

namespace MyFitnessApp.Pages;

public partial class DistancePage : ContentPage
{
    public DistancePage(DistanceViewModel vm)
	{
		InitializeComponent();

        BindingContext = vm;
    }
}