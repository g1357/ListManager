using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ListManager.Models;
using ListManager.Services;

namespace ListManager.ViewModels;

/// <summary>
/// Модель представления детальной информации о продукте.
/// </summary>
// Параметр: Выбранный продукт для создания/просмотра/редактирования/удаления.
// При создании в продукте передаётмя только мдентификатор списка.
[QueryProperty(nameof(ProductInfo), "SelectedProduct")]
// Режим работы представления: просмотр, добавление, редактирование
[QueryProperty(nameof(Mode), "Mode")]

public partial class ProductDetailsViewModel : ViewModelBase
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
    private Product? _productInfo;
    public Product? ProductInfo
    {
        get => _productInfo;
        set
        {
            if (SetProperty(ref _productInfo, value))
            {
                ProductName = _productInfo.Name;
                ProductDescription = _productInfo.Description;
                ProductQty = _productInfo.Qty;
                ProductMarked = _productInfo.Marked;
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

    #region Команды меню заголовка страницы
    /// <summary>
    /// Добавить новый продукт в список
    /// </summary>
    /// <returns>нет</returns>
    [RelayCommand]
    private async Task AddItemAsync()
    {
        await dialogService.DisplayAlert("Add Item",
            $"Add Item Buttom Pressed!", "Ok");
    }

    /// <summary>
    /// Перейти из режима просмотра (view) 
    /// в режим редактирования (edit) продукта.
    /// </summary>
    /// <returns>нет</returns>
    [RelayCommand(CanExecute = nameof(IsEditDeleteEnabled))]
    private async Task EditItemAsync()
    {
        await dialogService.DisplayAlert("Edit Item",
            "Edit Item Buttom Pressed!", "Ok");
        Mode = "edit";
        OnPropertyChanged();
    }

    /// <summary>
    /// Удалить текущий продукт.
    /// </summary>
    /// <returns>нет</returns>
    [RelayCommand(CanExecute = nameof(IsEditDeleteEnabled))]
    private async Task DeleteItemAsync()
    {
        if (Mode == "Add")
        {
            await dialogService.DisplayAlert("Delete Item",
                "You can not delete this product.\nIt is not saved yet!",
                "Ok");
        }
        else
        {
            await dialogService.DisplayAlert("Delete Item",
                "Delete Item Buttom Pressed!", "Ok");
        }
    }

    public bool IsEditDeleteEnabled => Mode != "view";

    #endregion Команды меню заголовка страницы

    #region Поля формы редактирования продукта
    private string? _productName;
    public string? ProductName
    {
        get => _productName;
        set
        {
            if (SetProperty(ref _productName, value))
            {
                dataChanged = true;
                SaveCommand.NotifyCanExecuteChanged();
                CancelCommand.NotifyCanExecuteChanged();
            }
        }
    }

    private string? _productDescription;
    public string? ProductDescription
    {
        get => _productDescription;
        set
        {
            if (SetProperty(ref _productDescription, value))
            {
                dataChanged = true;
                SaveCommand.NotifyCanExecuteChanged();
                CancelCommand.NotifyCanExecuteChanged();
            }
        }
    }

    private decimal _productQty;
    public decimal ProductQty
    {
        get => _productQty;
        set
        {
            if (SetProperty(ref _productQty, value))
            {
                dataChanged = true;
                SaveCommand.NotifyCanExecuteChanged();
                CancelCommand.NotifyCanExecuteChanged();
            }
        }
    }

    private bool _productMarked;
    public bool ProductMarked
    {
        get => _productMarked;
        set
        {
            if (SetProperty(ref _productMarked, value))
            {
                dataChanged = true;
                SaveCommand.NotifyCanExecuteChanged();
                CancelCommand.NotifyCanExecuteChanged();
            }
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
    [RelayCommand(CanExecute = nameof(IsSaveBtnEnabled))]
    private async Task SaveAsync()
    {
        Product newProduct;

        switch (Mode)
        {
            case "edit":
                newProduct = new Product
                {
                    ListId = _productInfo.ListId,
                    Id = _productInfo.Id,
                    Name = _productName,
                    Description = _productDescription,
                    Qty = _productQty,
                    Marked = _productMarked
                };
                bool res =  dataService.UpdateProduct(newProduct);
                break;
            case "add":
                newProduct = new Product
                {
                    ListId = _productInfo.ListId,
                    Id = 0, // Будет установлен при добавлении в хранилище данных
                    Name = _productName,
                    Description = _productDescription,
                    Qty = _productQty,
                    Marked = _productMarked
                };
                dataService.AddProduct(newProduct);
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

    public ProductDetailsViewModel(IDataService dataService,
        INavigationService navigationService, IDialogService dialogService)
    {
        this.dataService = dataService;
        this.navigationService = navigationService;
        this.dialogService = dialogService;
    }
}
