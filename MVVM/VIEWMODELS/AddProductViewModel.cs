using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.Services;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class AddProductViewModel : ObservableObject
{
    private readonly APISERVICES _api = new APISERVICES();

    public Action<Product>? OnSaved { get; set; }

    [ObservableProperty] private int productId;
    [ObservableProperty] private int categoryId;
    [ObservableProperty] private string categoryName = string.Empty;
    [ObservableProperty] private bool isEdit;

    [ObservableProperty] private string productName = string.Empty;
    [ObservableProperty] private double sizeMl;
    [ObservableProperty] private decimal price;
    [ObservableProperty] private int quantity;
    [ObservableProperty] private string photoPath = string.Empty;
    [ObservableProperty] private string photoFileName = "No file chosen";

    public void LoadProduct(Product product)
    {
        ProductId = product.ProductId;
        CategoryId = product.CategoryId;
        CategoryName = product.CategoryName ?? "";
        ProductName = product.ProductName;
        SizeMl = product.SizeMl;
        Price = product.Price;
        Quantity = product.Quantity;
        PhotoPath = product.PhotoPath;
        PhotoFileName = string.IsNullOrEmpty(product.PhotoPath) ? "No file chosen" : Path.GetFileName(product.PhotoPath);
        IsEdit = true;
    }

    [RelayCommand]
    private void IncrementQuantity() => Quantity++;

    [RelayCommand]
    private void DecrementQuantity()
    {
        if (Quantity > 0) Quantity--;
    }

    [RelayCommand]
    private async Task PickPhotoAsync()
    {
        var result = await MediaPicker.PickPhotoAsync();
        if (result is null) return;
        PhotoPath = result.FullPath;
        PhotoFileName = result.FileName;
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
            ProductId = ProductId,
            ProductName = ProductName,
            CategoryId = CategoryId,
            SizeMl = SizeMl,
            Price = Price,
            Quantity = Quantity,
            PhotoPath = PhotoPath
        };

        bool success = IsEdit 
            ? await _api.UpdateProduct(product) 
            : await _api.AddProductAsync(product);

        if (success)
        {
            OnSaved?.Invoke(product);
            await GoBack();
        }
        else
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", "Failed to save product.", "OK");
        }
    }

    [RelayCommand]
    private async Task GoBack()
    {
        if (Application.Current?.MainPage is FlyoutPage flyout && flyout.Detail is NavigationPage nav)
            await nav.PopAsync();
    }
}