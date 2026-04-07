using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using System.Collections.ObjectModel;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class ProductManagementViewModel : ObservableObject
{
    // ── Query Params ────────────────────────────────────────────────────────

    [ObservableProperty]
    private int _categoryId;

    [ObservableProperty]
    private string _categoryName = string.Empty;

    // ── Search ──────────────────────────────────────────────────────────────

    [ObservableProperty]
    private string _searchText = string.Empty;

    partial void OnSearchTextChanged(string value) => FilterProducts();

    // ── Products ────────────────────────────────────────────────────────────

    private readonly ObservableCollection<Product> _allProducts = new()
    {
        new Product { ProductId = 1, ProductName = "Jack Daniels",  CategoryId = 1, SizeMl = 750, Price = 15.98m, Quantity = 78,  PhotoPath = "jack_daniels.png"  },
        new Product { ProductId = 2, ProductName = "Black Label",   CategoryId = 2, SizeMl = 500, Price = 13.00m, Quantity = 89,  PhotoPath = "black_label.png"   },
        new Product { ProductId = 3, ProductName = "Chivas Regal",  CategoryId = 3, SizeMl = 750, Price = 17.88m, Quantity = 50,  PhotoPath = "chivas_regal.png"  },
        new Product { ProductId = 4, ProductName = "The Macallan",  CategoryId = 4, SizeMl = 500, Price = 12.08m, Quantity = 23,  PhotoPath = "the_macallan.png"  },
    };

    public ObservableCollection<Product> FilteredProducts { get; } = new();

    // ── Constructor ─────────────────────────────────────────────────────────

    public ProductManagementViewModel()
    {
        FilterProducts();
    }

    // Called from ProductManagementView when a Category is passed in
    public void SetCategory(Category category)
    {
        CategoryId   = category.CategoryId;
        CategoryName = category.CategoryName;
        FilterProducts();
    }

    // ── Filter Logic ────────────────────────────────────────────────────────

    private void FilterProducts()
    {
            FilteredProducts.Clear();

    var query = _allProducts
        .Where(p => p.CategoryId == CategoryId); // always filter by category first

    if (!string.IsNullOrWhiteSpace(SearchText))
    {
        query = query.Where(p =>
            p.ProductName.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
    }

    foreach (var p in query)
        FilteredProducts.Add(p);

    }

    // ── Navigation helper ───────────────────────────────────────────────────

    private static async Task PushAsync(Page page)
    {
        if (Application.Current?.MainPage is FlyoutPage flyout
            && flyout.Detail is NavigationPage nav)
            await nav.PushAsync(page);
    }

    private static async Task PopAsync()
    {
        if (Application.Current?.MainPage is FlyoutPage flyout
            && flyout.Detail is NavigationPage nav)
            await nav.PopAsync();
    }

    // ── Commands ────────────────────────────────────────────────────────────

    [RelayCommand]
    private async Task AddProduct()
    {
        var page = AppPages.NewAddProductView();
        page.OnProductSaved = (product) => AddProduct(product);
        await PushAsync(page);
    }

    [RelayCommand]
    private async Task EditProduct(Product product)
    {
        var page = AppPages.NewAddProductView();
        page.LoadProduct(product);
        page.OnProductSaved = (updated) =>
        {
            var index = _allProducts.IndexOf(product);
            if (index >= 0) _allProducts[index] = updated;
            FilterProducts();
        };
        await PushAsync(page);
    }

    [RelayCommand]
    private async Task DeleteProduct(Product product)
    {
        bool confirm = await Application.Current!.MainPage!.DisplayAlert(
            "Delete", $"Delete {product.ProductName}?", "Yes", "No");
        if (!confirm) return;
        _allProducts.Remove(product);
        FilterProducts();
    }

    [RelayCommand]
    private async Task GoBack() => await PopAsync();

    public void AddProduct(Product product)
    {
        _allProducts.Add(product);
        FilterProducts();
    }
}
