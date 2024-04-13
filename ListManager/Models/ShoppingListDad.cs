using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Пространство имён моделей данных
namespace ListManager.Models;

/// <summary>
/// Модель данных списка покупок для операций Drag And Drop (перетаскивания).
/// </summary>
public partial class ShoppingListDaD : ShoppingList
{
    /* Свойства, определённые в коассе Shopping List
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
    */

    // Дополнительные свойства для реализации операций перетаскивания
    // (Drag and Drop)

    /// <summary>
    /// Элемент перетаскивается?
    /// </summary>
    [ObservableProperty]
    public bool isBeingDragged;

    /// <summary>
    /// Перетаскиваемый элеиент находится над данным элементом?
    /// </summary>
    [ObservableProperty]
    public bool isBeingDraggedOver;

    public ShoppingListDaD(int listKindId, int id, string name, string? description = null)
    {
        this.ListKindId = listKindId;
        this.Id = id;
        this.Name = name;
        this.Description = description;
        this.isBeingDragged = false;
        this.isBeingDraggedOver = false;
    }

    public ShoppingListDaD(ShoppingList item) :
        this(item.ListKindId, item.Id, item.Name, item.Description)
    { 
    }

    public ShoppingList Base()
    {
        return new ShoppingList
        {
            ListKindId = ListKindId,
            Id = Id,
            Name = Name,
            Description = Description
        };
    }
}
