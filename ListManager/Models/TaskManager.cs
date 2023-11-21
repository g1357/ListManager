using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListManager.Models;

public class TaskManager : ObservableObject
{
    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private string _description = string.Empty;
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    private bool _done;
    public bool Done
    {
        get => _done;
        set => SetProperty(ref _done, value);
    }

    private ObservableCollection<TaskManager>? _subTasks;
    public ObservableCollection<TaskManager>? SubTasks
    {
        get => _subTasks;
        set => SetProperty(ref _subTasks, value);
    }
}
