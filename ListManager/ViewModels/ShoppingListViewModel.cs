using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ListManager.Models;
using ListManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListManager.ViewModels;

/// <summary>
/// Модель представления списка покупок.
/// </summary>
// Переданный параметр: выбранный список покупок
[QueryProperty(nameof(CurrentShoppingList), "SelectedItem")]
public partial class ShoppingListViewModel : ViewModelBase
{
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
                //RefreshList();
                RefreshingFlag = true;
            }
        }
    }

    // Сервис данных
    private readonly IDataService dataService;
    // Сервис навигации между страницами
    private readonly INavigationService navigationService;
    // Сервис диалогов
    private readonly IDialogService dialogService;

    /// <summary>
    /// Список элементов для перетаскивания
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<ProductDaD> productList;

    /// <summary>
    /// Перетаскиваемый элемент списка
    /// </summary>
    internal ProductDaD? _itemBeingDragged;

    private ProductDaD? _selectedItem;
    public ProductDaD? SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (SetProperty(ref _selectedItem, value))
            {
                EditItemCommand.NotifyCanExecuteChanged();
                DeleteItemCommand.NotifyCanExecuteChanged();
            }
        }
    }

    [ObservableProperty]
    private string message;

    [ObservableProperty]
    private bool _refreshingFlag = true;

    [RelayCommand]
    private void RefreshList()
    {
        //RefreshingFlag = true;
        if (CurrentShoppingList != null)
        {
            var list = dataService.GetProductList(CurrentShoppingList.Id);
            ProductList = new ObservableCollection<ProductDaD>();
            ProductDaD newProduct;
            foreach (var item in list)
            {
                newProduct = new ProductDaD(item);
                ProductList.Add(newProduct);
            }
            OnPropertyChanged(nameof(CurrentShoppingList));
        }
        OnPropertyChanged(nameof(ProductList));
        EditItemCommand.NotifyCanExecuteChanged();
        DeleteItemCommand.NotifyCanExecuteChanged();
        RefreshingFlag = false;
        return;
    }

    [RelayCommand]
    private async Task SelectionChangedAsync(ProductDaD product)
    {
        //await dialogService.DisplayAlert("Selected item",
        //    $"{product.Name} ({product.Description})", "Ok");
        if (SelectedItem == null) return;

        await navigationService.NavigateToAsync("ProductDetails",
            new Dictionary<string, object>
            {
                { "SelectedProduct", product },
                { "Mode", "view" }
            });
    }

    [RelayCommand]
    private async Task CheckedChangedAsync(ProductDaD product)
    {
        if (product == null) return;

        await dialogService.DisplayAlert("Chacked Changed item",
            $"{product.Name} ({product.Description}) value: {product.Marked}", "Ok");
        dataService.UpdateProduct(product);
    }

    [RelayCommand]
    private async Task AddItemAsync()
    {
        await dialogService.DisplayAlert("Add Item",
            $"Add Item Buttom Pressed!", "Ok");
        await navigationService.NavigateToAsync("ProductDetails",
            new Dictionary<string, object>
            {
                { "SelectedProduct", new Product{ ListId = CurrentShoppingList?.Id ?? 0 } },
                { "Mode", "add" }
            });
    }

    [RelayCommand(CanExecute = nameof(IsEditDeleteEnabled))]
    private async Task EditItemAsync()
    {
        if (SelectedItem == null)
        {
            await dialogService.DisplayAlert("Edit Item",
                $"No Selected Item for Editing! Select Item!", "Ok");
        }
        else
        {
            await dialogService.DisplayAlert("Edit Item",
                $"Edit Item Buttom Pressed!\n Item: {SelectedItem.Name}", "Ok");
            await navigationService.NavigateToAsync("ProductDetails",
                new Dictionary<string, object>
                {
                { "SelectedProduct", SelectedItem },
                { "Mode", "edit" }
                });
        }
    }

    [RelayCommand(CanExecute = nameof(IsEditDeleteEnabled))]
    private async Task DeleteItemAsync()
    {
        if (SelectedItem == null)
        {
            await dialogService.DisplayAlert("Delete Item",
                $"No Selected Item for Deleting! Select Item!", "Ok");
        }
        else
        {
            await dialogService.DisplayAlert("Delete Item",
                $"Delete Item Buttom Pressed!\n Item: {SelectedItem.Name}", "Ok");
        }
    }

    [RelayCommand]
    private async Task TapTwiceAsync(ProductDaD product)
    {
        await dialogService.DisplayAlert("Tap Twice Gesture",
            $"You have tapped the item \"{product.Name}\".", "Ok");
        Message = "Tap Twice";
    }

    [RelayCommand]
    private async Task TapOnceAsync(ProductDaD product)
    {
        await dialogService.DisplayAlert("Tap Once Gesture",
            $"You have tapped the item \"{product.Name}\".", "Ok");
        Message = "Tap Once";
        if (product == null) return;

        await navigationService.NavigateToAsync("ProductDetails",
            new Dictionary<string, object>
            {
                { "SelectedProduct", product.Base() },
                { "Mode", "view" }
            });
        
    }

    /// <summary>
    /// Взять элемент для перетаскивания.
    /// Начало операции перетаскивания.
    /// </summary>
    /// <param name="item">Элемент списка, выбранный для перетаскивания</param>
    [RelayCommand]
    internal void ItemDragged(ProductDaD item)
    {
        // Установить элементу признак того, что он перетаскивается
        item.IsBeingDragged = true;
        // Сохранить перетаскиваемый элемент в специальной переменной
        _itemBeingDragged = item;
        Message = $"Item {item.Name} Dragged";
    }

    /// <summary>
    /// Отметить элемент над которым находится перетаскиваемый элемент
    /// </summary>
    /// <param name="item"></param>
    [RelayCommand]
    internal void ItemDraggedOver(ProductDaD item)
    {
        // Если перетаскиваемый элемент находится над собой, то
        if (item == _itemBeingDragged)
        {
            item.IsBeingDragged = false;
        }
        // иначе, установить флаг того, что над данным элементом находится перетаскиваемый
        // для ситуации, кроме того когда перетаскиваемый над собой
        item.IsBeingDraggedOver = item != _itemBeingDragged;
        Message = $"Selected Item is over {item.Name}";
    }

    /// <summary>
    /// Уйти с элемента над которым находился перетаскиваемый
    /// </summary>
    /// <param name="item">Элемент с которого ушёл перетаскиваемый</param>
    [RelayCommand]
    internal void ItemDragLeave(ProductDaD item)
    {
        // Снять элементу признак того, что он перетаскивается
        item.IsBeingDraggedOver = false;
        Message = $"Selected Item has leaved item {item.Name}";
    }

    /// <summary>
    /// Бросить пертаскиваемый элемент.
    /// </summary>
    /// <param name="item">Элемент над которым бросается перетаскиваемый элемент</param>
    [RelayCommand]
    internal void ItemDropped(ProductDaD item)
    {
        try
        {
            // Сохранить перемещаемый элемент
            var itemToMove = _itemBeingDragged;
            // Сохранить эелемент перед которым вставить перетаскиваемый элемент
            var itemToInsertBefore = item;
            // Если один из элементов не задан или это один и тот же елемень,
            // то ничего делать не надо
            if (itemToMove == null || itemToInsertBefore == null ||
                itemToMove == itemToInsertBefore)
            {
                return;
            }
            // Получить индекс элемента перед которым вставлять
            int insertAtIndex = ProductList.IndexOf(itemToInsertBefore);
            if (insertAtIndex >= 0 && insertAtIndex < ProductList.Count)
            {
                // Удалить перемещаемый элемент из списка
                ProductList.Remove(itemToMove);
                // Вставить перемещаемый элемент по заданному индексу
                ProductList.Insert(insertAtIndex, itemToMove);

                // Очистить флаг у перетаскиваемого элемента
                itemToMove.IsBeingDragged = false;
                // Очистить флаг у элемента над которым бросили перетаскиваемый элемент
                itemToInsertBefore.IsBeingDraggedOver = false;
                // Очистить сохранённый перетаскиваемый элемент
                _itemBeingDragged = null;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        Message = $"Selected Item has dropped at item {item.Name}";
    }



    public ShoppingListViewModel(IDataService dataService,
        INavigationService navigationService, IDialogService dialogService)
    {
        this.dataService = dataService;
        this.navigationService = navigationService;
        this.dialogService = dialogService;

        ProductList = new ObservableCollection<ProductDaD>();
    }
    public bool IsEditDeleteEnabled => _selectedItem != null;

    public void OnNavigatedTo()
    {
        //RefreshList();
        RefreshingFlag = true;
        SelectedItem = null;
    }
}
