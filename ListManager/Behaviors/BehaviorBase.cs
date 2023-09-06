// Пространство имён поведений
namespace ListManager.Behaviors;

/// <summary>
/// Базовы класс для создания поведений.
/// Базируется на классе Behavior<typeparamref name="T"/>
/// </summary>
/// <typeparam name="T">Тип к которому подсоединяется поведение. 
/// Должен быть наследником класса BindableObject, т.е. позволять 
/// привязывать к нему привязанные свойства (BindableProperty)</typeparam>
public class BehaviorBase<T> : Behavior<T> where T : BindableObject
{
    /// <summary>
    /// Связанный объъект, к которому привязывается поведение.
    /// </summary>
    public T AssociatedObject { get; private set; }

    /// <summary>
    /// Действия при привязке.
    /// </summary>
    /// <param name="bindable">Объект к которому осуществляется привязка</param>
    protected override void OnAttachedTo(T bindable)
    {
        // Выполнить действия безового класса
        base.OnAttachedTo(bindable);

        // Сохранить объект к которому осуществяется привязка
        AssociatedObject = bindable;
        // Если задан контекст привязки, то
        if (bindable.BindingContext != null)
        {
            // Задать контекст привязки текущего объекта
            BindingContext = bindable.BindingContext;
        }
        // подключить обработчик события изменения контекста привязки
        bindable.BindingContextChanged += OnBindingContextChanged;
    }

    /// <summary>
    /// Действия при отвязке.
    /// </summary>
    /// <param name="bindable">Объект от которого осуществляется отвязка</param>
    protected override void OnDetachingFrom(T bindable)
    {
        // Выполнить действия безового класса
        base.OnDetachingFrom(bindable);

        // Отключить обработчик события изменения контекста привязки.
        bindable.BindingContextChanged -= OnBindingContextChanged;
        // Очистить свойсто, хранящие объект к которому была привязка. Для сборки мусора.
        AssociatedObject = null;
    }

    /// <summary>
    /// Обработка при смене контекста приязки.
    /// </summary>
    /// <param name="sender">источник возникновения события</param>
    /// <param name="e">Аргументы события</param>
    private void OnBindingContextChanged(object? sender, EventArgs e)
    {
        // Вызвать обработчие без параметров
        OnBindingContextChanged();
    }

    /// <summary>
    /// Обработка при смене контекста приязки
    /// </summary>
    protected override void OnBindingContextChanged()
    {
        // Выполнить действия безового класса
        base.OnBindingContextChanged();
        // Переустановить текущий контекст приязки объекта из объекта
        // к которому осуществлена привязка
        BindingContext = AssociatedObject.BindingContext;
    }
}
