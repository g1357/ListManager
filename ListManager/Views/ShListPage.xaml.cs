using ListManager.ViewModels;

namespace ListManager.Views;

public partial class ShListPage : ContentPage
{
	ShListViewModel viewmodel;
	public ShListPage(ShListViewModel viewmodel)
	{
		InitializeComponent();

		BindingContext = this.viewmodel= viewmodel;
	}
}