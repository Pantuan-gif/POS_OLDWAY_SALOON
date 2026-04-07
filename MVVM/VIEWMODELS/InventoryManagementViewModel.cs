using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.MVVM.VIEWS;
using POS_OLDWAY_SALOON.Services;
using System.Collections.ObjectModel;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class InventoryManagementViewModel : ObservableObject
{
    private readonly APISERVICES _api = new APISERVICES();

    [ObservableProperty]
    private string searchText = string.Empty;

    private List<Category> _allCategories = new();
    public ObservableCollection<Category> FilteredCategories { get; } = new();

    public InventoryManagementViewModel()
    {
        LoadCategories();
    }

    private async void LoadCategories()
    {
        var categories = await _api.GetAllCategoriesAsync();
        _allCategories = categories;
        FilteredCategories.Clear();
        foreach (var cat in categories)
            FilteredCategories.Add(cat);
    }

    partial void OnSearchTextChanged(string value)
    {
        var source = string.IsNullOrWhiteSpace(value)
            ? _allCategories
            : _allCategories.Where(c => c.CategoryName.Contains(value, StringComparison.OrdinalIgnoreCase)).ToList();

        FilteredCategories.Clear();
        foreach (var cat in source)
            FilteredCategories.Add(cat);
    }

    [RelayCommand]
    private async Task AddCategory()
    {
        var page = AppPages.NewAddCategoryView();
        page.OnCategoryAdded = (category) =>
        {
            LoadCategories();
        };
        await PushAsync(page);
    }

    [RelayCommand]
    private async Task ManageCategory(Category category)
    {
        var page = AppPages.NewProductManagementView();
        page.SetCategory(category);
        await PushAsync(page);
    }

    private static async Task PushAsync(Page page)
    {
        if (Application.Current?.MainPage is FlyoutPage flyout && flyout.Detail is NavigationPage nav)
            await nav.PushAsync(page);
    }
}