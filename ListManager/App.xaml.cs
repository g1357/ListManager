using ListManager.Services;
using System.Diagnostics;

namespace ListManager;

// Класс приложения
// Частичное определения, вторая часть в соответсвующем XAML-файле
public partial class App : Application
{
    // Сервис данных
    private readonly IDataService dataService;

    // Конструктор класса приложения
    public App(IDataService dataService)
    {
        this.dataService = dataService;

        // Инициализировать соответствующий XAML-файл
        InitializeComponent();

        // Задать главную станицу приложения
        MainPage = new AppShell();
    }

    // Выполнить действия при запуске приложения
    protected async override void OnStart()
    {
        // Handle when your app starts
        Debug.WriteLine("==== OnStart method is called!");

        base.OnStart();
        await dataService.RestoreData();
    }

    // Выполнить действия при деактивации приложения
    protected async override void OnSleep()
    {
        // Handle when your app sleeps
        Debug.WriteLine("==== OnSleep method is called!");

        base.OnSleep();
        await dataService.SaveData();
    }

    // Выполнить действия при активации приложения
    protected override void OnResume()
    {
        // Handle when your app resumes
        Debug.WriteLine("==== OnRresum method is called!");
        base.OnResume();
    }

    // Изменяем празмеры окна в Windows
    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);

        window.Width = 600;
        window.Height = 900;

        return window;
    }
}
