using ListManager.ViewModels;
using System.Diagnostics;

namespace ListManager.Views;

public partial class ShoppingListsPage : ContentPage
{
	ShoppingListsViewModel viewModel;
	public ShoppingListsPage(ShoppingListsViewModel viewModel)
	{
        // Выдать отладочное сообщение
        var m = System.Reflection.MethodBase.GetCurrentMethod();
        Debug.WriteLine($"===== Method : {m?.Name} of Class: {m?.DeclaringType?.Name}");

        BindingContext = this.viewModel = viewModel;

		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        // Выдать отладочное сообщение
        var m = System.Reflection.MethodBase.GetCurrentMethod();
        Debug.WriteLine($"===== Method : {m?.Name} of Class: {m?.DeclaringType?.Name}");

        base.OnAppearing();
        viewModel.OnAppearing();
    }
    protected override bool OnBackButtonPressed()
    {
        // Выдать отладочное сообщение
        var m = System.Reflection.MethodBase.GetCurrentMethod();
        Debug.WriteLine($"===== Method : {m?.Name} of Class: {m?.DeclaringType?.Name}");

        return base.OnBackButtonPressed();
    }
    protected override void OnDisappearing()
    {
        // Выдать отладочное сообщение
        var m = System.Reflection.MethodBase.GetCurrentMethod();
        Debug.WriteLine($"===== Method : {m?.Name} of Class: {m?.DeclaringType?.Name}");

        base.OnDisappearing();
    }
    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        // Выдать отладочное сообщение
        var m = System.Reflection.MethodBase.GetCurrentMethod();
        Debug.WriteLine($"===== Method : {m?.Name}  of Class:  {m?.DeclaringType?.Name}");

        base.OnNavigatedFrom(args);
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        // Выдать отладочное сообщение
        var m = System.Reflection.MethodBase.GetCurrentMethod();
        Debug.WriteLine($"===== Method : {m?.Name}  of Class:  {m?.DeclaringType?.Name}");

        base.OnNavigatedTo(args);
    }
    protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        // Выдать отладочное сообщение
        var m = System.Reflection.MethodBase.GetCurrentMethod();
        Debug.WriteLine($"===== Method : {m?.Name}  of Class:  {m?.DeclaringType?.Name}");

        base.OnNavigatingFrom(args);
    }
}