using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class InventoryManagementViewModel : ObservableObject
{
    private readonly APISERVICES _api = new();

    // ── Search ──────────────────────────────────────────────────────────────

    [ObservableProperty]
    private string _searchText = string.Empty;

    partial void OnSearchTextChanged(string value) => FilterCategories();

    // ── Loading / Error state ───────────────────────────────────────────────

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    // ── Categories ──────────────────────────────────────────────────────────

    private readonly ObservableCollection<Category> _allCategories = new();

    public ObservableCollection<Category> FilteredCategories { get; } = new();

    // ── Constructor ─────────────────────────────────────────────────────────

    public InventoryManagementViewModel()
    {
        // Fire-and-forget initial load; errors surfaced via ErrorMessage
        _ = LoadCategoriesAsync();
    }

    // ── API: Load ────────────────────────────────────────────────────────────

    [RelayCommand]
    private async Task LoadCategoriesAsync()
    {
        IsBusy = true;
        ErrorMessage = string.Empty;
        try
        {
            var categories = await _api.GetCategoriesAsync();
            // Get all products so we can compute stocks per category
            var products = await _api.GetProductsAsync();

            _allCategories.Clear();
            foreach (var c in categories)
            {
                // Sum product quantities for this category
                c.NoStocks = products
                    .Where(p => p.CategoryId == c.CategoryId)
                    .Sum(p => p.Quantity);

                // Only include categories that have stock
                if (c.NoStocks > 0)
                    _allCategories.Add(c);
            }
            FilterCategories();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load categories: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    // ── Filter Logic ─────────────────────────────────────────────────────────

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

    // ── Navigation helper ────────────────────────────────────────────────────

    private static async Task PushAsync(Page page)
    {
        if (Application.Current?.MainPage is FlyoutPage flyout
            && flyout.Detail is NavigationPage nav)
            await nav.PushAsync(page);
    }

    private static async Task ReplaceDetailAsync(Page page)
    {
        if (Application.Current?.MainPage is FlyoutPage flyout)
        {
            flyout.Detail = new NavigationPage(page);
            await Task.Yield();
        }
    }

    // ── Commands ─────────────────────────────────────────────────────────────

    [RelayCommand]
    public async Task AddCategory()
    {
        var page = AppPages.NewAddCategoryView();
        page.OnCategoryAdded = async (category) =>
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            try
            {
                var saved = await _api.AddCategoryAsync(category);
                if (saved is not null)
                {
                    _allCategories.Add(saved);
                    FilterCategories();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to add category: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
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

    [RelayCommand]
    private async Task GoBackToDashboard()
    {
        // Replace the Flyout Detail with the Dashboard view
        await ReplaceDetailAsync(AppPages.Dashboard);
    }

    [RelayCommand]
    private async Task DeleteCategory(Category category)
    {
        bool confirm = await Application.Current!.MainPage!.DisplayAlert(
            "Delete", $"Delete category \"{category.CategoryName}\"?", "Yes", "No");
        if (!confirm) return;

        IsBusy = true;
        ErrorMessage = string.Empty;
        try
        {
            bool ok = await _api.DeleteCategoryAsync(category.CategoryId);
            if (ok)
            {
                _allCategories.Remove(category);
                FilterCategories();
            }
            else
            {
                ErrorMessage = "Delete failed. Please try again.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to delete category: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task EditCategory(Category category)
    {
        var page = AppPages.NewAddCategoryView();
        //page.LoadCategory(category);                   // pre-fills the form
        page.OnCategoryAdded = async (updated) =>
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            try
            {
                var saved = await _api.UpdateCategoryAsync(updated);
                if (saved is not null)
                {
                    var index = _allCategories.IndexOf(category);
                    if (index >= 0) _allCategories[index] = saved;
                    FilterCategories();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to update category: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        };
        await PushAsync(page);
    }
}
