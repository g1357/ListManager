using ListManager.ViewModels;

namespace ListManager.Views;

public partial class VersionPage : ContentPage
{
	VersionViewModel viewModel;
	public VersionPage(VersionViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = this.viewModel = viewModel;
	}
}