using ListManager.ViewModels;
using System.Diagnostics;

namespace ListManager.Views;

public partial class ShoppingListPage : ContentPage
{
	ShoppingListViewModel viewModel;

	public ShoppingListPage(ShoppingListViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = this.viewModel = viewModel;
	}
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        // Выдать отладочное сообщение
        var m = System.Reflection.MethodBase.GetCurrentMethod();
        Debug.WriteLine($"===== Method : {m?.Name}  of Class:  {m?.DeclaringType?.Name}");

        base.OnNavigatedTo(args);

        //viewModel.OnNavigatedTo();
    }

}