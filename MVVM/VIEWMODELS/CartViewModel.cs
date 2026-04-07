using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.MVVM.SERVICES;
using System.Collections.ObjectModel;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class CartViewModel : ObservableObject
{
    public ReadOnlyObservableCollection<CartItem> Items => CartService.Items;

    public int TotalItems => CartService.TotalItems;
    public decimal TotalPrice => CartService.TotalPrice;

    [RelayCommand]
    private void ClearCart()
    {
        CartService.Clear();
        OnPropertyChanged(nameof(TotalItems));
        OnPropertyChanged(nameof(TotalPrice));
    }

    [RelayCommand]
    private async Task Proceed()
    {
        // Navigate to Payment page
        if (Application.Current?.MainPage is FlyoutPage flyout && flyout.Detail is NavigationPage nav)
            await nav.PushAsync(new MVVM.VIEWS.Payment());
    }
}
