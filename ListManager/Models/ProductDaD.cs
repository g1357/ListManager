using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListManager.Models;

public partial class ProductDaD : Product
{
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

    public ProductDaD(Product product)
    {
        ListId = product.ListId;
        Id = product.Id;
        Name = product.Name;
        Description = product.Description;
        Qty = product.Qty;
        Marked = product.Marked;
        isBeingDragged = false;
        isBeingDraggedOver = false;
    }

    public Product Base()
    {
        return new Product
        {
            ListId = ListId,
            Id = Id,
            Name = Name,
            Description = Description,
            Qty = Qty,
            Marked = Marked
        };
    }
}
