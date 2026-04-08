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
            flyout.Detail = new NavigationPage(page);

            // Also update the flyout selection if possible (keeps UI in sync when navigation
            // is triggered from other pages such as the Dashboard)
            try
            {
                if (flyout.Flyout is MVVM.VIEWS.FlyoutMenu menu && menu.collectionView != null)
                {
                    var items = menu.collectionView.ItemsSource as System.Collections.IEnumerable;
                    if (items != null)
                    {
                        foreach (var obj in items)
                        {
                            if (obj is MVVM.MODELS.FlyoutMenuItem item)
                            {
                                if (item.TargetPage == page.GetType() || string.Equals(item.Title, page.Title, StringComparison.OrdinalIgnoreCase))
                                {
                                    menu.collectionView.SelectedItem = item;
                                    break;
                                }
                                // If a cashier was redirected to Inventory, prefer selecting Orders in the flyout
                                if (POS_OLDWAY_SALOON.Services.AuthService.IsInRole("Cashier")
                                    && page.GetType() == typeof(POS_OLDWAY_SALOON.MVVM.VIEWS.InventoryManagementView))
                                {
                                    var orders = items.Cast<object?>().FirstOrDefault(o => o is MVVM.MODELS.FlyoutMenuItem f && f.TargetPage == typeof(POS_OLDWAY_SALOON.MVVM.VIEWS.OrderingManagementView));
                                    if (orders is MVVM.MODELS.FlyoutMenuItem ordersItem)
                                    {
                                        menu.collectionView.SelectedItem = ordersItem;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch { /* silently ignore if flyout/menu not present */ }
        }
    }

    [RelayCommand]
    private void GoToProducts()
        => NavigateTo(AppPages.NewProductManagementView());

    [RelayCommand]
    private void GoToInventory()
    {
        // If current user is a cashier, redirect to ordering management
        if (POS_OLDWAY_SALOON.Services.AuthService.IsInRole("Manager"))
            NavigateTo(AppPages.NewOrderingManagementView());
        else
            NavigateTo(AppPages.Inventory);
    }

    [RelayCommand]
    private void GoToReports()
        => NavigateTo(AppPages.Reports);
}
