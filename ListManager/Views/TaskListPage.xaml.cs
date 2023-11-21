using ListManager.ViewModels;

namespace ListManager.Views;

public partial class TaskListPage : ContentPage
{
	TaskListViewModel viewModel;
	public TaskListPage(TaskListViewModel viewModel)
	{
        BindingContext = this.viewModel = viewModel;
        
		InitializeComponent();

	}
}