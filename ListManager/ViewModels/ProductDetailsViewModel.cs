using CommunityToolkit.Mvvm.ComponentModel;
using ListManager.Models;
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
    [ObservableProperty]
    private Product? _productInfo;

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

    [ObservableProperty]
    private bool _editable;

}
