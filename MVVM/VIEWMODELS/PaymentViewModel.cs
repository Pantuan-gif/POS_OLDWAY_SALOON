using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.SERVICES;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class PaymentViewModel : ObservableObject
{
    public decimal TotalAmount => CartService.TotalPrice;

    [RelayCommand]
    private async Task Back()
    {
        if (Application.Current?.MainPage is FlyoutPage flyout && flyout.Detail is NavigationPage nav)
            await nav.PopAsync();
    }

    [RelayCommand]
    private async Task Pay()
    {
        // Build an order from cart, clear cart, and navigate to receipt
        var order = new MVVM.MODELS.Order
        {
            PaymentMethod = "Cash",
            Items = CartService.Items.Select(i => new MVVM.MODELS.CartItem
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                PhotoPath = i.PhotoPath
            }).ToList()
        };

        CartService.Clear();

        if (Application.Current?.MainPage is FlyoutPage flyout && flyout.Detail is NavigationPage nav)
        {
            await nav.PushAsync(new MVVM.VIEWS.Reciept { BindingContext = order });
        }
    }
}
