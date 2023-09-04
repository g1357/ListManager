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
public class Page2ViewModel : ViewModelBase
{
    // Сервис навигации
    private readonly INavigationService navigationService;

    private string? _param1 = null;
    public string? Param1
    {
        get => _param1;
        set
        {
            SetProperty(ref _param1, value);
        }
    }
    public Command BackCommand { get; }

    public bool IsBackEnabled => navigationService.CanNavigateBack();

    /// <summary>
    /// Свойство возвращает грубину стэка навигации.
    /// </summary>
    public string NavStackCount => Shell.Current.Navigation.NavigationStack.Count.ToString();

    public ICommand RenewStackCountCommand { get; }
    public Page2ViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;

        // Выдать отладочное сообщение
        Debug.WriteLine($"===== an instance of the class has been created: {nameof(Page2ViewModel)}");

        BackCommand = new Command(
            execute: async () =>
            {
                await navigationService.NavigateBackAsync();
            },
            canExecute: () =>
            {
                bool res = navigationService.CanNavigateBack();
                return res;
            }
        );
        RenewStackCountCommand = new Command(() =>
        {
            OnPropertyChanged(nameof(NavStackCount));
            OnPropertyChanged(nameof(IsBackEnabled));
            ((Command)BackCommand).ChangeCanExecute();
        });

    }
}
