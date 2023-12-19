using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListManager.Services;

/// <summary>
/// Помошник явного разрешения зависимостей.
/// </summary>
/// <remarks>
/// Для использования этого класса для разрешения зависимостей необходимо
/// в файл MauiProgram.cs добавить команду:
/// <code>
/// var app = builder.Build();
/// ServiceHelper.Initialize(app.Services);
/// return app;
/// </code>
/// вместо
/// <code>
/// return builder.Build();
/// </code>
/// </remarks>
public static class ServiceHelper
{
    public static IServiceProvider Services { get; private set; }
    public static void Initialize(IServiceProvider serviceProvider)
    {
        Services = serviceProvider;
    }

    public static T GetService<T>() => Services.GetService<T>();
    // Application.Current.MainPage.Handler.MauiContext.Services.GetService<T>();

}
