using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.MVVM.SERVICES;
using POS_OLDWAY_SALOON.Services;
using System.Collections.ObjectModel;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

/// <summary>
/// A flat, display-ready projection of a Product enriched with sales data.
/// </summary>
public class InventoryReportItem
{
    // link back to product id
    public int ProductId { get; init; }
    // ── Product fields ──────────────────────────────────────────────────────
    public string Image   { get; init; } = string.Empty;
    public string Name    { get; init; } = string.Empty;
    public string Type    { get; init; } = string.Empty;   // CategoryId label
    public string Volume  { get; init; } = string.Empty;   // SizeMl formatted
    public decimal Price  { get; init; }
    public int StocksLeft { get; init; }

    // ── Computed sales metrics ──────────────────────────────────────────────
    public int SoldToday  { get; init; }

    // ── Display helpers ─────────────────────────────────────────────────────
    public string PriceDisplay     => $"₱{Price:F2}";
    public string StocksDisplay    => $"Stocks left: {StocksLeft}";
    public string SoldTodayDisplay => $"Sold today: {SoldToday}";
}

public partial class InventoryReportViewModel : ObservableObject
{
    private readonly APISERVICES _api = new();

    // low stock threshold (configurable)
    [ObservableProperty]
    private int _lowStockThreshold = 5;

    [ObservableProperty]
    private string _viewMode = "All"; // All | LowStock | OutOfStock

    public ObservableCollection<InventoryReportItem> LowStockItems { get; } = new();
    public ObservableCollection<InventoryReportItem> OutOfStockItems { get; } = new();

    // ── State ────────────────────────────────────────────────────────────────

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private string _searchText = string.Empty;

    partial void OnSearchTextChanged(string value) => ApplyFilter();

    // ── Collections ──────────────────────────────────────────────────────────

    private readonly List<InventoryReportItem> _allItems = new();

    public ObservableCollection<InventoryReportItem> Products { get; } = new();

    // ── Summary stats (header row) ────────────────────────────────────────────

    [ObservableProperty]
    private int _totalProducts;

    [ObservableProperty]
    private int _totalStock;

    [ObservableProperty]
    private int _totalSoldToday;

    // ── Constructor ──────────────────────────────────────────────────────────

    public InventoryReportViewModel()
    {
        _ = LoadAsync();
    }

    [RelayCommand]
    private void SetViewMode(string mode)
    {
        ViewMode = mode;
        ApplyFilter();
    }

    // ── Commands ─────────────────────────────────────────────────────────────

    [RelayCommand]
    private async Task Load() => await LoadAsync();

    [RelayCommand]
    private void Search() => ApplyFilter();

    [RelayCommand]
    private async Task Back()
    {
        if (Application.Current?.MainPage is FlyoutPage flyout
            && flyout.Detail is NavigationPage nav)
            await nav.PopAsync();
    }

    // ── Data loading ─────────────────────────────────────────────────────────

    private async Task LoadAsync()
    {
        IsBusy = true;
        ErrorMessage = string.Empty;

        try
        {
            // Load products and categories in parallel
            var productsTask    = _api.GetProductsAsync();
            var categoriesTask  = _api.GetCategoriesAsync();
            var ordersTask      = TransactionService.GetAllAsync();

            await Task.WhenAll(productsTask, categoriesTask, ordersTask);

            var products    = productsTask.Result;
            var categories  = categoriesTask.Result;
            var orders      = ordersTask.Result;

            // Build a quick lookup: categoryId → categoryName
            var catMap = categories.ToDictionary(
                c => c.CategoryId,
                c => c.CategoryName,
                EqualityComparer<int>.Default);

            // Compute units sold today per productId from all orders placed today
            var today = DateTime.Today;
            var soldTodayMap = orders
                .Where(o => o.Date.Date == today)
                .SelectMany(o => o.Items)
                .GroupBy(i => i.ProductId)
                .ToDictionary(g => g.Key, g => g.Sum(i => i.Quantity));

            _allItems.Clear();
            foreach (var p in products)
            {
                soldTodayMap.TryGetValue(p.ProductId, out int soldToday);
                catMap.TryGetValue(p.CategoryId, out string? catName);

                _allItems.Add(new InventoryReportItem
                {
                    ProductId  = p.ProductId,
                    Image      = p.PhotoPath,
                    Name       = p.ProductName,
                    Type       = catName ?? "—",
                    Volume     = p.SizeMl > 0 ? $"{p.SizeMl}ml" : string.Empty,
                    Price      = p.Price,
                    StocksLeft = p.Quantity,
                    SoldToday  = soldToday
                });
            }

            // Build low-stock and out-of-stock lists
            LowStockItems.Clear();
            OutOfStockItems.Clear();
            foreach (var it in _allItems)
            {
                if (it.StocksLeft == 0) OutOfStockItems.Add(it);
                else if (it.StocksLeft <= LowStockThreshold) LowStockItems.Add(it);
            }

            ApplyFilter();
            RefreshSummary();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load inventory: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    // ── Filtering ────────────────────────────────────────────────────────────

    private void ApplyFilter()
    {
        var q = SearchText?.Trim() ?? string.Empty;

        IEnumerable<InventoryReportItem> filtered = _allItems;

        if (!string.IsNullOrEmpty(q))
            filtered = filtered.Where(i => i.Name.Contains(q, StringComparison.OrdinalIgnoreCase) || i.Type.Contains(q, StringComparison.OrdinalIgnoreCase));

        // apply view mode filters
        if (string.Equals(ViewMode, "LowStock", StringComparison.OrdinalIgnoreCase))
            filtered = filtered.Where(i => i.StocksLeft > 0 && i.StocksLeft <= LowStockThreshold);
        else if (string.Equals(ViewMode, "OutOfStock", StringComparison.OrdinalIgnoreCase))
            filtered = filtered.Where(i => i.StocksLeft == 0);

        Products.Clear();
        foreach (var item in filtered)
            Products.Add(item);

        RefreshSummary();
    }

    private void RefreshSummary()
    {
        TotalProducts  = Products.Count;
        TotalStock     = Products.Sum(i => i.StocksLeft);
        TotalSoldToday = Products.Sum(i => i.SoldToday);
    }
}
