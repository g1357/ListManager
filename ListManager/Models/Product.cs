using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Пространство имён моделей данных
namespace ListManager.Models;

/// <summary>
/// Модель данных о продукте.
/// </summary>
public class Product : ObservableObject
{
    /// <summary>
    /// Идентификатор списка покупок
    /// </summary>
    public int ListId { get; set; }

    /// <summary>
    /// Идентификатор продукта
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название продукта
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Краткое описание продукта
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Количество продукта
    /// </summary>
    public decimal Qty { get; set; }

    /// <summary>
    /// Единица измерения продукта.
    /// </summary>
    public string? Unit {  get; set; }

    /// <summary>
    /// Отмечен (куплен)
    /// </summary>
    public bool Marked { get; set; }

    public Product(Product product)
    {
        ListId = product.ListId;
        Id = product.Id;
        Name = product.Name;
        Description = product.Description;
        Qty = product.Qty;
        Unit = product.Unit;
        Marked = product.Marked;
    }
    public Product() { }
}
