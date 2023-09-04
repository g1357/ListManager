using ListManager.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ListManager.ViewModels;

[QueryProperty(nameof(Param1), "param1")]
public class Page1ViewModel : ViewModelBase
{
    // Сервис навигации
    private readonly INavigationService navigationService;

    private string _param1 = null;
    public string Param1
    {
        get => _param1;
        set
        {
            SetProperty(ref _param1, value);
        }
    }

    private string _param = string.Empty;
    public string Param
    {
        get => _param;
        set
        {
            if (SetProperty(ref _param, value))
            {
                ((Command)NavigateCommand).ChangeCanExecute();
            }
        }
    }

    public ICommand NavigateCommand { get; }

    /// <summary>
    /// Свойство возвращает грубину стэка навигации.
    /// </summary>
    public string NavStackCount => Shell.Current.Navigation.NavigationStack.Count.ToString();
    public ICommand RenewStackCountCommand { get; }

    public Page1ViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;

        // Выдать отладочное сообщение
        Debug.WriteLine($"===== an instance of the class has been created: {nameof(Page1ViewModel)}");

        NavigateCommand = new Command(
            execute: async () =>
            {
                await navigationService.NavigateToAsync("Page2",
                    new Dictionary<string, object>
                    {
                        { "param1", Param }
                    });
            },
            canExecute: () => !string.IsNullOrEmpty(_param)
        );
        RenewStackCountCommand = new Command(() =>
        {
            OnPropertyChanged(nameof(NavStackCount));
        });
    }
}
