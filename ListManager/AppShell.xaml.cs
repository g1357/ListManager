using ListManager.Views;
using System.Diagnostics;

namespace ListManager
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Задать начальную страницу приложения
            // Получить начальную страницу или 0, если она не сохранены
            var id = Preferences.Get("StartPage", 0);
            // Получить количество страниц в оболочке
            var qty = App_Shell.Items.Count;
            if (id < qty && id >=0) // Если индекс страницы в диапазоне
            {
                // Установить начальную страницу по индексу
                App_Shell.CurrentItem = App_Shell.Items[id];
            }
        }
    }
}