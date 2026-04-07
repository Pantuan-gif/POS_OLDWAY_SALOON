using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.SERVICES;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class PaymentViewModel : ObservableObject
{
    public decimal Total => CartService.GetTotal();

    [ObservableProperty] private string paymentMethod = "Cash";

    [RelayCommand]
    private async Task CompletePayment()
    {
        if (CartService.Cart.Count == 0) return;

        await Application.Current!.MainPage!.DisplayAlert("Payment Successful", 
            $"Total Amount: ₱{Total:F2}\nThank you!", "OK");

        CartService.Clear();

        // Navigate back to main screen
        if (Application.Current?.MainPage is FlyoutPage flyout && flyout.Detail is NavigationPage nav)
            await nav.PopToRootAsync();
    }
}