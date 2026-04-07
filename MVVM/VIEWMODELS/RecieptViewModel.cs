using CommunityToolkit.Mvvm.ComponentModel;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.MVVM.SERVICES;
using System.Collections.ObjectModel;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class RecieptViewModel : ObservableObject
{
    public ObservableCollection<CartItem> Items { get; } = new();
    public decimal Total { get; set; }

    public RecieptViewModel()
    {
        // For demo we copy current cart (in real app you would pass transaction ID)
        foreach (var item in CartService.Cart)
            Items.Add(item);
        Total = CartService.GetTotal();
    }
}