using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.MVVM.SERVICES;
using System.Collections.ObjectModel;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class TransactionReportsViewModel : ObservableObject
{
    // ── Master collection (unfiltered) ────────────────────────────────────────
    private readonly List<Order> _all = new();

    // ── Displayed collection ──────────────────────────────────────────────────
    public ObservableCollection<Order> Transactions { get; } = new();

    // ── State ─────────────────────────────────────────────────────────────────

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    // ── Search & date filter ──────────────────────────────────────────────────

    [ObservableProperty]
    private string _searchText = string.Empty;

    partial void OnSearchTextChanged(string value) => ApplyFilters();

    [ObservableProperty]
    private DateTime _filterDate = DateTime.Today;

    partial void OnFilterDateChanged(DateTime value) => ApplyFilters();

    [ObservableProperty]
    private bool _isDateFilterActive;

    partial void OnIsDateFilterActiveChanged(bool value) => ApplyFilters();

    // ── Summary KPIs ──────────────────────────────────────────────────────────

    [ObservableProperty]
    private int _totalTransactions;

    [ObservableProperty]
    private decimal _totalRevenue;

    [ObservableProperty]
    private int _totalItemsSold;

    public string TotalRevenueDisplay => $"₱{TotalRevenue:N2}";

    // ── Constructor ───────────────────────────────────────────────────────────

    public TransactionReportsViewModel()
    {
        // Default to showing all transactions
        IsDateFilterActive = false;
        FilterDate = DateTime.Today;
        FilterLabel = "All transactions";
        _ = LoadAsync();
    }

    [ObservableProperty]
    private int _loadedTransactionsCount;

    [ObservableProperty]
    private int _displayedTransactionsCount;

    // ── Commands ──────────────────────────────────────────────────────────────

    [RelayCommand]
    private async Task Load() => await LoadAsync();

    [RelayCommand]
    private void Search() => ApplyFilters();

    [RelayCommand]
    private void ClearDateFilter()
    {
        IsDateFilterActive = false;
        FilterDate = DateTime.Today;
    }

    [RelayCommand]
    private void EnableDateFilter()
    {
        IsDateFilterActive = true;
    }

    [RelayCommand]
    private async Task Back()
    {
        if (Application.Current?.MainPage is FlyoutPage flyout
            && flyout.Detail is NavigationPage nav)
            await nav.PopAsync();
    }

    [RelayCommand]
    private async Task ItemSelected(object args)
    {
        // Show modal details dialog instead of navigating to a page
        try
        {
            Order selected = null;
            if (args is Microsoft.Maui.Controls.SelectionChangedEventArgs sce)
                selected = sce.CurrentSelection?.FirstOrDefault() as Order;
            else if (args is Order o)
                selected = o;

            if (selected is null) return;

            // Build a simple modal page to display receipt-like details
            var page = new ContentPage
            {
                Title = "Transaction",
                Content = new ScrollView
                {
                    Content = new VerticalStackLayout
                    {
                        Padding = 16,
                        Spacing = 10,
                        Children =
                        {
                            new Label { Text = $"Ref: {selected.ReferenceNumber}", FontAttributes = FontAttributes.Bold },
                            new Label { Text = $"Date: {selected.Date:g}" },
                            new Label { Text = $"Operator: {selected.OperatorName}" },
                            new Label { Text = $"Payment: {selected.PaymentMethod}" },
                            new BoxView { HeightRequest = 1, Color = Colors.LightGray },
                        }
                    }
                }
            };

            // Add items list
            var itemsLayout = new VerticalStackLayout { Spacing = 6 };
            foreach (var it in selected.Items)
            {
                itemsLayout.Children.Add(new Grid
                {
                    ColumnDefinitions = new ColumnDefinitionCollection { new ColumnDefinition { Width = GridLength.Star }, new ColumnDefinition { Width = new GridLength(80) } },
                    Children =
                    {
                        new Label { Text = it.ProductName },
                        new Label { Text = $"x{it.Quantity} @ ₱{it.UnitPrice:F2}", HorizontalTextAlignment = TextAlignment.End }
                    }
                });
            }

            ((page.Content as ScrollView).Content as VerticalStackLayout).Children.Add(itemsLayout);
            ((page.Content as ScrollView).Content as VerticalStackLayout).Children.Add(new BoxView { HeightRequest = 1, Color = Colors.LightGray });
            ((page.Content as ScrollView).Content as VerticalStackLayout).Children.Add(new Label { Text = $"Total: ₱{selected.Total:F2}", FontAttributes = FontAttributes.Bold });

            await Application.Current!.MainPage!.Navigation.PushModalAsync(new NavigationPage(page));
        }
        catch { /* ignore modal errors */ }
    }

    // ── Data loading ──────────────────────────────────────────────────────────

    private async Task LoadAsync()
    {
        IsBusy = true;
        ErrorMessage = string.Empty;

        try
        {
            var list = await TransactionService.GetAllAsync();
            if (list == null || list.Count == 0)
            {
#if DEBUG
                // For developer convenience: seed a small sample transaction when none exist on device
                var sample = new List<Order>
                {
                    new Order
                    {
                        Date = DateTime.Now,
                        PaymentMethod = "Cash",
                        OperatorName = "Debug User",
                        TenderedAmount = 1000m,
                        Items = new List<CartItem>
                        {
                            new CartItem { ProductId = 1, ProductName = "Jack Daniels", Quantity = 1, UnitPrice = 1800m, PhotoPath = "jack_daniels.png" }
                        }
                    }
                };
                await TransactionService.SaveAsync(sample);
                list = await TransactionService.GetAllAsync();
#else
                // no transactions found
                _all.Clear();
                Transactions.Clear();
                TotalTransactions = 0;
                TotalRevenue = 0m;
                TotalItemsSold = 0;
                return;
#endif
            }

            _all.Clear();
            foreach (var o in list.OrderByDescending(x => x.Date))
                _all.Add(o);

            ApplyFilters();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load transactions: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    // ── Filtering & summary refresh ───────────────────────────────────────────

    private void ApplyFilters()
    {
        IEnumerable<Order> result = _all;

        // Date filter
        if (IsDateFilterActive)
            result = result.Where(o => o.Date.Date == FilterDate.Date);

        // Text search — ref#, payment method, product name, date string
        var q = SearchText?.Trim().ToLowerInvariant() ?? string.Empty;
        if (!string.IsNullOrEmpty(q))
        {
            result = result.Where(o =>
                (o.ReferenceNumber?.ToLowerInvariant().Contains(q) ?? false)
                || (o.PaymentMethod?.ToLowerInvariant().Contains(q) ?? false)
                || o.Items.Any(i => i.ProductName.ToLowerInvariant().Contains(q))
                || o.Date.ToString("g").ToLowerInvariant().Contains(q));
        }

        var filtered = result.ToList();

        Transactions.Clear();
        foreach (var o in filtered)
            Transactions.Add(o);

        // counts
        LoadedTransactionsCount = _all.Count;
        DisplayedTransactionsCount = filtered.Count;

        // KPIs
        TotalTransactions = filtered.Count;
        TotalRevenue      = filtered.Sum(o => o.Total);
        TotalItemsSold    = filtered.Sum(o => o.ItemCount);
        OnPropertyChanged(nameof(TotalRevenueDisplay));

        // Update filter label for UI
        if (IsDateFilterActive)
        {
            if (FilterDate.Date == DateTime.Today)
                FilterLabel = "Today's transactions";
            else
                FilterLabel = $"Specified date: {FilterDate:MMM dd, yyyy}";
        }
        else if (!string.IsNullOrWhiteSpace(SearchText))
        {
            FilterLabel = $"Search: {SearchText}";
        }
        else
        {
            FilterLabel = "All transactions";
        }
    }

    [ObservableProperty]
    private string _filterLabel = string.Empty;
}
