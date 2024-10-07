using CommunityToolkit.Maui.Views;
using ListManager.Models;

namespace ListManager.Popups;

public partial class EditListHeaderPopup : Popup
{
    EditListHeaderViewModel viewModel;
    ShoppingListDaD list;
	public EditListHeaderPopup()
	{
		InitializeComponent();
	}

    public EditListHeaderPopup(ShoppingListDaD list) : this()
    {
        this.list = list;
        BindingContext = viewModel = new EditListHeaderViewModel(list);
    }

    async void OnSaveButtonClicked(object? sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        await CloseAsync(list, cts.Token);
    }

    async void OnCancelButtonClicked(object? sender, EventArgs e)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        await CloseAsync(list, cts.Token);
    }
}