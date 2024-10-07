using CommunityToolkit.Maui.Views;
using ListManager.Models;

namespace ListManager.Popups;

public partial class EditListHeaderPopup : Popup
{
    readonly EditListHeaderViewModel viewModel;
    ShoppingList list;
	private EditListHeaderPopup()
	{

		InitializeComponent();
    }

    public EditListHeaderPopup(ShoppingList list) : this()
    {
        this.list = list;
        BindingContext = viewModel = new EditListHeaderViewModel(list, this);
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