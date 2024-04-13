using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Пространство имён моделей данных
namespace ListManager.Models;

/// <summary>
/// Модель данных списка покупок.
/// </summary>
public partial class ShoppingList : ObservableObject
{ 
    /// <summary>
    /// Идентификатор вида списка.
    /// Должен соответствовать одному из идентификаторов вида списка.
    /// </summary>
    [ObservableProperty]
    private int listKindId;

    /// <summary>
    /// Идентификатор списка в системе.
    /// </summary>
    [ObservableProperty]
    private int id;

    /// <summary>
    /// Наименование списка.
    /// </summary>
    [ObservableProperty]
    private string name = string.Empty;

    /// <summary>
    /// Краткое описание списка.
    /// </summary>
    [ObservableProperty]
    private string? description = string.Empty;
}
