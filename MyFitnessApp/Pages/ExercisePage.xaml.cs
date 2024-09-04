using MyFitnessApp.ViewModels;

namespace MyFitnessApp.Pages;

public partial class ExercisePage : ContentPage
{
    public ExercisePage(ExerciseViewModel vm)
	{
		InitializeComponent();

        BindingContext = vm;
    }
}