using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.Services;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class EditProductPageViewModel : ObservableObject
{
    // (No direct API call here) Save will behave like Add: return product via callback

    // ── Callback fired after a successful save ────────────────────────────────
    public Action<Product>? OnSaved { get; set; }

    // ── Internal state ────────────────────────────────────────────────────────
    private int _originalProductId;

    // ── Observable Properties ─────────────────────────────────────────────────

    [ObservableProperty]
    private int _productId;

    [ObservableProperty]
    private int _categoryId;

    [ObservableProperty]
    private string _categoryName = string.Empty;

    [ObservableProperty]
    private string _productName = string.Empty;

    [ObservableProperty]
    private double _sizeMl;

    [ObservableProperty]
    private decimal _price;

    [ObservableProperty]
    private int _quantity;

    [ObservableProperty]
    private string _photoPath = string.Empty;

    [ObservableProperty]
    private string _photoFileName = "No file chosen";

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    // ── Load existing product ─────────────────────────────────────────────────

    public void LoadProduct(Product product)
    {
        _originalProductId = product.ProductId;
        ProductId    = product.ProductId;
        CategoryId   = product.CategoryId;
        ProductName  = product.ProductName;
        SizeMl       = product.SizeMl;
        Price        = product.Price;
        Quantity     = product.Quantity;
        PhotoPath    = product.PhotoPath;
        PhotoFileName = string.IsNullOrEmpty(product.PhotoPath)
            ? "No file chosen"
            : Path.GetFileName(product.PhotoPath);
    }

    public void SetCategory(int categoryId, string categoryName)
    {
        CategoryId   = categoryId;
        CategoryName = categoryName;
    }

    // ── Quantity Stepper ──────────────────────────────────────────────────────

    [RelayCommand]
    private void IncrementQuantity() => Quantity++;

    [RelayCommand]
    private void DecrementQuantity()
    {
        if (Quantity > 0) Quantity--;
    }

    // ── Photo Picker ──────────────────────────────────────────────────────────

    [RelayCommand]
    private async Task PickPhotoAsync()
    {
        var result = await MediaPicker.PickPhotoAsync();
        if (result is null) return;
        PhotoPath     = result.FullPath;
        PhotoFileName = result.FileName;
    }

    // ── Save (PUT to mockapi) ─────────────────────────────────────────────────

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(ProductName))
        {
            await Application.Current!.MainPage!.DisplayAlert(
                "Validation", "Product Name is required.", "OK");
            return;
        }

        var updated = new Product
        {
            ProductId   = _originalProductId,
            ProductName = ProductName,
            CategoryId  = CategoryId,
            SizeMl      = SizeMl,
            Price       = Price,
            Quantity    = Quantity,
            PhotoPath   = PhotoPath
        };

        // Fire callback so the caller receives the updated product (same behavior as Add)
        OnSaved?.Invoke(updated);

        // Pop back
        if (Application.Current?.MainPage is FlyoutPage flyout
            && flyout.Detail is NavigationPage nav)
            await nav.PopAsync();
    }

    // ── Navigation ────────────────────────────────────────────────────────────

    [RelayCommand]
    private async Task GoBackAsync()
    {
        if (Application.Current?.MainPage is FlyoutPage flyout
            && flyout.Detail is NavigationPage nav)
            await nav.PopAsync();
    }
}
