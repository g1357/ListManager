using ListManager.ViewModels;

namespace ListManager.Views;

public partial class HelpPage : ContentPage
{
	HelpViewModel viewModel;

    public HelpPage()
    {
    }

    public HelpPage(HelpViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = this.viewModel = viewModel;
	}
}