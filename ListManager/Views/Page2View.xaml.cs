using ListManager.ViewModels;
using System.Diagnostics;

namespace ListManager.Views;

public partial class Page2View : ContentPage
{
	public Page2View(Page2ViewModel viewModel)
	{
        // Выдать отладочное сообщение
        Debug.WriteLine($"===== an instance of the class has been created: {nameof(Page2View)}");

        InitializeComponent();
		BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        // Выдать отладочное сообщение
        Debug.WriteLine($"===== OnAppearing: {nameof(Page2View)}");

        base.OnAppearing();
    }
    // Выполняется при нажатии кноаки "Назад" устройства.
    protected override bool OnBackButtonPressed()
    {
        // Выдать отладочное сообщение
        Debug.WriteLine($"===== aOnBackButtonPressed: {nameof(Page2View)}");

        return base.OnBackButtonPressed();
    }
    protected override void OnDisappearing()
    {
        // Выдать отладочное сообщение
        Debug.WriteLine($"===== OnDisappearing: {nameof(Page2View)}");

        base.OnDisappearing();
    }
    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        // Выдать отладочное сообщение
        Debug.WriteLine($"===== OnNavigatedFrom: {nameof(MainPage)}");

        base.OnNavigatedFrom(args);
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        // Выдать отладочное сообщение
        Debug.WriteLine($"===== OnNavigatedTo: {nameof(Page2View)}");

        base.OnNavigatedTo(args);
    }
    protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        // Выдать отладочное сообщение
        Debug.WriteLine($"===== OnNavigatingFrom: {nameof(Page2View)}");

        base.OnNavigatingFrom(args);
    }
}
