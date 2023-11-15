using CommunityToolkit.Mvvm.ComponentModel;
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
    private ShoppingList? selectedItem;

    [RelayCommand]
    private async Task SelectionChangedAsync(ShoppingList  shoppingList)
    {
        //await dialogService.DisplayAlert("Selected item",
        //    $"{shoppingList.Name} ({shoppingList.Description})", "Ok");

        await navigationService.NavigateToAsync("ShoppingList",
            new Dictionary<string, object>
            {
                { "SelectedItem", shoppingList }
            });

    }

    [ObservableProperty]
    private bool _refreshingFlag = true;

    [RelayCommand]
    private void RefreshList()
    {
        RefreshingFlag = true;

        var list = dataService.GetShoppingLists();
        ShoppingLists = new ObservableCollection<ShoppingList>();
        foreach (var item in list)
        {
            ShoppingLists.Add(item);
        }

        OnPropertyChanged(nameof(ShoppingLists));
        RefreshingFlag = false;
        return;
    }

    [RelayCommand]
    private async Task AddItemAsync()
    {
        await dialogService.DisplayAlert("Add itrm to List",
            "You will add ityem to list!", "Ok");
    }

    public ShoppingListsViewModel(IDataService dataService,
        INavigationService navigationService, IDialogService dialogService)
    {
        this.dataService = dataService;
        this.navigationService = navigationService;
        this.dialogService = dialogService;

        ShoppingLists = new ObservableCollection<ShoppingList>();

        RefreshList();
     }

    internal void OnAppearing()
    {
        RefreshList();
    }
}
