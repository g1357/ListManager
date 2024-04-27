using CommunityToolkit.Maui;
using ListManager.Services;
using ListManager.ViewModels;
using ListManager.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Syncfusion.Maui.Core.Hosting;
using System.Diagnostics;

namespace ListManager
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                // Initialize the .NET MAUI Community Toolkit
                .UseMauiCommunityToolkit()

                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("materialdesignicons-webfont.ttf", "MaterialDesignIcons");
                })
                .ConfigureEssentials(essentials =>
                {
                    essentials.UseVersionTracking(); // Подключение отслеживания версий
                });
            builder.ConfigureSyncfusionCore();
            builder.ConfigureLifecycleEvents(AppLifecycle =>
            {
#if ANDROID
                AppLifecycle.AddAndroid(android => android
                    .OnDestroy((activity) => Android_OnDestroy())
                    .OnStop((activity) => Android_OnStop())
                    .OnSaveInstanceState((activity, del) => Android_OnSaveInstanceState())
                    .OnPause((activity) => Android_OnPause())
                    );
#endif
            });

#if DEBUG
		    builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<IDialogService, DialogService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<IDataService, DataService>();

            builder.Services.AddPage<MainPage,MainViewModel>();
            builder.Services.AddPage<ShoppingListsPage, ShoppingListsViewModel>("ShoppingLists");
            builder.Services.AddPage<ShoppingListPage, ShoppingListViewModel>("ShoppingList");
            builder.Services.AddPage<ShoppingListDetailsPage, ShoppingListDetailsViewModel>("ShoppingListDetails");
            builder.Services.AddPage<ProductDetailsPage, ProductDetailsViewModel>("ProductDetails");
            builder.Services.AddPage<TaskListPage, TaskListViewModel>("TaskList");
            builder.Services.AddPage<SettingsPage, SettingsViewModel>("Settings");
            builder.Services.AddPage<HelpPage, HelpViewModel>("Help");
            builder.Services.AddPage<VersionPage, VersionViewModel>("Version");
            builder.Services.AddPage<Page1View, Page1ViewModel>("Page1");
            builder.Services.AddPage<Page2View, Page2ViewModel>("Page2");
            builder.Services.AddPage<ShListPage, ShListViewModel>("ShList");
            builder.Services.AddPage<AppShell, AppShellViewModel>("AppShell");

            var app = builder.Build();
            ServiceHelper.Initialize(app.Services);
            return app;
        }

        /// <summary>
        /// Добавить страницу (представление) в схему навигации.
        /// </summary>
        /// <typeparam name="TPage">Тип страницы (представления)</typeparam>
        /// <typeparam name="TViewModel">Тип модели представления</typeparam>
        /// <param name="services">Коллекция сервисов приложения</param>
        /// <param name="route">Путь в системе навигации</param>
        /// <returns>Коллекция сервисов</returns>
        private static IServiceCollection AddPage<TPage, TViewModel>(
            this IServiceCollection services,
            string? route = null) where TPage : Page where TViewModel : ViewModelBase
        {
            services
                .AddTransient(typeof(TPage))
                // Некоторые модели представления создаются как AddSingleton
                .AddTransient(typeof(TViewModel));
            if (route != null)
            {
                Routing.RegisterRoute(route, typeof(TPage));
            }
            return services;
        }

        static void Android_OnDestroy()
        {
            var m = System.Reflection.MethodBase.GetCurrentMethod();
            Debug.WriteLine($"===== Method : {m?.Name} of Class: {m?.DeclaringType?.Name}");
        }
        static void Android_OnStop()
        {
            var m = System.Reflection.MethodBase.GetCurrentMethod();
            Debug.WriteLine($"===== Method : {m?.Name} of Class: {m?.DeclaringType?.Name}");
        }
        static void Android_OnSaveInstanceState()
        {
            var m = System.Reflection.MethodBase.GetCurrentMethod();
            Debug.WriteLine($"===== Method : {m?.Name} of Class: {m?.DeclaringType?.Name}");
        }
        static async void Android_OnPause()
        {
            var dataService = ServiceHelper.GetService<IDataService>();
            await dataService.SaveData();
            var m = System.Reflection.MethodBase.GetCurrentMethod();
            Debug.WriteLine($"===== Method : {m?.Name} of Class: {m?.DeclaringType?.Name}");
        }
    }
}