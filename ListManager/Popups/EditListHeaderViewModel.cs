using CommunityToolkit.Mvvm.ComponentModel;
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
        #region Поля формы редактирования продукта
        private string _listName = string.Empty;
        public string ListName
        {
            get => _listName;
            set
            {
                if (SetProperty(ref _listName, value))
                {
                    //CheckChanges();
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
                    //CheckChanges();
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
                    //CheckChanges();
                }
            }
        }

        /*
        void CheckChanges()
        {
            if (_listName == _shoppingListInfo?.Name &&
            _listDescription == _shoppingListInfo.Description &&
            _listFavourite == _shoppingListInfo.Favourite)
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
        */
        #endregion Поля формы редактирования продукта


        public EditListHeaderViewModel(ShoppingListDaD list)
        {
            ListName = list.Name;
            ListDescription = list.Description;
            ListFavourite = list.Favourite;
        }
    }
}
