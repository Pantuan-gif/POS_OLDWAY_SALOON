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
        _ = LoadAsync();
    }

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
        try
        {
            var selected = (args as Microsoft.Maui.Controls.SelectionChangedEventArgs)
                               ?.CurrentSelection?.FirstOrDefault() as Order;
            if (selected is null) return;

            if (Application.Current?.MainPage is FlyoutPage flyout
                && flyout.Detail is NavigationPage nav)
            {
                await nav.PushAsync(new MVVM.VIEWS.Reciept { BindingContext = selected });
            }
        }
        catch { /* ignore navigation errors */ }
    }

    // ── Data loading ──────────────────────────────────────────────────────────

    private async Task LoadAsync()
    {
        IsBusy = true;
        ErrorMessage = string.Empty;

        try
        {
            var list = await TransactionService.GetAllAsync();

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

        // KPIs
        TotalTransactions = filtered.Count;
        TotalRevenue      = filtered.Sum(o => o.Total);
        TotalItemsSold    = filtered.Sum(o => o.ItemCount);
        OnPropertyChanged(nameof(TotalRevenueDisplay));
    }
}
