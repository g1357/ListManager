using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using ListManager.Models;
using ListManager.Services;

namespace ListManager.ViewModels;

public partial class TaskListViewModel : ViewModelBase
{
    // Сервис данных
    private readonly IDataService dataService;
    // Сервис навигации между страницами
    private readonly INavigationService navigationService;
    // Сервис диалогов
    private readonly IDialogService dialogService;

    private ObservableCollection<TaskManager>? _treeViewData;
    public ObservableCollection<TaskManager>? TreeViewData
    {
        get => _treeViewData;
        set
        {
            if (SetProperty(ref _treeViewData, value))
            {

            }
        }
    }

    [RelayCommand]
    private async Task DoneChanged(TaskManager taskManager)
    {
        if (taskManager == null) return;

        await dialogService.DisplayAlert("The \"Completed\" marker has been changed",
            $"The task: {taskManager.Name} is completed!", "Ok");
        //dataService.UpdateTask(taskManager);

    }

    public TaskListViewModel(IDataService dataService,
        IDialogService dialogService, INavigationService navigationService)
    {
        this.dataService = dataService;
        this.dialogService = dialogService;
        this.navigationService = navigationService;

        GenerateSource();
    }

    private void GenerateSource()
    {
        var rootNode = new ObservableCollection<TaskManager>();

        var doc = new TaskManager { Name = "Documents", Description = "My documents" };
        var download = new TaskManager { Name = "Downloads", Description = "My downloads" };
        var music = new TaskManager { Name = "Music", Description = "My music" };
        var pictures = new TaskManager { Name = "Pictures", Description = "My pictures" };
        var video = new TaskManager { Name = "Videos", Description = "My video" };


        var pollution = new TaskManager() { Name = "Environmental Pollution.docx" };
        var globalWarming = new TaskManager() { Name = "Global Warming.ppt" };
        var sanitation = new TaskManager() { Name = "Sanitation.docx" };
        var socialNetwork = new TaskManager() { Name = "Social Network.pdf" };
        var youthEmpower = new TaskManager() { Name = "Youth Empowerment.pdf" };

        doc.SubTasks = new ObservableCollection<TaskManager>
            {
                pollution,
                globalWarming,
                sanitation,
                socialNetwork,
                youthEmpower
            };

        var tutorials = new TaskManager() { Name = "Tutorials.zip" };
        var typeScript = new TaskManager() { Name = "TypeScript.7z" };
        var uiGuide = new TaskManager() { Name = "UI-Guide.pdf" };

        download.SubTasks = new ObservableCollection<TaskManager>
            {
                tutorials,
                typeScript,
                uiGuide
            };

        var song = new TaskManager() { Name = "Gouttes" };

        music.SubTasks = new ObservableCollection<TaskManager>
            {
                song
            };

        var camera = new TaskManager() { Name = "Camera Roll" };
        var stone = new TaskManager() { Name = "Stone.jpg" };
        var wind = new TaskManager() { Name = "Wind.jpg" };


        pictures.SubTasks = new ObservableCollection<TaskManager>
            {
                camera,
                stone,
                wind
            };

        var img0 = new TaskManager() { Name = "WIN_20160726_094117.JPG" };
        var img1 = new TaskManager() { Name = "WIN_20160726_094118.JPG" };

        camera.SubTasks = new ObservableCollection<TaskManager>
            {
                img0,
                img1
            };

        var video1 = new TaskManager() { Name = "Naturals.mp4" };
        var video2 = new TaskManager() { Name = "Wild.mpeg" };

        video.SubTasks = new ObservableCollection<TaskManager>
            {
                video1,
                video2
            };

        rootNode.Add(doc);
        rootNode.Add(download);
        rootNode.Add(music);
        rootNode.Add(pictures);
        rootNode.Add(video);
        _treeViewData = rootNode;
    }
}
