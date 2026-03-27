using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class AddCategoryViewModel : ObservableObject
{
    // Set by AddCategoryView after instantiation
    public Action<Category>? OnSaved { get; set; }
    // ── Fields ───────────────────────────────────────────────────────────────

    [ObservableProperty]
    private int _categoryId;

    [ObservableProperty]
    private string _categoryName = string.Empty;

    [ObservableProperty]
    private int _noStocks;

    [ObservableProperty]
    private string _photoPath = string.Empty;

    [ObservableProperty]
    private string _photoFileName = "No file chosen";

    // ── Stepper Commands ────────────────────────────────────────────────────

    [RelayCommand]
    private void IncrementStocks() => NoStocks++;

    [RelayCommand]
    private void DecrementStocks()
    {
        if (NoStocks > 0) NoStocks--;
    }

    // ── Photo Picker ────────────────────────────────────────────────────────

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
        if (string.IsNullOrWhiteSpace(CategoryName))
        {
            await Application.Current!.MainPage!.DisplayAlert("Validation", "Category Name is required.", "OK");
            return;
        }

        var newCategory = new Category
        {
            CategoryId   = CategoryId,
            CategoryName = CategoryName,
            NoStocks     = NoStocks,
            PhotoPath    = PhotoPath
        };

        // Fire callback so the caller (InventoryManagementView) receives the result
        OnSaved?.Invoke(newCategory);

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
