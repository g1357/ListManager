using ListManager.Services;

// пространство имён моделей представления
namespace ListManager.ViewModels;

/// <summary>
/// Модель представления страницы2.
/// Базируется на базовом классе моделей представления.
/// Реализует интерфейс IQueryAttributable для получения аргументов.
/// </summary>
public class Page2ViewModel : ViewModelBase, IQueryAttributable
{
    // Сервис навигации
    private readonly INavigationService navigationService;

    // Поле для свойства Param1
    private string? _param1 = null;
    /// <summary>
    /// Свойство, получаеще значение параметра param1
    /// </summary>
    public string? Param1
    {
        get => _param1;
        set
        {
            SetProperty(ref _param1, value);
        }
    }
    
    /// <summary>
    ///  Команда возврата к предыдущей стрвнице
    /// </summary>
    public Command BackCommand { get; }

    /// <summary>
    /// Конструктор класса.
    /// </summary>
    /// <param name="navigationService">Сервис навигации</param>
    public Page2ViewModel(INavigationService navigationService)
    {
        // Сохранить сервис навигации
        this.navigationService = navigationService;

        // Определить команду возврата к предыдущей странице
        BackCommand = new Command(async () =>
            {
                await navigationService.NavigateBackAsync();
            }
        );
    }

    /// <summary>
    /// Получение переданных аргументов
    /// </summary>
    /// <param name="query">Словарь аргументов</param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        // Пометить в свойство Param1 аргумент параметра с именем param1
        // преобразованный в строку.
        Param1 = query["param1"] as string;
    }
}
