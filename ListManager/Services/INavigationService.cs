// Пространство имён сервисов
namespace ListManager.Services;

public interface INavigationService
{
    /// <summary>
    /// Перейти по заданному пути к соответствующей странице (представлению).
    /// Асинхронный метод.
    /// </summary>
    /// <param name="route">Путь, связанный со страницей.</param>
    /// <param name="routeParameters">Передаваемые параметры.</param>
    /// <returns>Нет</returns>
    public Task NavigateToAsync(string route,
        IDictionary<string, object>? routeParameters = null);

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
        IDictionary<string, object>? routeParameters = null);

    /// <summary>
    /// Вернуться к предыдущей страницы (из стека навигации).
    /// Асинхронный метод.
    /// </summary>
    /// <returns>Нет</returns>
    public Task NavigateBackAsync();

    /// <summary>
    /// Вернуться к предыдущей страницы (из стека навигации).
    /// Асинхронный метод.
    /// </summary>
    /// <param name="parameters">Передаваемые параметры.</param>
    /// <returns>Нет</returns>
    public Task NavigateBackAsync(IDictionary<string, object>? parameters = null);

    /// <summary>
    /// Проверка возможности перехода к предыдущей страницы=е
    /// </summary>
    /// <returns>true, если возврат возможен, иначе - афдыу</returns>
    public bool CanNavigateBack();

    public Task PushModalAsync(Page page);
    public Task PopModalAsync();

}