using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ListManager.Models;
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

    [ObservableProperty]
    private List<ListKind> _startPageList;

    private ListKind? _selectedItem;
    public ListKind? SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (SetProperty(ref _selectedItem, value))
            {
                Preferences.Set("StartPage", _selectedItem.Id);
            }
        }
    }

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

        var startPage = Preferences.Get("StartPage", 0);
        var listKinds = dataService.GetListTypes();
        StartPageList = new List<ListKind>();
        StartPageList.Add(new ListKind { Id = 0, Name = "Выбор типа списка", Description = "" });
        foreach (var listKind in listKinds)
        {
            StartPageList.Add(listKind);
        }
        SelectedItem = StartPageList.FirstOrDefault(x => x.Id == startPage);
    }
}
