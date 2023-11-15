using ListManager.ViewModels;

namespace ListManager.Views;

public partial class ShoppingListPage : ContentPage
{
	ShoppingListViewModel viewModel;

	public ShoppingListPage(ShoppingListViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = this.viewModel = viewModel;
	}
}