using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ListManager.Models;
using ListManager.Popups;
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

    /// <summary>
    /// Свойство: флаг обновления
    /// </summary>
    [ObservableProperty]
    private bool _refreshingFlag = true;

    /// <summary>
    /// Команда обновления списка
    /// </summary>
    [RelayCommand]
    private void RefreshList()
    {
        //RefreshingFlag = true;

        var list = dataService.GetShoppingLists();
        ShoppingLists = [];
        ShoppingListDaD shList;
        foreach (var item in list)
        {
            shList = new ShoppingListDaD(item);
            shList.ProdQty = dataService.GetProductQty(shList.Id);
            ShoppingLists.Add(shList);
        }

        OnPropertyChanged(nameof(ShoppingLists));
        RefreshingFlag = false;
        return;
    }

    /// <summary>
    /// Команда добавления элемента: списка покупок.
    /// Создаёт новый список покупок, позволяет задать его наименование и описание,
    /// создаёт пустой список продуктов.
    /// </summary>
    /// <returns>не возвращает значений</returns>
    [RelayCommand]
    private async Task AddItemAsync()
    {
        await dialogService.DisplayAlert("Add itrm to List",
            "You will add ityem to list!", "Ok");
        await navigationService.NavigateToAsync("ShoppingListDetails",
            new Dictionary<string, object>
            {
                { "SelectedShoppingList", new ShoppingList() },
                { "Mode", "add" }
            });
    }

    /// <summary>
    /// Команда показа подсказки.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Команда выбора списка покупок одиночным нажатием на него.
    /// </summary>
    /// <param name="list">Список покупок</param>
    /// <returns>не возвращает значений</returns>
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

    /// <summary>
    /// Команда - 
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    [RelayCommand]
    private async Task TapTwiceAsync(ShoppingListDaD list)
    {
        await dialogService.DisplayAlert("Tap Twice Gesture",
            "You asked help! The help is comming!", "Ok");
        //var helpPage = Application.Current.MainPage.Handler.MauiContext.Services.GetService<HelpPage>();
        //var helpPage = ServiceHelper.GetService<HelpPage>();
        //await navigationService.PushModalAsync(helpPage);
        await EditAsync(list);
    }

    /// <summary>
    /// Команда: удалить список покупок.
    /// </summary>
    /// <param name="list">Удаляемый список покупок</param>
    /// <returns>не возвращает значений</returns>
    [RelayCommand]
    private async Task DeleteAsync(ShoppingList list)
    {
        // Подтвердить удаление списка покупок
        var result = await dialogService.DisplayAlert("Delete Command",
            "Delete Command Button Pressed!", "Ok", "Cancel");
        if (result)
        {
            // Удалить список покупок по идентификатору
            var res = dataService.DeleteShoppingList(list.Id);
            if (!res)
            {
                await dialogService.DisplayAlert("Delete Shopping List",
                    "Some problem with Shopping List Deleting!", "Close");
            }
            // Обновить перегень списков покупок
            RefreshingFlag = true; // Установка флага вызвает исполнени командв обновления
        }
    }

    /// <summary>
    /// Переключть звёздочку у списка покупок.
    /// Выполнение команды изменяет значение звёздочки.
    /// </summary>
    /// <param name="list">Список покупок для отметки</param>
    /// <returns>не возвращает значение</returns>
    [RelayCommand]
    private async Task FavouriteAsync(ShoppingList list)
    {
        await dialogService.DisplayAlert("Favourite List",
            "Toggle Favourite List", "Ok");
        // Переключить звёздочку у списка покупок
        var res = dataService.FavToggleShoppingList(list.Id);
        if (!res)
        {
            // Сообщить о внутренней ошибке
            await dialogService.DisplayAlert("Delete Shopping List",
                "Some problem with Shopping List Deleting!", "Close");
        }
        // Обновить перечень списков покупок
        RefreshingFlag = true; // При установке флага вызывается команда обновления
    }

    /// <summary>
    /// Редактировать заголовок списка покупок.
    /// </summary>
    /// <param name="list">Список покупок</param>
    /// <returns>не возвращает значен7ий</returns>
    [RelayCommand]
    private async Task EditAsync(ShoppingListDaD list)
    {
        // Выдать информационное сообщение
        await dialogService.DisplayAlert("Edit Shopping List",
            "Editing the header of shopping list.", "Ok");
        ShoppingList _list = list.Base();
        // Перейти на страницу редактирования заголовка списка покупок
        await navigationService.NavigateToAsync("ShoppingListDetails",
            new Dictionary<string, object>
            {
                { "SelectedShoppingList", _list },
                { "Mode", "edit" }
            });
    }

    /// <returns>не возвращает значение</returns>
    [RelayCommand]
    private async Task CopyAsync(ShoppingListDaD list)
    {
        await dialogService.DisplayAlert("Copying thr Shopping List",
            "The Shopping List will be Copied", "Ok");

        object? result = await navigationService.ShowPopupAsync(new EditListHeaderPopup(list.Base()));

        if (result is ShoppingList shList)
        {
            var id = dataService.CreateShoppingList(shList.Name,
                shList.Description, shList.Favourite);
            var prodList = dataService.GetProductList(list.Id);
            bool res;
            //Product newProduct;
            foreach (var prod in prodList.ToList())
            {
                //newProduct = new Product(prod);
                prod.ListId = id;
                res = dataService.AddProduct(prod);
            }
        }
        else
        {
            // Сообщить о внутренней ошибке
            await dialogService.DisplayAlert("Copying thr Shopping List",
                "Some problem with Shopping List opying!", "Close");
        }
        // Обновить перечень списков покупок
        RefreshingFlag = true; // При установке флага вызывается команда обновления
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

        ShoppingLists = [];

        //RefreshList();
     }

    internal void OnAppearing()
    {
        //RefreshList();
        RefreshingFlag = true; // Установка флага инициирует выполнения команды
    }

    internal void OnNavigatedTo()
    {
        SelectedItem = null;
    }
}
