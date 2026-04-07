using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.Services;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class AddCategoryViewModel : ObservableObject
{
    private readonly APISERVICES _api = new APISERVICES();

    public Action<Category>? OnSaved { get; set; }

    [ObservableProperty] private int categoryId;
    [ObservableProperty] private string categoryName = string.Empty;
    [ObservableProperty] private int noStocks;
    [ObservableProperty] private string photoPath = string.Empty;
    [ObservableProperty] private string photoFileName = "No file chosen";

    [RelayCommand]
    private void IncrementStocks() => NoStocks++;

    [RelayCommand]
    private void DecrementStocks()
    {
        if (NoStocks > 0) NoStocks--;
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
        if (string.IsNullOrWhiteSpace(CategoryName))
        {
            await Application.Current!.MainPage!.DisplayAlert("Validation", "Category Name is required.", "OK");
            return;
        }

        var newCategory = new Category
        {
            CategoryName = CategoryName,
            NoStocks = NoStocks,
            PhotoPath = PhotoPath
        };

        bool success = await _api.AddCategoryAsync(newCategory);

        if (success)
        {
            OnSaved?.Invoke(newCategory);
            await GoBack();
        }
        else
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", "Failed to save category.", "OK");
        }
    }

    [RelayCommand]
    private async Task GoBack()
    {
        if (Application.Current?.MainPage is FlyoutPage flyout && flyout.Detail is NavigationPage nav)
            await nav.PopAsync();
    }
}