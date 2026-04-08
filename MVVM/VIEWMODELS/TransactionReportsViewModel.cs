using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.MVVM.SERVICES;
using System.Collections.ObjectModel;
using System.Linq;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class TransactionReportsViewModel : ObservableObject
{
    private readonly ObservableCollection<Order> _all = new();

    public ObservableCollection<Order> Transactions { get; } = new();

    [ObservableProperty]
    private string _searchText = string.Empty;

    public TransactionReportsViewModel()
    {
        _ = LoadAsync();
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        try
        {
            _all.Clear();
            Transactions.Clear();
            var list = await TransactionService.GetAllAsync();
            foreach (var o in list.OrderByDescending(x => x.Date))
            {
                _all.Add(o);
                Transactions.Add(o);
            }
        }
        catch
        {
            // ignore for now
        }
    }

    [RelayCommand]
    private void Search()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            Transactions.Clear();
            foreach (var o in _all) Transactions.Add(o);
            return;
        }

        var q = SearchText.Trim().ToLowerInvariant();
        var filtered = _all.Where(o =>
            (o.ReferenceNumber?.ToLowerInvariant().Contains(q) ?? false)
            || (o.PaymentMethod?.ToLowerInvariant().Contains(q) ?? false)
            || o.Items.Any(i => i.ProductName.ToLowerInvariant().Contains(q))
            || o.Date.ToString("g").ToLowerInvariant().Contains(q)
        ).ToList();

        Transactions.Clear();
        foreach (var o in filtered) Transactions.Add(o);
    }

    [RelayCommand]
    private async Task Back()
    {
        if (Application.Current?.MainPage is FlyoutPage flyout && flyout.Detail is NavigationPage nav)
            await nav.PopAsync();
    }

    [RelayCommand]
    private async Task ItemSelected(object args)
    {
        try
        {
            var selected = (args as Microsoft.Maui.Controls.SelectionChangedEventArgs)?.CurrentSelection?.FirstOrDefault() as Order;
            if (selected == null) return;

            if (Application.Current?.MainPage is FlyoutPage flyout && flyout.Detail is NavigationPage nav)
            {
                await nav.PushAsync(new MVVM.VIEWS.Reciept { BindingContext = selected });
            }
        }
        catch
        {
            // ignore
        }
    }
}
