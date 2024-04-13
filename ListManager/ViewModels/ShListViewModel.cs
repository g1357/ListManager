using ListManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListManager.ViewModels;
/// <summary>
/// Модель представления списка покупок.
/// </summary>
// Переданный параметр: выбранный список покупок
[QueryProperty(nameof(CurrentShoppingList), "SelectedItem")]

public class ShListViewModel :ViewModelBase
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
            }
        }
    }
}
