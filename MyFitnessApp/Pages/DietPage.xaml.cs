using MyFitnessApp.ViewModels;

namespace MyFitnessApp.Pages;

public partial class DietPage : ContentPage
{
	public DietPage(DietViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}