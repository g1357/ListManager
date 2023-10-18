using System.Diagnostics;

namespace ListManager;

// Класс приложения
// Частичное определения, вторая часть в соответсвующем XAML-файле
public partial class App : Application
{
    // Конструктор класса приложения
    public App()
    {
        // Инициализировать соответствующий XAML-файл
        InitializeComponent();

        // Задать главную станицу приложения
        MainPage = new AppShell();
    }

    // Выполнить действия при запуске приложения
    protected override async void OnStart()
    {
        // Handle when your app starts
        Debug.WriteLine("==== OnStart method is called!");
    }

    // Выполнить действия при деактивации приложения
    protected override async void OnSleep()
    {
        // Handle when your app sleeps
        Debug.WriteLine("==== OnSleep method is called!");
    }

    // Выполнить действия при активации приложения
    protected override async void OnResume()
    {
        // Handle when your app resumes
        Debug.WriteLine("==== OnRresum method is called!");
    }
}
