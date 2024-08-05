using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

// Пространство имён моделей представления
namespace ListManager.ViewModels;

/// <summary>
/// Модель представления информации о версии приложения
/// </summary>
public partial class VersionViewModel :ViewModelBase
{
    // Признак первого запуска приложения
    [ObservableProperty]
    private bool _isFirst =
        VersionTracking.Default.IsFirstLaunchEver;

    // Признак первого запуска этой версии приложения
    [ObservableProperty]
    private bool _currentVersionIsFirst = 
        VersionTracking.Default.IsFirstLaunchForCurrentVersion;

    // Признак первого запуска этой сборки приложения
    [ObservableProperty]
    private bool _currentBuildIsFirst = 
        VersionTracking.Default.IsFirstLaunchForCurrentBuild;

    // Текущая версия приложения
    [ObservableProperty]
    private string _currentVersion = 
        VersionTracking.Default.CurrentVersion;

    // Текущая сборка приложения
    [ObservableProperty]
    private string _currentBuild = 
        VersionTracking.Default.CurrentBuild;

    // Первая установленная версия данного приложения на данном устройстве
    [ObservableProperty]
    private string? _firstInstalledVer = 
        VersionTracking.Default.FirstInstalledVersion;

    // Первая установленная сборка данногог приложения на данном устройстве
    [ObservableProperty]
    private string? _firstInstalledBuild = 
        VersionTracking.Default.FirstInstalledBuild;

    // История установленных версий (список через запятую)
    [ObservableProperty]
    private string _versionHistory = 
        String.Join(',', VersionTracking.Default.VersionHistory);

    // История установленных сборок (список через запятую)
    [ObservableProperty]
    private string _buildHistory = 
        String.Join(',', VersionTracking.Default.BuildHistory);

    // These two properties may be null if this is the first version
    // Предыдущая установленая версия приложения (может быть пустой)
    [ObservableProperty]
    private string? _previousVersion = 
        VersionTracking.Default.PreviousVersion ?? "none";

    // Предыдущая установленная сборка приложения (может быть пустой)
    [ObservableProperty]
    private string? _previousBuild = 
        VersionTracking.Default.PreviousBuild ?? "none";

    [RelayCommand]
    private async Task ShowSettings()
    {
        AppInfo.Current.ShowSettingsUI();
    }
}

