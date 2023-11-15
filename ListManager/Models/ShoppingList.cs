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
public class ShoppingList
{
    /// <summary>
    /// Идентификатор вида списка.
    /// Должен соответствовать одному из идентификаторов вида списка.
    /// </summary>
    public int ListKindId { get; set; }

    /// <summary>
    /// Идентификатор списка в системе.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Наименование списка.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Краткое описание списка.
    /// </summary>
    public string? Description { get; set; }
}
