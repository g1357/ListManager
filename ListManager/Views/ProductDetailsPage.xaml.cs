using ListManager.ViewModels;

namespace ListManager.Views;

public partial class ProductDetailsPage : ContentPage
{
	ProductDetailsViewModel viewModel;

	public ProductDetailsPage(ProductDetailsViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = this.viewModel = viewModel;
	}
}