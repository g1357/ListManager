using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Пространство имён моделей данных
namespace ListManager.Models;

/// <summary>
/// Вид списка.
/// Например: список покупок, список дел и т.п.
/// </summary>
public class ListKind
{
    /// <summary>
    /// Идентификатор списка в системе.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Наименование списка.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Краткое описание содержимого списка.
    /// </summary>
    public string Description { get; set; }
}