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

    // Search / Category filtering
    [ObservableProperty]
    private string _searchText = string.Empty;

    partial void OnSearchTextChanged(string value) => FilterProducts();

    private readonly ObservableCollection<Category> _allCategories = new();
    public ObservableCollection<Category> Categories { get; } = new();

    [ObservableProperty]
    private Category _selectedCategory;

    partial void OnSelectedCategoryChanged(Category value) => FilterProducts();

    private readonly ObservableCollection<Product> _allProducts = new();
    public ObservableCollection<ProductSelection> Products { get; } = new();

    public OrderingManagementViewModel()
    {
        _ = LoadProductsAsync();
        _ = LoadCategoriesAsync();
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
            foreach (var p in products)
                _allProducts.Add(p);

            FilterProducts();
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

    [RelayCommand]
    private async Task LoadCategoriesAsync()
    {
        try
        {
            var cats = await _api.GetCategoriesAsync();
            _allCategories.Clear();
            Categories.Clear();
            foreach (var c in cats)
                _allCategories.Add(c);
            // Add an "All" pseudo-category so users can view every product
            var allCat = new Category { CategoryId = 0, CategoryName = "All", Id = "0", PhotoPath = string.Empty };
            Categories.Add(allCat);

            foreach (var c in _allCategories)
                Categories.Add(c);

            // Select "All" by default
            SelectedCategory = allCat;
        }
        catch { /* ignore */ }
    }

    private void FilterProducts()
    {
        Products.Clear();

        var query = _allProducts.AsEnumerable();

        if (SelectedCategory is not null && SelectedCategory.CategoryId != 0)
            query = query.Where(p => p.CategoryId == SelectedCategory.CategoryId);

        if (!string.IsNullOrWhiteSpace(SearchText))
            query = query.Where(p => p.ProductName.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

        foreach (var p in query)
            Products.Add(new ProductSelection(p));
    }

    // Public wrapper so views can explicitly request a refresh without depending
    // on generated command members. This avoids issues if source generators
    // are not available at design-time.
    public Task EnsureLoadedAsync() => LoadProductsAsync();

}
