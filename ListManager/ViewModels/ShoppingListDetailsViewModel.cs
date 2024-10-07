using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ListManager.Models;
using ListManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListManager.ViewModels;

// Параметры: Выбранный продукт для создания/просмотра/редактирования/удаления.
// При создании в продукте передаётмя только мдентификатор списка.
[QueryProperty(nameof(ShoppingListInfo), "SelectedShoppingList")]
// Режим работы представления: просмотр, добавление, редактирование
[QueryProperty(nameof(Mode), "Mode")]
public partial class ShoppingListDetailsViewModel : ViewModelBase
{
    #region Сервисы
    // Сервис данных
    private readonly IDataService dataService;
    // Сервис навигации между страницами
    private readonly INavigationService navigationService;
    // Сервис диалогов
    private readonly IDialogService dialogService;
    #endregion Сервисы

    #region Параметры страницы
    private ShoppingList? _shoppingListInfo;
    public ShoppingList? ShoppingListInfo
    {
        get => _shoppingListInfo;
        set
        {
            if (SetProperty(ref _shoppingListInfo, value))
            {
                ListName = _shoppingListInfo?.Name ?? string.Empty; 
                ListDescription = _shoppingListInfo?.Description;
                ListFavourite = _shoppingListInfo?.Favourite ?? false;
                dataChanged = false;
                SaveCommand.NotifyCanExecuteChanged();
                CancelCommand.NotifyCanExecuteChanged();
            }
        }
    }

    private string? _mode;
    public string? Mode
    {
        get => _mode;
        set
        {
            if (SetProperty(ref _mode, value))
            {
                switch (_mode)
                {
                    case "view":
                        Editable = false;
                        break;
                    case "add":
                        Editable = true;
                        break;
                    case "edit":
                        Editable = true;
                        break;
                    default: // Внутренняя ошибка приложения
                        break;
                }
            }
        }
    }
    #endregion Параметры страницы

    #region Поля формы редактирования продукта
    private string _listName = string.Empty;
    public string ListName
    {
        get => _listName;
        set
       {
            if (SetProperty(ref _listName, value))
            {
                CheckChanges();
            }
        }
    }

    private string? _listDescription;
    public string? ListDescription
    {
        get => _listDescription;
        set
        {
            if (SetProperty(ref _listDescription, value))
            {
                CheckChanges();
            }
        }
    }

    private bool _listFavourite;
    public bool ListFavourite
    {
        get => _listFavourite;
        set
        {
            if (SetProperty(ref _listFavourite, value))
            {
                CheckChanges();
            }
        }
    }

    void CheckChanges()
    {
        if (_listName == _shoppingListInfo?.Name &&
        _listDescription == _shoppingListInfo.Description &&
        _listFavourite == _shoppingListInfo.Favourite)
        {
            dataChanged = false;
            SaveCommand.NotifyCanExecuteChanged();
            CancelCommand.NotifyCanExecuteChanged();
        }
        else
        {
            dataChanged = true;
            SaveCommand.NotifyCanExecuteChanged();
            CancelCommand.NotifyCanExecuteChanged();
        }
    }

    #endregion Поля формы редактирования продукта

    /// <summary>
    /// Свойство возможности редактирования формы
    /// </summary>
    [ObservableProperty]
    private bool _editable;

    // Признак изменения данные о продукте в форме
    private bool dataChanged = false;

    #region Кнопки формы страницы

    [RelayCommand]
    private async Task AddItemAsync()
    {
        await dialogService.DisplayAlert("Add Item",
            "Add Item Button is clicked!", "Ok");
    }

    [RelayCommand]
    private async Task EditItemAsync()
    {
        await dialogService.DisplayAlert("Edit Item",
            "Edit Item Button is clicked!", "Ok");
    }

    [RelayCommand]
    private async Task DeleteItemAsync()
    {
        await dialogService.DisplayAlert("Delete Item",
            "Delete Item Button is clicked!", "Ok");
    }

    [RelayCommand(CanExecute = nameof(IsSaveBtnEnabled))]
    private async Task SaveAsync()
    {
        ShoppingList newList;

        switch (Mode)
        {
            case "edit":
                newList = new ShoppingList
                {
                    ListKindId = _shoppingListInfo?.ListKindId ?? 0,
                    Id = _shoppingListInfo?.Id ?? 0,
                    Name = _listName,
                    Description = _listDescription,
                    Favourite = _listFavourite
                };
                bool res = dataService.UpdateShoppingList(newList);
                break;
            case "add":
                //newList = new ShoppingList
                //{
                //    ListKindId = _shoppingListInfo.ListKindId,
                //    Id = 0, // Будет установлен при добавлении в хранилище данных
                //    Name = _listName,
                //    Description = _listDescription,
                //};
                var id = dataService.CreateShoppingList(_listName, _listDescription,
                    _listFavourite);
                break;
        }
        await navigationService.NavigateBackAsync();
    }
    public bool IsSaveBtnEnabled => dataChanged;

    [RelayCommand(CanExecute = nameof(IsCancelBtnEnabled))]
    private async Task CancelAsync()
    {
        await navigationService.NavigateBackAsync();
    }

    public bool IsCancelBtnEnabled => dataChanged;

    #endregion Кнопки формы страницы

    public ShoppingListDetailsViewModel(IDataService dataService,
        INavigationService navigationService, IDialogService dialogService)
    {
        this.dataService = dataService;
        this.navigationService = navigationService;
        this.dialogService = dialogService;
    }

}
