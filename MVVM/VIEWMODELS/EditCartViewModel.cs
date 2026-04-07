using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.MVVM.SERVICES;
using System.Collections.ObjectModel;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class EditCartViewModel : ObservableObject
{
    public ObservableCollection<CartItem> CartItems => CartService.Cart;

    [RelayCommand]
    private void UpdateQuantity(CartItem item, int change)
    {
        item.Quantity = Math.Max(1, item.Quantity + change);
        OnPropertyChanged(nameof(CartItems));
    }

    [RelayCommand]
    private void Remove(CartItem item) => CartService.RemoveFromCart(item);
}