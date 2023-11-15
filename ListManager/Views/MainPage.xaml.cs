using ListManager.ViewModels;
using System.Diagnostics;

namespace ListManager.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        // Выдать отладочное сообщение
        Debug.WriteLine($"===== an instance of the class has been created: {nameof(MainPage)}");
        var m = System.Reflection.MethodBase.GetCurrentMethod();
        Debug.WriteLine($"===== Method : {m?.Name} of Class: {m?.DeclaringType?.Name}");

        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        // Выдать отладочное сообщение
        Debug.WriteLine($"===== OnAppearing: {nameof(MainPage)}");
        var m = System.Reflection.MethodBase.GetCurrentMethod();
        Debug.WriteLine($"===== Method : {m?.Name} of Class: {m?.DeclaringType?.Name}");

        base.OnAppearing();
    }
    protected override bool OnBackButtonPressed()
    {
        // Выдать отладочное сообщение
        Debug.WriteLine($"===== OnBackButtonPressed: {nameof(MainPage)}");
        var m = System.Reflection.MethodBase.GetCurrentMethod();
        Debug.WriteLine($"===== Method : {m?.Name} of Class: {m?.DeclaringType?.Name}");

        return base.OnBackButtonPressed();
    }
    protected override void OnDisappearing()
    {
        // Выдать отладочное сообщение
        Debug.WriteLine($"===== OnDisappearing: {nameof(MainPage)}");
        var m = System.Reflection.MethodBase.GetCurrentMethod();
        Debug.WriteLine($"===== Method : {m?.Name} of Class: {m?.DeclaringType?.Name}");

        base.OnDisappearing();
    }
    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        // Выдать отладочное сообщение
        Debug.WriteLine($"===== OnNavigatedFrom: {nameof(MainPage)}");
        var m = System.Reflection.MethodBase.GetCurrentMethod();
        Debug.WriteLine($"===== Method : {m?.Name} of Class: {m?.DeclaringType?.Name}");

        base.OnNavigatedFrom(args);
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        // Выдать отладочное сообщение
        Debug.WriteLine($"===== OnNavigatedTo: {nameof(MainPage)}");
        var m = System.Reflection.MethodBase.GetCurrentMethod();
        Debug.WriteLine($"===== Method : {m?.Name} of Class: {m?.DeclaringType?.Name}");

        base.OnNavigatedTo(args);
    }
    protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        // Выдать отладочное сообщение
        Debug.WriteLine($"===== OnNavigatingFrom: {nameof(MainPage)}");
        var m = System.Reflection.MethodBase.GetCurrentMethod();
        Debug.WriteLine($"===== Method : {m?.Name} of Class: {m?.DeclaringType?.Name}");

        base.OnNavigatingFrom(args);
    }
}