using ListManager.Services;
using System.Diagnostics;
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

    // Сервис навигации
    private readonly INavigationService navigationService;

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

    private string _param = string.Empty;                                                 
    public string Param
    {
        get => _param;
        set
        {
            if (SetProperty(ref _param, value))
            {
                ((Command)NavigateCommand).ChangeCanExecute();
            }
        }
    }

    public ICommand NavigateCommand { get; }

    /// <summary>
    /// Свойство возвращает грубину стэка навигации.
    /// </summary>
    public string NavStackCount => Shell.Current.Navigation.NavigationStack.Count.ToString();

    public ICommand RenewStackCountCommand { get; } 
    /// <summary>
    /// Конструктор модели представления главной страницы
    /// </summary>
    public MainViewModel(IDialogService dialogService,
        INavigationService navigationService)
    {
        // Сохранить диалоговый сервис
        this.dialogService = dialogService;

        // Сохранить навигационный сервис
        this.navigationService = navigationService;

        // Выдать отладочное сообщение
        Debug.WriteLine($"===== an instance of the class has been created: {nameof(MainViewModel)}");

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

        NavigateCommand = new Command(
            execute: async () =>
            {
                await navigationService.NavigateToAsync("Page1",
                    new Dictionary<string, object>
                    {
                        { "param1", Param }
                    });
            },
            canExecute: () => !string.IsNullOrEmpty(_param)
        );
        RenewStackCountCommand = new Command(() =>
        {
            OnPropertyChanged(nameof(NavStackCount));
        });
    }
}
