using ListManager.ViewModels;

namespace ListManager.Views;

public partial class ShoppingListDetailsPage : ContentPage
{
	ShoppingListDetailsViewModel viewModel;
	public ShoppingListDetailsPage(ShoppingListDetailsViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = this.viewModel = viewModel;
	}
}