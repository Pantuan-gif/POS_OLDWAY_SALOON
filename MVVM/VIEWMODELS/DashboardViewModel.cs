using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.MVVM.VIEWS;
using System.Collections.ObjectModel;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class DashboardViewModel : ObservableObject
{
    // ── Stat Cards ──────────────────────────────────────────────────────────

    [ObservableProperty]
    private int _cashierCount = 15;

    [ObservableProperty]
    private int _inventoryCount = 2031;

    [ObservableProperty]
    private decimal _todaysSale = 4231.98m;

    [ObservableProperty]
    private int _transactionsCount = 76;

    public string TodaysSaleDisplay => $"$ {TodaysSale:N2}";

    // ── Recent Transactions ─────────────────────────────────────────────────

    public ObservableCollection<Transaction> RecentTransactions { get; } = new()
    {
        new Transaction
        {
            ProductName  = "Jack Daniels",
            ImageSource  = "jack_daniels.png",
            DateTime     = "Today | 10:00am",
            Category     = "Whisky",
            Amount       = 15.98m,
            Quantity     = 1
        },
        new Transaction
        {
            ProductName  = "Bacardi",
            ImageSource  = "bacardi.png",
            DateTime     = "Yesterday | 12:01pm",
            Category     = "Rum",
            Amount       = 78.00m,
            Quantity     = 2
        },
        new Transaction
        {
            ProductName  = "Absolut Vodka",
            ImageSource  = "absolut_vodka.png",
            DateTime     = "Yesterday | 11:00am",
            Category     = "Vodka",
            Amount       = 10.98m,
            Quantity     = 1
        }
    };

    // ── Navigation helper ───────────────────────────────────────────────────

    private static void NavigateTo(Page page)
    {
        if (Application.Current?.MainPage is FlyoutPage flyout)
        {
            flyout.Detail      = new NavigationPage(page);
        }
    }

    [RelayCommand]
    private void GoToProducts()
        => NavigateTo(AppPages.NewProductManagementView());

    [RelayCommand]
    private void GoToInventory()
        => NavigateTo(AppPages.Inventory);

    [RelayCommand]
    private void GoToReports()
        => NavigateTo(AppPages.NewAddProductView()); // replace with AppPages.Reports when ready
}
