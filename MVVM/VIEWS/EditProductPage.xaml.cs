using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class EditProductPage : ContentPage
{
    // Caller wires this up before pushing the page so the list can refresh
    public Action<Product>? OnProductSaved { get; set; }

    private readonly EditProductPageViewModel _vm;

    public EditProductPage()
    {
        InitializeComponent();
        _vm = new EditProductPageViewModel();
        BindingContext = _vm;

        // Forward the ViewModel save-callback to whoever pushed this page
        _vm.OnSaved = (product) => OnProductSaved?.Invoke(product);
    }

    /// <summary>Populate all fields from an existing product (required before pushing).</summary>
    public void LoadProduct(Product product) => _vm.LoadProduct(product);

    /// <summary>Set the category context (required before pushing).</summary>
    public void SetCategory(int categoryId, string categoryName)
        => _vm.SetCategory(categoryId, categoryName);
}
