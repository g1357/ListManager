using ListManager.Services;
using System.Windows.Input;

// Пространство имён моделей представления
namespace ListManager.ViewModels;

/// <summary>
/// Модель представления страницы 1.
/// Базируется на базовом классе моделей представления.
/// </summary>
// Связка имени передаваемого параметра со свойствос
[QueryProperty(nameof(Param1), "param1")]
public class Page1ViewModel : ViewModelBase
{
    // Сервис навигации
    private readonly INavigationService navigationService;

    // Поле для свойства Param1
    private string? _param1 = null;
    /// <summary>
    /// Свойство, получающее значение параметра param1
    /// </summary>
    public string? Param1
    {
        get => _param1;
        set
        {
            SetProperty(ref _param1, value);
        }
    }

    // Поле для свойства Param
    private string _param = string.Empty;
    /// <summary>
    /// Свойство передающееся в качестве аргумента при переходе к странице 2
    /// </summary>
    public string Param
    {
        get => _param;
        set
        {
            // Если значение свойство изменилось,
            if (SetProperty(ref _param, value))
            {
                // то перевычислить возможность выполнения команды NavigateCommand
                ((Command)NavigateCommand).ChangeCanExecute();
            }
        }
    }

    /// <summary>
    /// Свойство для команды
    /// </summary>
    public ICommand NavigateCommand { get; }

    /// <summary>
    /// Конструктор класса
    /// </summary>
    /// <param name="navigationService">сервис навигации</param>
    public Page1ViewModel(INavigationService navigationService)
    {
        // Сохранить сервис навигации
        this.navigationService = navigationService;

        // Определить косманду навигации
        NavigateCommand = new Command(
            // Действия команды
            execute: async () =>
            {
                // Выполнить переход к странце Page2 с передачей параметра
                await navigationService.NavigateToAsync("Page2",
                    new Dictionary<string, object>
                    {
                        { "param1", Param }
                    });
            },
            // Проверка возможности выполнения команды
            // Если поле параметра не пустое, то команду можно выполнить
            canExecute: () => !string.IsNullOrEmpty(_param)
        );
    }
}
