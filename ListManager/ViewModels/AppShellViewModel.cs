using CommunityToolkit.Mvvm.Input;
using ListManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListManager.ViewModels;

public partial class AppShellViewModel : ViewModelBase
{
    // Сервис данных
    private readonly IDataService dataService;
    // Сервис диалогов
    private readonly IDialogService dialogService;


    [RelayCommand]
    private async Task ExitAsync()
    {
        var answer = await dialogService.DisplayAlert("Exit the Applicatiom",
            "Do you sure you want to exit the application?", "Yes", "No");
        if (answer)
        {
            await dataService.SaveData();
            Application.Current?.Quit();
        }
    }

    public AppShellViewModel(IDataService dataService,
        IDialogService dialogService)
    {
        this.dataService = dataService;
        this.dialogService = dialogService;
    }
}
