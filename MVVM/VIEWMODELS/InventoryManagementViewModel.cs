using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using System.Collections.ObjectModel;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class InventoryManagementViewModel : ObservableObject
{
    // ── Search ──────────────────────────────────────────────────────────────

    [ObservableProperty]
    private string _searchText = string.Empty;

    partial void OnSearchTextChanged(string value) => FilterCategories();

    // ── Categories ──────────────────────────────────────────────────────────

    private readonly ObservableCollection<Category> _allCategories = new()
    {
        new Category { CategoryId = 1, CategoryName = "Whisky",  NoStocks = 240, PhotoPath = "whisky.png"  },
        new Category { CategoryId = 2, CategoryName = "Rum",     NoStocks = 892, PhotoPath = "rum.png"     },
        new Category { CategoryId = 3, CategoryName = "Vodka",   NoStocks = 892, PhotoPath = "vodka.png"   },
        new Category { CategoryId = 4, CategoryName = "Tequila", NoStocks = 892, PhotoPath = "tequila.png" },
        new Category { CategoryId = 5, CategoryName = "Brandy",  NoStocks = 892, PhotoPath = "brandy.png"  },
        new Category { CategoryId = 6, CategoryName = "Beer",    NoStocks = 892, PhotoPath = "beer.png"    },
    };

    public ObservableCollection<Category> FilteredCategories { get; } = new();

    // ── Constructor ─────────────────────────────────────────────────────────

    public InventoryManagementViewModel()
    {
        FilterCategories();
    }

    // ── Filter Logic ────────────────────────────────────────────────────────

    private void FilterCategories()
    {
        FilteredCategories.Clear();
        var query = string.IsNullOrWhiteSpace(SearchText)
            ? _allCategories
            : _allCategories.Where(c =>
                c.CategoryName.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

        foreach (var cat in query)
            FilteredCategories.Add(cat);
    }

    // ── Navigation helper ───────────────────────────────────────────────────

    private static async Task PushAsync(Page page)
    {
        if (Application.Current?.MainPage is FlyoutPage flyout
            && flyout.Detail is NavigationPage nav)
            await nav.PushAsync(page);
    }

    // ── Commands ────────────────────────────────────────────────────────────

    [RelayCommand]
    private async Task AddCategory()
    {
        // Fresh form page every time
        var page = AppPages.NewAddCategoryView();
        page.OnCategoryAdded = (category) =>
        {
            AddCategory(category);
        };
        await PushAsync(page);
    }

    [RelayCommand]
    private async Task ManageCategory(Category category)
    {
        // Fresh product page, pass in the selected category
        var page = AppPages.NewProductManagementView();
        page.SetCategory(category);
        await PushAsync(page);
    }

    public void AddCategory(Category category)
    {
        _allCategories.Add(category);
        FilterCategories();
    }
}
