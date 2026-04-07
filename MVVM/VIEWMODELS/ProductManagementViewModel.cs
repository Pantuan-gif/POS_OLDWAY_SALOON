using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.MVVM.VIEWS;
using POS_OLDWAY_SALOON.Services;
using System.Collections.ObjectModel;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class ProductManagementViewModel : ObservableObject
{
    private readonly APISERVICES _api = new APISERVICES();

    [ObservableProperty] private int categoryId;
    [ObservableProperty] private string categoryName = string.Empty;
    [ObservableProperty] private string searchText = string.Empty;

    private List<Product> _allProducts = new();
    public ObservableCollection<Product> FilteredProducts { get; } = new();

    public ProductManagementViewModel()
    {
        // Initial load will be done via SetCategory
    }

    public async void SetCategory(Category category)
    {
        CategoryId = category.CategoryId;
        CategoryName = category.CategoryName;

        var products = await _api.GetProductsByCategoryAsync(category.CategoryId);
        _allProducts = products;
        FilteredProducts.Clear();
        foreach (var p in products)
            FilteredProducts.Add(p);
    }

    partial void OnSearchTextChanged(string value)
    {
        var source = string.IsNullOrWhiteSpace(value)
            ? _allProducts
            : _allProducts.Where(p => p.ProductName.Contains(value, StringComparison.OrdinalIgnoreCase)).ToList();

        FilteredProducts.Clear();
        foreach (var p in source)
            FilteredProducts.Add(p);
    }

    [RelayCommand]
    private async Task AddProduct()
    {
        var page = AppPages.NewAddProductView();
        page.OnProductSaved = async (product) =>
        {
            product.CategoryId = CategoryId;   // important
            await LoadProducts();
        };
        await PushAsync(page);
    }

    [RelayCommand]
    private async Task EditProduct(Product product)
    {
        var page = AppPages.NewAddProductView();
        page.LoadProduct(product);
        page.OnProductSaved = async (updated) =>
        {
            await LoadProducts();
        };
        await PushAsync(page);
    }

    [RelayCommand]
    private async Task DeleteProduct(Product product)
    {
        bool confirm = await Application.Current!.MainPage!.DisplayAlert("Delete", $"Delete {product.ProductName}?", "Yes", "No");
        if (!confirm) return;

        bool success = await _api.DeleteProductAsync(product.ProductId);
        if (success)
            await LoadProducts();
    }

    private async Task LoadProducts()
    {
        var products = await _api.GetProductsByCategoryAsync(CategoryId);
        _allProducts = products;
        FilteredProducts.Clear();
        foreach (var p in products)
            FilteredProducts.Add(p);
    }

    private static async Task PushAsync(Page page)
    {
        if (Application.Current?.MainPage is FlyoutPage flyout && flyout.Detail is NavigationPage nav)
            await nav.PushAsync(page);
    }

    [RelayCommand]
    private async Task GoBack()
    {
        if (Application.Current?.MainPage is FlyoutPage flyout && flyout.Detail is NavigationPage nav)
            await nav.PopAsync();
    }
}