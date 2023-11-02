using CommunityToolkit.Mvvm.Input;
using ListManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListManager.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    // Сервис диалогов
    private readonly IDialogService dialogService;
    // Сервис данных
    private readonly IDataService dataService;
    // Сервис навигации между страницами
    private readonly INavigationService navigationService;

    [RelayCommand]
    private async Task ClearDataAsync()
    {
        var answer = await dialogService.DisplayAlert(
            "Data Deletion!",
            "Are you sure you want to delete all the data?",
            "Yes", "No");
        if (answer)
        {
            dataService.ClearAllData();
            await dialogService.DisplayAlert(
                "Data Deletion!",
                "You have deleted all the data!",
                "Close");
        }
    }

    [RelayCommand]
    private async Task DemoDataAsync()
    {
        var answer = await dialogService.DisplayAlert(
            "Adding Demo Data!",
            "ADo you want to add demo data to the storage?",
            "Yes", "No");
        if (answer)
        {
            dataService.DataSeed();
            await dialogService.DisplayAlert(
                "Demo Data!",
                "Demo data added",
                "Close");
        }
    }

    public SettingsViewModel(IDialogService dialogService,
        IDataService dataService, INavigationService navigationService)
    {
        this.dialogService = dialogService;
        this.dataService = dataService;
        this.navigationService = navigationService;
    }
}
