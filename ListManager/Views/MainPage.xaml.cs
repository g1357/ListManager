﻿using ListManager.ViewModels;
using System.Diagnostics;

namespace ListManager.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        // Выдать отладочное сообщение
        Debug.WriteLine($"===== an instance of the class has been created: {nameof(MainPage)}");

        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        // Выдать отладочное сообщение
        Debug.WriteLine($"===== OnAppearing: {nameof(MainPage)}");

        base.OnAppearing();
    }
    protected override bool OnBackButtonPressed()
    {
        // Выдать отладочное сообщение
        Debug.WriteLine($"===== aOnBackButtonPressed: {nameof(MainPage)}");

        return base.OnBackButtonPressed();
    }
    protected override void OnDisappearing()
    {
        // Выдать отладочное сообщение
        Debug.WriteLine($"===== OnDisappearing: {nameof(MainPage)}");

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
        Debug.WriteLine($"===== OnNavigatedTo: {nameof(MainPage)}");

        base.OnNavigatedTo(args);
    }
    protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        // Выдать отладочное сообщение
        Debug.WriteLine($"===== OnNavigatingFrom: {nameof(MainPage)}");

        base.OnNavigatingFrom(args);
    }
}