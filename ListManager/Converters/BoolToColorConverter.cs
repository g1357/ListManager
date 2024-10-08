using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListManager.Converters;

/// <summary>
/// Конвертировать логическое значение в цвет.
/// </summary>
public class BoolToColorConverter : IValueConverter
{
    /// <summary>
    /// Цвет при истинном значении.
    /// </summary>
    public Color TrueColor { get; set; } = Color.FromArgb("#bcacdc");

    /// <summary>
    /// Цвет при ложном значении.
    /// </summary>
    public Color FalseColor { get; set; } = Color.FromArgb("#ffffff");

    /// <summary>
    /// Конвертировать логическое значение в цвет
    /// </summary>
    /// <param name="value">Значение (истина или ложь)</param>
    /// <param name="tergetType">Тип результата</param>
    /// <param name="parameter">Параметер</param>
    /// <param name="culture">Информация о культуре</param>
    /// <returns>Цвет</returns>
    public object? Convert(object? value, Type tergetType, object? parameter,
        CultureInfo culture)
    {
        var  boolValue = (bool?)value;
        var result = (boolValue ?? false) ? TrueColor : FalseColor;
        return result;
    }

    /// <summary>
    /// Корвертировать обратно.
    /// Не используется.
    /// </summary>
    /// <param name="value">Значение (цвет)</param>
    /// <param name="tergetType">Тип результата</param>
    /// <param name="parameter">Параметр</param>
    /// <param name="culture">Информация о культуре</param>
    /// <returns>Результат</returns>
    public object? ConvertBack(object? value, Type tergetType, object? parameter,
        CultureInfo culture)
    {
        // Вернуть входящее значение
        return value;
    }
}
