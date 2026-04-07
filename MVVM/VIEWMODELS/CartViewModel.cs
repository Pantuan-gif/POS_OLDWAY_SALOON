using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.MVVM.SERVICES;
using POS_OLDWAY_SALOON.Services;
using System.Collections.ObjectModel;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class CartViewModel : ObservableObject
{
    private readonly APISERVICES _api = new APISERVICES();

    public ObservableCollection<CartItem> CartItems => CartService.Cart;

    public decimal Total => CartService.GetTotal();

    [RelayCommand]
    private void Remove(CartItem item)
    {
        CartService.RemoveFromCart(item);
        OnPropertyChanged(nameof(Total));
    }

    [RelayCommand]
    private async Task Checkout()
    {
        if (CartItems.Count == 0) return;

        foreach (var item in CartItems)
        {
            var transaction = new Transaction
            {
                ProductName = item.Product.ProductName,
                Quantity = item.Quantity,
                Amount = item.Total,
                DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                Category = "Sale"   // you can improve this later
            };

            await _api.AddTransactionAsync(transaction);

            // Optional: reduce stock in products table
        }

        await Application.Current!.MainPage!.DisplayAlert("Success", "Payment completed successfully!", "OK");

        CartService.Clear();
        
        // Navigate back to main POS screen
        if (Application.Current?.MainPage is FlyoutPage flyout && flyout.Detail is NavigationPage nav)
            await nav.PopToRootAsync();
    }
}