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

/// <summary>
/// Модель представления списка покупок.
/// </summary>
[QueryProperty(nameof(CurrentShoppingList), "SelectedItem")]
public partial class ShoppingListViewModel : ViewModelBase
{
    // Сервис данных
    private readonly IDataService dataService;
    // Сервис навигации между страницами
    private readonly INavigationService navigationService;
    // Сервис диалогов
    private readonly IDialogService dialogService;

    public ObservableCollection<Product> ProductList { get; set; }
    
    [ObservableProperty]
    private ShoppingList? selectedItem;

    /// <summary>
    /// Текущий список покупок
    /// </summary>
    private ShoppingList? _currentShoppingList;
    public ShoppingList? CurrentShoppingList
    {
        get => _currentShoppingList;
        set
        {
            if (SetProperty(ref _currentShoppingList, value))
            {
                RefreshList();
            }
        }
    }

    [ObservableProperty]
    private bool _refreshingFlag = true;

    [RelayCommand]
    private void RefreshList()
    {
        RefreshingFlag = true;
        if (CurrentShoppingList != null)
        {
            var list = dataService.GetProductList(CurrentShoppingList.Id);
            ProductList = new ObservableCollection<Product>();
            Product newProduct;
            foreach (var item in list)
            {
                newProduct = new Product(item);
                ProductList.Add(newProduct);
            }
        }
        OnPropertyChanged(nameof(ProductList));
        OnPropertyChanged(nameof(CurrentShoppingList));
        RefreshingFlag = false;
        return;
    }

    [RelayCommand]
    private async Task SelectionChangedAsync(Product product)
    {
        //await dialogService.DisplayAlert("Selected item",
        //    $"{product.Name} ({product.Description})", "Ok");
       
    }

    [RelayCommand]
    private async Task CheckedChangedAsync(Product product)
    {
        if (product == null) return;

        //await dialogService.DisplayAlert("Chacked Changed item",
        //    $"{product.Name} ({product.Description}) value: {product.Marked}", "Ok");
        dataService.UpdateProduct(product);
    }

    public ShoppingListViewModel(IDataService dataService,
        INavigationService navigationService, IDialogService dialogService)
    {
        this.dataService = dataService;
        this.navigationService = navigationService;
        this.dialogService = dialogService;

        ProductList = new ObservableCollection<Product>();
    }
}
