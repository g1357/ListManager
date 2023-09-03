using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Пространство имён сервисов
namespace ListManager.Services;

/// <summary>
/// Сервис навигации между страницами (представлениями) по заданному пути (URI).
/// Реализует интерфейс INavigationService
/// </summary>
public class NavigationService : INavigationService
{
    /// <summary>
    /// Перейти по заданному пути к соответствующей странице (представлению).
    /// Асинхронный метод.
    /// </summary>
    /// <param name="route">Путь, связанный со страницей.</param>
    /// <param name="routeParameters">Передаваемые параметры.</param>
    /// <returns>Нет</returns>
    public Task NavigateToAsync(string route, 
        IDictionary<string, object>? routeParameters = null)
    {
        // Преобразовать путь в состояний навигации
        var shellNavigation = new ShellNavigationState(route);

        // В зависимости от наличия параметров выполнить переход.
        return routeParameters != null
            ? Shell.Current.GoToAsync(shellNavigation, routeParameters)
            : Shell.Current.GoToAsync(shellNavigation);
    }

    /// <summary>
    /// Перейти по заданному пути к соответствующей странице (представлению)
    /// с возможностью указания отработки заданной анимации.
    /// Асинхронный метод.
    /// </summary>
    /// <param name="route">Путь, связанный со страницей.</param>
    /// <param name="animate">Признак отработки анимации</param>
    /// <param name="routeParameters">Передаваемые параметры.</param>
    /// <returns>Нет</returns>
    public Task NavigateToAsync(string route, bool animate,
        IDictionary<string, object>? routeParameters = null)
    {
        var shellNavigation = new ShellNavigationState(route);

        return routeParameters != null
            ? Shell.Current.GoToAsync(shellNavigation, animate, routeParameters)
            : Shell.Current.GoToAsync(shellNavigation, animate);
    }

    /// <summary>
    /// Вернуться к предыдущей страницы (из стека навигации).
    /// Асинхронный метод.
    /// </summary>
    /// <returns>Нет</returns>
    public Task NavigateBackAsync() => Shell.Current.GoToAsync("..");

    /// <summary>
    /// Вернуться к предыдущей страницы (из стека навигации).
    /// Асинхронный метод.
    /// </summary>
    /// <param name="parameters">Передаваемые параметры.</param>
    /// <returns>Нет</returns>
    public Task NavigateBackAsync(IDictionary<string, object>? parameters = null) => 
        Shell.Current.GoToAsync("..", parameters);

}
