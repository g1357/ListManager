using System.Reflection;
using System.Windows.Input;

// Пространсто имён поведений 
namespace ListManager.Behaviors;

/// <summary>
/// Поведение привязки команды к событию.
/// Базируется на классе базового поведения
/// для элементов управления - наследников класс View.
/// </summary>
public class EventToCommandBehavior : BehaviorBase<View>
{
    // Обработчик события
    Delegate? eventHandler;

    // Создать "привязанное" свойство EventName, которое будет содержать имя события
    // к которым связывается команда
    public static readonly BindableProperty EventNameProperty =
        BindableProperty.Create("EventName", typeof(string),
            typeof(EventToCommandBehavior), null, propertyChanged: OnEventNameChanged);

    // Создать "привязанное" свойство Command, которое будет содержать команду,
    // которая будет выполнена при возникновении связанного события
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create("Command", typeof(ICommand),
            typeof(EventToCommandBehavior), null);

    // Создать "привязанное" свойство CommandParameter, которое будет содержать
    // параметр, который будет передан команде при выполнении
    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create("CommandParameter", typeof(object),
            typeof(EventToCommandBehavior), null);

    // Создать "привязанное" свойство Converter, которое будет содержать имя конвертера
    public static readonly BindableProperty InputConverterProperty =
        BindableProperty.Create("Converter", typeof(IValueConverter),
            typeof(EventToCommandBehavior), null);

    // Свойство имя события
    public string EventName
    {
        // Получить значение "привязанного" свойста
        get => (string)GetValue(EventNameProperty);
        // Установить значение "привязанного" свойста
        set => SetValue(EventNameProperty, value);
    }

    // Свойство команда
    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    // Свойство параметр команды
    public object CommandParamrter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    // Свойство конвертер
    public IValueConverter Converter
    {
        get => (IValueConverter)GetValue(InputConverterProperty);
        set => SetValue(InputConverterProperty, value);
    }

    /// <summary>
    /// Обработчик события при присоединении к объекту
    /// </summary>
    /// <param name="bindable">Объект к которому осуществояется присоединение</param>
    protected override void OnAttachedTo(View bindable)
    {
        // Выполнить действия безового класса
        base.OnAttachedTo(bindable);
        // Зарегистрировать событие
        RegisterEvent(EventName);
    }

    /// <summary>
    /// Обработчик события отсоединения от объекта
    /// </summary>
    /// <param name="bindable">Объект от которого осуществляется отсоединение</param>
    protected override void OnDetachingFrom(View bindable)
    {
        // Выполнить действия безового класса
        base.OnDetachingFrom(bindable);
        // Отменить регистрацию события
        DeregesterEvent(EventName);
    }

    /// <summary>
    /// Зарегистрировать событие
    /// </summary>
    /// <param name="eventName">Имя события</param>
    /// <exception cref="ArgumentException">Ошибка в аргументе</exception>
    private void RegisterEvent(string? eventName)
    {
        // Если имя события не задано,
        if (string.IsNullOrEmpty(eventName))
        {
            // то выйти
            return;
        }
        // Получить информацию о событии
        EventInfo? eventInfo = AssociatedObject?.GetType().GetRuntimeEvent(eventName);
        // Если информация о собитии не доступна
        if (eventInfo == null)
        {
            // Вызвать исключение: ошибка в аргументе
            throw new ArgumentException($"EventToCommandBehavior: Can't register the '{eventName}' event.");
        }
        // Получить информацию о методе обработки события
        MethodInfo? methodInfo = typeof(EventToCommandBehavior).GetTypeInfo().GetDeclaredMethod("OnEvent");
        // Создать обработсчик события, указывающий на данный объект
        eventHandler = methodInfo?.CreateDelegate(eventInfo.EventHandlerType, this);
        // Добавить обработчик события к объекту  которому мы привязываем поведение
        eventInfo.AddEventHandler(AssociatedObject, eventHandler);
    }

    /// <summary>
    /// Удалить регистрацию события.
    /// </summary>
    /// <param name="eventName">Имя события</param>
    /// <exception cref="ArgumentException">Ошибка в аргументе</exception>
    private void DeregesterEvent(string eventName)
    {
        // Если имя события не задано,
        if (!string.IsNullOrEmpty(eventName))
        {
            // то выйти
            return;
        }
        // Если обработчик события отсутствует,
        if (eventHandler == null)
        {
            // то выйти
            return;
        }
        // Получить информацию о событии
        EventInfo? eventInfo = AssociatedObject?.GetType().GetRuntimeEvent(eventName);
        // Если информация о событии отсутсивует,
        if (eventInfo == null)
        {
            // Вызвать исключение: невозможность отменить регистрацию события
            throw new ArgumentException($"EventToCommandBehavior: Can;t de-register the '{eventName}' event.");
        }
        // Удалить обработчик события
        eventInfo.RemoveEventHandler(AssociatedObject, eventHandler);
        // Очистить обработчик события.
        eventHandler = null;
    }

    /// <summary>
    /// Обработчик заданного события
    /// </summary>
    /// <param name="sender">Истояник возникновения события</param>
    /// <param name="eventArgs">Аргументы события</param>
    private void OnEvent(object? sender, object eventArgs)
    {
        // Если команда не задана,
        if (Command == null)
        {
            // то выйти
            return;
        }
        // разрешённый параметр
        object resolvedParameter;
        // Если параметр команды задан,
        if (CommandParamrter != null)
        {
            // то разрешённым параметром будет параметр команды
            resolvedParameter = CommandParamrter;
        }
        // иначе, если задан конвертер,
        else if (Converter != null)
        {
            // то разрешённым параметром будет преобразованный аргумент
            resolvedParameter = Converter.Convert(eventArgs, typeof(object), null, null);
        }
        // иначе
        else
        {
            // в качестве разрешённого параметра возьмём аргумент
            resolvedParameter = eventArgs;
        }
        // Если коменду можно выполнить для разрешённого параметра,
        if (Command.CanExecute(resolvedParameter))
        {
            // то выполняет команду с разрешённым параметром
            Command.Execute(resolvedParameter);
        }
    }

    /// <summary>
    /// Дейстия при смене имени событие
    /// </summary>
    /// <param name="bindable">Привязанный объект</param>
    /// <param name="oldValue">Старое событие</param>
    /// <param name="newValue">Новое событие</param>
    private static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
    {
        // преобразовать привязанный объект к типу нашего поведения 
        var behavior = (EventToCommandBehavior)bindable;
        // Если не задан объект к которому привязано поведение,
        if (behavior.AssociatedObject == null)
        {
            // то выйти
            return;
        }
        // Преобразовать имя старого собития в строку
        string oldEventName = (string)oldValue;
        // Преобразовать имя нового собития в строку
        string newEventName = (string)newValue;
        // Отменить регистрацию старого события
        behavior.DeregesterEvent(oldEventName);
        // Зарегистрировать новое событие
        behavior.RegisterEvent(newEventName);
    }
}
