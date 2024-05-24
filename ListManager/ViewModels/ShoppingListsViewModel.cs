using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ListManager.Models;
using ListManager.Services;
using ListManager.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

    /// <summary>
    /// Список элементов для перетаскивания
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<ShoppingListDaD> shoppingLists;

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
    private bool _refreshingFlag = false;

    [RelayCommand]
    private void RefreshList()
    {
        //RefreshingFlag = true;

        var list = dataService.GetShoppingLists();
        ShoppingLists = new ObservableCollection<ShoppingListDaD>();
        foreach (var item in list)
        {
            ShoppingLists.Add(new ShoppingListDaD(item));
        }

        OnPropertyChanged(nameof(ShoppingLists));
        //RefreshingFlag = false;
        return;
    }

    [RelayCommand]
    private async Task AddItemAsync()
    {
        await dialogService.DisplayAlert("Add item to List",
            "You will add item to list!", "Ok");
        await navigationService.NavigateToAsync("ShoppingListDetails",
            new Dictionary<string, object>
            {
                { "SelectedShoppingList", new ShoppingList() },
                { "Mode", "add" }
            });
    }

    [RelayCommand]
    private async Task HelpAsync()
    {
        //await dialogService.DisplayAlert("Help",
        //    "You asked help! The help is comming!", "Ok");
        //var helpPage = Application.Current.MainPage.Handler.MauiContext.Services.GetService<HelpPage>();
        var helpPage = ServiceHelper.GetService<HelpPage>();
        if (helpPage != null)
        {
            await navigationService.PushModalAsync(helpPage);
        }
    }

    [RelayCommand]
    private async Task TapOnceAsync(ShoppingListDaD list)
    {
        await dialogService.DisplayAlert("Tap Once Gesture",
            $"You have tapped the item \"{list.Name}\".", "Ok");
        //await navigationService.NavigateToAsync("ShoppingList",
        //    new Dictionary<string, object>
        //    {
        //        { "SelectedItem", list }
        //    });
        ShoppingList _list = list.Base();
        await navigationService.NavigateToAsync("ShoppingList",
            new Dictionary<string, object>
            {
                { "SelectedItem", _list }
            });
    }

    [RelayCommand]
    private async Task TapTwiceAsync(ShoppingList list)
    {
        await dialogService.DisplayAlert("Tap Twice Gesture",
            "You asked help! The help is comming!", "Ok");
        //var helpPage = Application.Current.MainPage.Handler.MauiContext.Services.GetService<HelpPage>();
        //var helpPage = ServiceHelper.GetService<HelpPage>();
        //await navigationService.PushModalAsync(helpPage);
    }

    [RelayCommand]
    private async Task DeleteAsync(ShoppingList list)
    {
        await dialogService.DisplayAlert("Delete Command",
            "Delete Command Button Pressed!", "Ok");
        //var helpPage = Application.Current.MainPage.Handler.MauiContext.Services.GetService<HelpPage>();
        //var helpPage = ServiceHelper.GetService<HelpPage>();
        //await navigationService.PushModalAsync(helpPage);
    }

    /// <summary>
    /// Перетаскиваемый элемент списка
    /// </summary>
    internal ShoppingListDaD? _itemBeingDragged;

    /// <summary>
    /// Взять элемент для перетаскивания.
    /// Начало операции перетаскивания.
    /// </summary>
    /// <param name="item">Элемент списка, выбранный для перетаскивания</param>
    [RelayCommand]
    internal void ItemDragged(ShoppingListDaD item)
    {
        // Установить элементу признак того, что он перетаскивается
        item.IsBeingDragged = true;
        // Сохранить перетаскиваемый элемент в специальной переменной
        _itemBeingDragged = item;
    }

    /// <summary>
    /// Отметить элемент над которым находится перетаскиваемый элемент
    /// </summary>
    /// <param name="item"></param>
    [RelayCommand]
    internal void ItemDraggedOver(ShoppingListDaD item)
    {
        // Если перетаскиваемый элемент находится над собой, то
        if (item == _itemBeingDragged)
        {
            item.IsBeingDragged = false;
        }
        // иначе, установить флаг того, что над данным элементом находится перетаскиваемый
        // для ситуации, кроме того когда перетаскиваемый над собой
        item.IsBeingDraggedOver = item != _itemBeingDragged;
    }

    /// <summary>
    /// Уйти с элемента над которым находился перетаскиваемый
    /// </summary>
    /// <param name="item">Элемент с которого ушёл перетаскиваемый</param>
    [RelayCommand]
    internal void ItemDragLeave(ShoppingListDaD item)
    {
        // Снять элементу признак того, что он перетаскивается
        item.IsBeingDraggedOver = false;
    }

    /// <summary>
    /// Бросить пертаскиваемый элемент.
    /// </summary>
    /// <param name="item">Элемент над которым бросается перетаскиваемый элемент</param>
    [RelayCommand]
    internal void ItemDropped(ShoppingListDaD item)
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
            int insertAtIndex = ShoppingLists.IndexOf(itemToInsertBefore);
            if (insertAtIndex >= 0 && insertAtIndex < ShoppingLists.Count)
            {
                // Удалить перемещаемый элемент из списка
                ShoppingLists.Remove(itemToMove);
                // Вставить перемещаемый элемент по заданному индексу
                ShoppingLists.Insert(insertAtIndex, itemToMove);

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
    }


    public ShoppingListsViewModel(IDataService dataService,
        INavigationService navigationService, IDialogService dialogService)
    {
        this.dataService = dataService;
        this.navigationService = navigationService;
        this.dialogService = dialogService;

        ShoppingLists = new ObservableCollection<ShoppingListDaD>();

        //RefreshList();
     }

    internal void OnAppearing()
    {
        RefreshList();
    }

    internal void OnNavigatedTo()
    {
        SelectedItem = null;
    }
}
