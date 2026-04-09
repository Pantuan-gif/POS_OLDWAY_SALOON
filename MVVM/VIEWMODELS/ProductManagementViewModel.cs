using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using System.Linq;
using POS_OLDWAY_SALOON.Services;
using System.Collections.ObjectModel;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class ProductManagementViewModel : ObservableObject
{
    private readonly APISERVICES _api = new();

    // ── Query Params ─────────────────────────────────────────────────────────

    [ObservableProperty]
    private int _categoryId;

    [ObservableProperty]
    private string _categoryName = string.Empty;

    // ── Search ───────────────────────────────────────────────────────────────

    [ObservableProperty]
    private string _searchText = string.Empty;

    partial void OnSearchTextChanged(string value) => FilterProducts();

    // ── Loading / Error state ────────────────────────────────────────────────

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    // ── Products ─────────────────────────────────────────────────────────────

    private readonly ObservableCollection<Product> _allProducts = new();

    public ObservableCollection<Product> FilteredProducts { get; } = new();

    // ── Constructor ───────────────────────────────────────────────────────────

    public ProductManagementViewModel() { }

    // Called from ProductManagementView when a Category is passed in
    public void SetCategory(Category category)
    {
        CategoryId   = category.CategoryId;
        CategoryName = category.CategoryName;
        _ = LoadProductsAsync();
    }

    // ── API: Load ─────────────────────────────────────────────────────────────

    [RelayCommand]
    private async Task LoadProductsAsync()
    {
        IsBusy = true;
        ErrorMessage = string.Empty;
        try
        {
            var products = await _api.GetProductsByCategoryAsync(CategoryId);
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

    // ── Filter Logic ──────────────────────────────────────────────────────────

    private void FilterProducts()
    {
        FilteredProducts.Clear();

        // If CategoryId is 0 (not set) treat as all categories
        IEnumerable<Product> query = (CategoryId == 0)
            ? _allProducts
            : _allProducts.Where(p => p.CategoryId == CategoryId);

        if (!string.IsNullOrWhiteSpace(SearchText))
            query = query.Where(p => p.ProductName.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

        foreach (var p in query)
            FilteredProducts.Add(p);
    }

    // ── Navigation helpers ────────────────────────────────────────────────────

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

    // ── Commands ──────────────────────────────────────────────────────────────

    [RelayCommand]
    public async Task AddProduct()
    {
        var page = AppPages.NewAddProductView();
        page.setCategory(CategoryId, CategoryName);
        page.OnProductSaved = async (product) =>
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            try
            {
                var saved = await _api.AddProductAsync(product);
                if (saved is not null)
                {
                    _allProducts.Add(saved);
                    FilterProducts();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to add product: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        };
        await PushAsync(page);
    }

    // Called by external pages when a product has been saved and should be added to the list
    public void AddProduct(Product product)
    {
        if (product == null) return;
        _allProducts.Add(product);
        FilterProducts();
    }

    [RelayCommand]
    private async Task EditProduct(Product product)
    {
        var page = AppPages.NewEditProductPage();
        page.LoadProduct(product);
        page.SetCategory(CategoryId, CategoryName);
        // The EditProductPageViewModel calls the API internally;
        // we only need to refresh the local list on success.
        page.OnProductSaved = (saved) =>
        {
            var index = _allProducts.IndexOf(product);
            if (index >= 0) _allProducts[index] = saved;
            FilterProducts();
        };
        await PushAsync(page);
    }

    [RelayCommand]
    private async Task DeleteProduct(Product product)
    {
        bool confirm = await Application.Current!.MainPage!.DisplayAlert(
            "Delete", $"Delete \"{product.ProductName}\"?", "Yes", "No");
        if (!confirm) return;

        IsBusy = true;
        ErrorMessage = string.Empty;
        try
        {
            // Use server id when available (mockapi uses string ids)
            var serverId = product.Id ?? product.ProductId.ToString();
            bool ok = await _api.DeleteProductAsync(serverId);
            if (ok)
            {
                // Remove from master list first
                var original = _allProducts.FirstOrDefault(p => p.ProductId == product.ProductId);
                if (original != null)
                    _allProducts.Remove(original);

                // Recompute filtered list to reflect deletion
                FilterProducts();

                await Application.Current.MainPage.DisplayAlert("Deleted", $"{product.ProductName} has been deleted.", "OK");
            }
        }
        catch (Exception ex)
        {
            // Show error details returned by API or transport
            ErrorMessage = $"Failed to delete product: {ex.Message}";
            await Application.Current.MainPage.DisplayAlert("Error", ErrorMessage, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task GoBack() => await PopAsync();
}
