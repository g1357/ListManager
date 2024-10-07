using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ListManager.Models;
using ListManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListManager.Popups
{
    public partial class EditListHeaderViewModel : ViewModelBase
    {
        readonly Popup popup;
        ShoppingList shoppingList;

        #region Поля формы редактирования продукта

        private string _listName = string.Empty;
        public string ListName
        {
            get => _listName;
            set
            {
                if (SetProperty(ref _listName, value))
                {
                    CheckChanges();
                }
            }
        }

        private string? _listDescription;
        public string? ListDescription
        {
            get => _listDescription;
            set
            {
                if (SetProperty(ref _listDescription, value))
                {
                    CheckChanges();
                }
            }
        }

        private bool _listFavourite;
        public bool ListFavourite
        {
            get => _listFavourite;
            set
            {
                if (SetProperty(ref _listFavourite, value))
                {
                    CheckChanges();
                }
            }
        }

        // Признак изменения данные о продукте в форме
        private bool dataChanged = false;

        void CheckChanges()
        {
            if (_listName == shoppingList?.Name &&
            _listDescription == shoppingList.Description &&
            _listFavourite == shoppingList.Favourite)
            {
                dataChanged = false;
                SaveCommand.NotifyCanExecuteChanged();
                CancelCommand.NotifyCanExecuteChanged();
            }
            else
            {
                dataChanged = true;
                SaveCommand.NotifyCanExecuteChanged();
                CancelCommand.NotifyCanExecuteChanged();
            }
        }

        #endregion Поля формы редактирования продукта

        #region Кнопки формы страницы

        [RelayCommand(CanExecute = nameof(IsSaveBtnEnabled))]
        private async Task SaveAsync()
        {
            var newList = new ShoppingList
            {
                ListKindId = shoppingList.ListKindId,
                Id = 0,
                Name = _listName,
                Description = _listDescription,
                Favourite = _listFavourite
            };

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await popup.CloseAsync(newList, cts.Token);
        }

        public bool IsSaveBtnEnabled => dataChanged;

        [RelayCommand(CanExecute = nameof(IsCancelBtnEnabled))]
        private async Task CancelAsync()
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await popup.CloseAsync(shoppingList, cts.Token);
        }
        public bool IsCancelBtnEnabled => true; // dataChanged;

        #endregion Кнопки формы страницы

        public EditListHeaderViewModel(ShoppingList list, Popup popup)
        {
            this.popup = popup;
            shoppingList = list;

            ListName = list.Name;
            ListDescription = list.Description;
            ListFavourite = list.Favourite;
        }
    }
}
