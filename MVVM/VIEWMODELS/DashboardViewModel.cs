using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.MVVM.VIEWS;
using System.Collections.ObjectModel;
using System.Linq;
using POS_OLDWAY_SALOON.Services;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class DashboardViewModel : ObservableObject
{
    private readonly APISERVICES _api = new();
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

    // keep recent orders to show full receipt when tapped
    private readonly List<MVVM.MODELS.Order> _recentOrders = new();

    [RelayCommand]
    private async Task ItemSelected(object args)
    {
        try
        {
            var tapped = args as MVVM.MODELS.Transaction;
            if (tapped == null) return;

            var index = RecentTransactions.IndexOf(tapped);
            if (index < 0 || index >= _recentOrders.Count) return;

            var selected = _recentOrders[index];

            // Build modal page similar to TransactionReportsViewModel
            var page = new ContentPage
            {
                Title = "Receipt",
                Content = new ScrollView
                {
                    Content = new VerticalStackLayout
                    {
                        Padding = 16,
                        Spacing = 10
                    }
                }
            };

            var layout = ((page.Content as ScrollView)!.Content as VerticalStackLayout)!;

            // Close button
            var close = new Button { Text = "Close", BackgroundColor = Colors.Transparent, TextColor = Colors.Black };
            close.Clicked += async (s, e) => await Application.Current!.MainPage!.Navigation.PopModalAsync();
            layout.Children.Add(close);

            layout.Children.Add(new Label { Text = $"Ref: {selected.ReferenceNumber}", FontAttributes = FontAttributes.Bold });
            layout.Children.Add(new Label { Text = $"Date: {selected.Date:g}" });
            layout.Children.Add(new Label { Text = $"Operator: {selected.OperatorName}" });
            layout.Children.Add(new Label { Text = $"Payment: {selected.PaymentMethod}" });
            layout.Children.Add(new BoxView { HeightRequest = 1, Color = Colors.LightGray });

            var itemsLayout = new VerticalStackLayout { Spacing = 6 };
            foreach (var it in selected.Items)
            {
                var grid = new Grid
                {
                    ColumnDefinitions = new ColumnDefinitionCollection { new ColumnDefinition { Width = GridLength.Star }, new ColumnDefinition { Width = new GridLength(80) } },
                    Children =
                    {
                        new Label { Text = it.ProductName },
                        new Label { Text = $"x{it.Quantity} @ ₱{it.UnitPrice:F2}", HorizontalTextAlignment = TextAlignment.End }
                    }
                };
                itemsLayout.Children.Add(grid);
            }

            layout.Children.Add(itemsLayout);
            layout.Children.Add(new BoxView { HeightRequest = 1, Color = Colors.LightGray });
            layout.Children.Add(new Label { Text = $"Total: ₱{selected.Total:F2}", FontAttributes = FontAttributes.Bold });

            await Application.Current!.MainPage!.Navigation.PushModalAsync(new NavigationPage(page));
        }
        catch { /* ignore */ }
    }

    public DashboardViewModel()
    {
        // Populate dashboard stats and recent transactions
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await LoadDashboardStatsAsync();
        await LoadRecentTransactionsAsync();
    }

    private async Task LoadDashboardStatsAsync()
    {
        try
        {
            // Cashier count from user list
            try
            {
                var users = LoginViewModels.User;
                // Prefer counting active cashiers, but fall back to any user with role "Cashier" if IsActive is not set
                var activeCount = users.Count(u => string.Equals(u.Role, "Cashier", StringComparison.OrdinalIgnoreCase) && u.IsActive);
                CashierCount = activeCount > 0 ? activeCount : users.Count(u => string.Equals(u.Role, "Cashier", StringComparison.OrdinalIgnoreCase));
            }
            catch { /* ignore user lookup errors */ }

            // Inventory count from products API
            try
            {
                var products = await _api.GetProductsAsync();
                InventoryCount = products?.Count ?? 0;
            }
            catch { InventoryCount = 0; }

            // Transactions and today's sales from local transactions store
            try
            {
                var orders = await MVVM.SERVICES.TransactionService.GetAllAsync();
                TransactionsCount = orders?.Count ?? 0;
                var today = DateTime.Today;
                TodaysSale = orders?.Where(o => o.Date.Date == today).Sum(o => o.Total) ?? 0m;
                OnPropertyChanged(nameof(TodaysSaleDisplay));
            }
            catch { TransactionsCount = 0; TodaysSale = 0m; }
        }
        catch { /* ignore overall errors */ }
    }

    private async Task LoadRecentTransactionsAsync()
    {
        try
        {
            var orders = await MVVM.SERVICES.TransactionService.GetAllAsync();
            RecentTransactions.Clear();
            _recentOrders.Clear();
            foreach (var o in orders.OrderByDescending(x => x.Date).Take(5))
            {
                if (o.Items?.Count > 0)
                {
                    var first = o.Items[0];
                    RecentTransactions.Add(new Transaction
                    {
                        ProductName = first.ProductName,
                        ImageSource = first.PhotoPath,
                        DateTime = o.Date.ToString("MMM dd | hh:mm tt"),
                        Category = "", // category not stored on order lines
                        Amount = o.Total,
                        Quantity = o.ItemCount
                    });
                    _recentOrders.Add(o);
                }
            }
        }
        catch { }
    }

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
                                    var orders = items.Cast<object?>().FirstOrDefault(o => o is MVVM.MODELS.FlyoutMenuItem f && (f.TargetPage == typeof(POS_OLDWAY_SALOON.MVVM.VIEWS.OrderingManagement) || string.Equals(f.Title, "Orders", StringComparison.OrdinalIgnoreCase)));
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
        => NavigateTo(AppPages.NewOrderingManagementView());

    [RelayCommand]
    private void GoToInventory()
    {
        // If current user is a cashier, redirect to ordering management
        if (POS_OLDWAY_SALOON.Services.AuthService.IsInRole("Cashier"))
            NavigateTo(AppPages.NewOrderingManagementView());
        else
            NavigateTo(AppPages.Inventory);
    }

    [RelayCommand]
    private void GoToReports()
        => NavigateTo(AppPages.Reports);
}
