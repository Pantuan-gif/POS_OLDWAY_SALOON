using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.Services;
using System.Collections.ObjectModel;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class OrderingManagementViewModel : ObservableObject
{
    private readonly APISERVICES _api = new();

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    private readonly ObservableCollection<Product> _allProducts = new();
    public ObservableCollection<ProductSelection> Products { get; } = new();

    public OrderingManagementViewModel()
    {
        _ = LoadProductsAsync();
    }

    public string CartSummaryTotalItems => $"Total Item: {MVVM.SERVICES.CartService.TotalItems}";
    public string CartSummaryTotalPrice => MVVM.SERVICES.CartService.TotalPrice.ToString("C2");

    [RelayCommand]
    private async Task Proceed()
    {
        if (Application.Current?.MainPage is FlyoutPage flyout && flyout.Detail is NavigationPage nav)
        {
            // Navigate to Cart or directly to Payment — go to Payment as requested
            await nav.PushAsync(new MVVM.VIEWS.Payment());
        }
    }

    [RelayCommand]
    private void IncrementQuantity(ProductSelection selection)
    {
        if (selection == null) return;
        selection.SelectedQuantity++;
    }

    [RelayCommand]
    private void DecrementQuantity(ProductSelection selection)
    {
        if (selection == null) return;
        if (selection.SelectedQuantity > 1) selection.SelectedQuantity--;
    }

    [RelayCommand]
    private void AddSelectedToCart(ProductSelection selection)
    {
        if (selection == null) return;
        POS_OLDWAY_SALOON.MVVM.SERVICES.CartService.AddToCart(selection.Product, selection.SelectedQuantity);
        // notify cart summary changed
        OnPropertyChanged(nameof(CartSummaryTotalItems));
        OnPropertyChanged(nameof(CartSummaryTotalPrice));
    }

    [RelayCommand]
    private async Task LoadProductsAsync()
    {
        IsBusy = true;
        ErrorMessage = string.Empty;
        try
        {
            var products = await _api.GetProductsAsync();
            _allProducts.Clear();
            Products.Clear();
            foreach (var p in products)
                _allProducts.Add(p);

            Products.Clear();
            foreach (var p in _allProducts)
                Products.Add(new ProductSelection(p));
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load products: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }

    }

    // Public wrapper so views can explicitly request a refresh without depending
    // on generated command members. This avoids issues if source generators
    // are not available at design-time.
    public Task EnsureLoadedAsync() => LoadProductsAsync();

}
