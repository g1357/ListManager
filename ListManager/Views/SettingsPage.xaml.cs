using ListManager.ViewModels;

namespace ListManager.Views;

public partial class SettingsPage : ContentPage
{
	SettingsViewModel viewModel;
	public SettingsPage(SettingsViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = this.viewModel = viewModel;
	}
}