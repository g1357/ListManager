using ListManager.ViewModels;

namespace ListManager.Views;

public partial class ShoppingListsPage : ContentPage
{
	ShoppingListsViewModel viewModel;
	public ShoppingListsPage(ShoppingListsViewModel viewModel)
	{
		BindingContext = this.viewModel = viewModel;

		InitializeComponent();
	}
}