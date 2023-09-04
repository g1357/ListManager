using ListManager.Services;
using ListManager.ViewModels;
using ListManager.Views;
using Microsoft.Extensions.Logging;

namespace ListManager
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
		    builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<IDialogService, DialogService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddPage<MainPage,MainViewModel>();
            builder.Services.AddPage<Page1View, Page1ViewModel>("Page1");
            builder.Services.AddPage<Page2View, Page2ViewModel>("Page2");

            return builder.Build();
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

    }
}