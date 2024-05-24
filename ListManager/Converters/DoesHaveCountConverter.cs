using CommunityToolkit.Maui.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Пространство имён конвертеров (преобразователей)
namespace ListManager.Converters;

/// <summary>
/// Преобразовать счётчик в логическое значение.
/// Если счётчик больше нуля, то вернуть true (истина),
/// иначе, если счётчик меньше либо равен нулю - false (ложь).
/// </summary>
public class DoesHaveCountConverter : BaseConverterOneWay<int, bool>
{
    // Если преобразование невозможно, то вернуть false (ложь)
    public override bool DefaultConvertReturnValue { get; set; } = false;

    /// <summary>
    /// Преобразовать счётчик в логическое значение.
    /// Истина, если список пуст, ложь в противном случае.
    /// используется для выдачи сообщения, при пустом списке.
    /// </summary>
    /// <param name="value">Значение счётчика</param>
    /// <param name="culture">Языковая культура. Не используется</param>
    /// <returns>Логическое значение. Истина, если в списке нетзначений.</returns>
    public override bool ConvertFrom(int value, CultureInfo? culture)
    {
        // Вернуть истину, если чсписок пуст.
        return value > 0;
    }
}
