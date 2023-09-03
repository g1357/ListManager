using ListManager.Services;
using System.Windows.Input;

// Пространство имён моделей приложения
namespace ListManager.ViewModels;

/// <summary>
/// Модель предсьавления главной страницы.
/// </summary>
public class MainViewModel : ViewModelBase
{
    // Диалоговый сервис
    private readonly IDialogService dialogService;

    // Счётчик нажатий
    private int count;

    // Поле для хранения значения Текста кнопки
    private string _btnText = "Click Me!";

    /// <summary>
    /// Свойство текста кнопки
    /// </summary>
    public string BtnText
    {
        get => _btnText;
        set => SetProperty(ref _btnText, value);
    }

    /// <summary>
    ///  Свойство команды нажатия на кнопку.
    /// </summary>
    public ICommand ClickCommand { get; }

    /// <summary>
    /// Конструктор модели представления главной страницы
    /// </summary>
    public MainViewModel(IDialogService dialogService)
    {
        // Сохранить диалоговый сервис
        this.dialogService = dialogService;

        // Определить действие при нажатии на кнопку
        ClickCommand = new Command(async () =>
        {
            // Выдать сообщение на экран
            await dialogService.DisplayAlert("Alert", "The button is clicked!", "Ok");

            // Увеличить счётчик надатий
            count++;

            // Установить тект на кноаке в зависимости от числа нажатий
            if (count == 1)
                BtnText = $"Clicked {count} time"; // Ед. число
            else
                BtnText = $"Clicked {count} times"; // Мн. число
        });
    }
}
