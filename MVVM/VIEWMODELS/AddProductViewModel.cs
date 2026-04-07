using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class AddProductViewModel : ObservableObject
{
    // Set by AddProductView after instantiation
    public Action<Product>? OnSaved { get; set; }
    // ── Fields ───────────────────────────────────────────────────────────────

    [ObservableProperty]
    private int _productId;

    [ObservableProperty]
    private int _categoryId;

    [ObservableProperty]
    private string _categoryName = string.Empty;

    [ObservableProperty]
    private bool _isEdit;

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

    // ── Load existing product (edit mode) ───────────────────────────────────

    public void LoadProduct(Product product)
    {
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
        IsEdit = true;
    }

    // ── Stepper Commands ────────────────────────────────────────────────────

    [RelayCommand]
    private void IncrementQuantity() => Quantity++;

    [RelayCommand]
    private void DecrementQuantity()
    {
        if (Quantity > 0) Quantity--;
    }

    // ── Photo Picker ────────────────────────────────────────────────────────

    [RelayCommand]
    private async Task PickPhotoAsync()
    {
        var result = await MediaPicker.PickPhotoAsync();
        if (result is null) return;
        PhotoPath     = result.FullPath;
        PhotoFileName = result.FileName;
    }
    public void SetCategory(int cId,string cN)
    {
        CategoryId = cId;
        CategoryName = cN;
    }
    [RelayCommand]
    private async Task Add()
    {
        if (string.IsNullOrWhiteSpace(ProductName))
        {
            await Application.Current!.MainPage!.DisplayAlert("Validation", "Product Name is required.", "OK");
            return;
        }

        var product = new Product
        {
            ProductId    = ProductId,
            ProductName  = ProductName,
            CategoryId = CategoryId,
            SizeMl       = SizeMl,
            Price        = Price,
            Quantity     = Quantity,
            PhotoPath    = PhotoPath
        };

        // Fire callback so the caller (ProductManagementView) receives the result
        OnSaved?.Invoke(product);

        // Pop back
        if (Application.Current?.MainPage is FlyoutPage flyout
            && flyout.Detail is NavigationPage nav)
            await nav.PopAsync();
    }

    [RelayCommand]
    private async Task GoBack()
    {
        if (Application.Current?.MainPage is FlyoutPage flyout
            && flyout.Detail is NavigationPage nav)
            await nav.PopAsync();
    }
}
