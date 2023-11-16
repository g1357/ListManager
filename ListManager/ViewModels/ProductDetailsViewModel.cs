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

/// <summary>
/// Модель представления детфльной информации о продукте.
/// </summary>
[QueryProperty(nameof(ProductInfo), "SelectedProduct")]
[QueryProperty(nameof(Mode), "Mode")]

public partial class ProductDetailsViewModel : ViewModelBase
{
    // Сервис данных
    private readonly IDataService dataService;
    // Сервис навигации между страницами
    private readonly INavigationService navigationService;
    // Сервис диалогов
    private readonly IDialogService dialogService;


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
                    default:
                        break;
                }
            }
        }
    }

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

    private int _productQty;
    public int ProductQty
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

    [ObservableProperty]
    private bool _editable;

    private bool dataChanged = false;

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
                    Id = 0, // _productInfo.Id,
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

    public ProductDetailsViewModel(IDataService dataService,
        INavigationService navigationService, IDialogService dialogService)
    {
        this.dataService = dataService;
        this.navigationService = navigationService;
        this.dialogService = dialogService;
    }
}
