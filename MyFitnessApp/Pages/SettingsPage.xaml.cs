using MyFitnessApp.ViewModels;

namespace MyFitnessApp.Pages;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}