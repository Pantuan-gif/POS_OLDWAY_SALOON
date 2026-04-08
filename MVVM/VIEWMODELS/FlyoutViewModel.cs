using POS_OLDWAY_SALOON.MVVM.MODELS;
using System;
using POS_OLDWAY_SALOON.MVVM.VIEWS;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS
{
    class FlyoutViewModel
    {
        public ObservableCollection<FlyoutMenuItem> MenuItems { get; set; }

        public ICommand MenuItemSelected { get; }

        public FlyoutViewModel()
        {
            MenuItems = new ObservableCollection<FlyoutMenuItem>()
        {
            new FlyoutMenuItem
            {
                Title = "Home",
                Icon = "placeholder.png",
                TargetPage = typeof(Home)
            },
        };

            MenuItemSelected = new Command<FlyoutMenuItem>(OnMenuSelected);
        }

        private void OnMenuSelected(FlyoutMenuItem item)
        {
            if (item == null)
                return;

            var page = (Page)Activator.CreateInstance(item.TargetPage);

            Application.Current.MainPage = new FlyoutPage
            {
                Detail = new NavigationPage(page)
            };
        }
    }
}
