using CommunityToolkit.Mvvm.Input;
using ListManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListManager.ViewModels;

public partial class HelpViewModel : ViewModelBase
{
    #region Сервисы
    // Сервис данных
    private readonly IDataService dataService;
    // Сервис навигации между страницами
    private readonly INavigationService navigationService;
    // Сервис диалогов
    private readonly IDialogService dialogService;
    #endregion Сервисы

    [RelayCommand]
    private async Task CloseAsync()
    {
        await navigationService.PopModalAsync();
    }
    public HelpViewModel(IDataService dataService,
    INavigationService navigationService, IDialogService dialogService)
    {
        this.dataService = dataService;
        this.navigationService = navigationService;
        this.dialogService = dialogService;
    }

}
