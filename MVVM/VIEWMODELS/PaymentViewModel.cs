using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.SERVICES;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class PaymentViewModel : ObservableObject
{
    public decimal TotalAmount => CartService.TotalPrice;

    [ObservableProperty]
    private decimal _tenderedAmount;

    [ObservableProperty]
    private decimal _changeAmount;

    partial void OnTenderedAmountChanged(decimal value)
    {
        // compute change live when tendered amount changes
        var change = Math.Max(0, value - TotalAmount);
        ChangeAmount = change;
    }

    partial void OnPaymentMethodChanged(string value)
    {
        // if non-cash payment, clear tendered/change
        if (!string.Equals(value, "Cash", StringComparison.OrdinalIgnoreCase))
        {
            TenderedAmount = 0m;
            ChangeAmount = 0m;
        }
    }
    [ObservableProperty]
    private string _paymentMethod = "Cash";

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

        // If paying by cash, compute change
        order.TenderedAmount = TenderedAmount;
        order.Change = Math.Max(0, TenderedAmount - order.Total);
        // Set operator (current user)
        order.OperatorName = Services.AuthService.CurrentUser?.FirstName + " " + Services.AuthService.CurrentUser?.LastName;

        // update local properties so UI can show change
        ChangeAmount = order.Change;

        // Persist order to local JSON store so TransactionReports can read it
        try
        {
            await MVVM.SERVICES.TransactionService.AppendAsync(order);
        }
        catch
        {
            // ignore persistence failure for now
        }

        CartService.Clear();

        if (Application.Current?.MainPage is FlyoutPage flyout && flyout.Detail is NavigationPage nav)
        {
            await nav.PushAsync(new MVVM.VIEWS.Reciept { BindingContext = order });
        }
    }
}
