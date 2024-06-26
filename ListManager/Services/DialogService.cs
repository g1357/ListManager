﻿using System.Diagnostics.CodeAnalysis;

// Пространство имён сервисов
namespace ListManager.Services;

/// <summary>
/// Служба диалоговых окон.
/// Реализует интерфейс IDialogService
/// </summary>
public class DialogService : IDialogService
{
    /// <summary>
    /// Показать диалоговое окно с одной кнопкой (закрыть).
    /// Метод асинхронный.
    /// </summary>
    /// <param name="title">Заголовок диалогового окна</param>
    /// <param name="message">Текст сообщения</param>
    /// <param name="buttonLabel">название кнопки "закрыть"</param>
    /// <returns>нет</returns>
    public Task DisplayAlert(string title, string message, string buttonLabel)
    {
        Page? mainPage = Application.Current?.MainPage;
        if (mainPage != null)
        {
            // Вызвать соотвествующий метод из экземпляра корневой страницы
            return mainPage.DisplayAlert(title, message, buttonLabel);
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// Показать диалоговое окно с двумя кнопками (принять, отменить).
    /// Метод асинхронный.
    /// </summary>
    /// <param name="title">Заголовок диалогового окна</param>
    /// <param name="message">Текст сообщения</param>
    /// <param name="acceptLabel">Название кнопки "принять"</param>
    /// <param name="cancelLabel">Название кнопки "отменить"</param>
    /// <returns>true, если нажата кноака "принять", иначе false</returns>
    public Task<bool> DisplayAlert(string title, string message, 
        string acceptLabel, string cancelLabel)
    {
        Page? mainPage = Application.Current?.MainPage;
        if (mainPage != null)
        {
            // Вызвать соотвествующий метод из экземпляра корневой страницы
            return mainPage.DisplayAlert(title, message,
            acceptLabel, cancelLabel);
        }
        return Task.FromResult(false);
    }

    /// <summary>
    /// Показать диалоговое окно с набором действияй и двумя кнопками 
    /// (отменить, разрушить)
    /// Метод асинхронный.
    /// </summary>
    /// <param name="title">Заголовок диалогового окна</param>
    /// <param name="cancelLabel">Название кнопкт "отменить"</param>
    /// <param name="destructionLabel">Название кнопкт "разрушить"</param>
    /// <param name="buttons">список названий пользовательских кнопок.
    /// список или массив.</param>
    /// <returns>Название нажатой кнопки</returns>
    public Task<string> DisplayActionSheet([NotNull] string title, 
        string cancelLabel, string destructionLabel, [NotNull] params string[] buttons)
    {
        Page? mainPage = Application.Current?.MainPage;
        if (mainPage != null)
        {
            // Вызвать соотвествующий метод из экземпляра корневой страницы
            return mainPage.DisplayActionSheet(title, 
            cancelLabel, destructionLabel, buttons);
        }
        return Task.FromResult(string.Empty);
    }

    /// <summary>
    /// Показать диалоговое окно для запроса ввода строкового значения.
    /// Метод асинхронный.
    /// </summary>
    /// <param name="title">Заголовок диалогового окна</param>
    /// <param name="message">Текст запроса</param>
    /// <param name="acceptLabel">Название кнопки "принять"</param>
    /// <param name="cancelLabel">Название кнопки "отменить"></param>
    /// <param name="placeholder">Заполнитель поля ввода</param>
    /// <param name="maxLength">максимальная длина вводимой строки</param>
    /// <param name="keyboard">Тип клавиатуры. Может быть: 
    /// Plain, Chat, Default, Email, Numeric, Telephone, Text, Url</param>
    /// <param name="initialValue">Начальное значение ответа</param>
    /// <returns></returns>
    public Task<string> DisplayPromptAsync(string title, string message, 
        string acceptLabel = "OK", string cancelLabel = "Cancel", 
        string? placeholder = default, int maxLength = -1, 
        Microsoft.Maui.Keyboard? keyboard = default, string initialValue = "")
    {
        Page? mainPage = Application.Current?.MainPage;
        if (mainPage != null)
        {
            // Вызвать соотвествующий метод из экземпляра корневой страницы
            return mainPage.DisplayPromptAsync(title, message,
            acceptLabel, cancelLabel, placeholder, maxLength, keyboard, initialValue);
        }
        return Task.FromResult(string.Empty);
    }
}