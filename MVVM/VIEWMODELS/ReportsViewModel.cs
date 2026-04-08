using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.VIEWS;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class ReportsViewModel : ObservableObject
{
    // ── Navigation helper ────────────────────────────────────────────────────

    private static async Task PushAsync(Page page)
    {
        if (Application.Current?.MainPage is FlyoutPage flyout
            && flyout.Detail is NavigationPage nav)
            await nav.PushAsync(page);
    }

    // ── Hamburger / flyout toggle ─────────────────────────────────────────────

    [RelayCommand]
    private void Menu()
    {
        if (Application.Current?.MainPage is FlyoutPage flyout)
            flyout.IsPresented = !flyout.IsPresented;
    }

    // ── Report sub-page navigation ────────────────────────────────────────────

    [RelayCommand]
    private async Task NavigateTransactionHistory()
        => await PushAsync(new TransactionReports());

    [RelayCommand]
    private async Task NavigateInventoryReport()
        => await PushAsync(new InventoryReport());

    [RelayCommand]
    private async Task NavigateOutOfStock()
        => await PushAsync(new OutofStock());
}
