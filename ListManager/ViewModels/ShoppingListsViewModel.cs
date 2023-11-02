﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ListManager.Models;
using ListManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListManager.ViewModels;

public partial class ShoppingListsViewModel : ViewModelBase
{
    // Сервис данных
    private readonly IDataService dataService;
    // Сервис навигации между страницами
    private readonly INavigationService navigationService;
    // Сервис диалогов
    private readonly IDialogService dialogService;

    public ObservableCollection<ShoppingList> ShoppingLists { get; set; }

    [ObservableProperty]
    private ShoppingList? _selectedItem;

    [RelayCommand]
    private async Task SelectionChangedAsync(ShoppingList  shoppingList)
    {
        await dialogService.DisplayAlert("Selected item",
            $"{shoppingList.Name} ({shoppingList.Description})", "Ok");
    }

    [ObservableProperty]
    private bool _refreshingFlag = true;

    [RelayCommand]
    private async Task RefreshListAsync()
    {
        RefreshingFlag = true;

        var list = await dataService.GetShoppingLists();
        ShoppingLists = new ObservableCollection<ShoppingList>();
        foreach (var item in list)
        {
            ShoppingLists.Add(item);
        }

        OnPropertyChanged(nameof(ShoppingLists));
        RefreshingFlag = false;
        return;
    }

    public ShoppingListsViewModel(IDataService dataService,
        INavigationService navigationService, IDialogService dialogService)
    {
        this.dataService = dataService;
        this.navigationService = navigationService;
        this.dialogService = dialogService;

        //RefreshList();
        var list = dataService.GetShoppingLists().Result;
        ShoppingLists = new ObservableCollection<ShoppingList>();
        foreach (var item in list)
        {
            ShoppingLists.Add(item);
        }

    }
}
