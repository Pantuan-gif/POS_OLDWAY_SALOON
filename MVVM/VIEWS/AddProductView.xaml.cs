using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class AddProductView : ContentPage
{
    // Caller sets this before pushing the page
    public Action<Product>? OnProductSaved { get; set; }

    private readonly AddProductViewModel _vm;

    public AddProductView()
    {
        InitializeComponent();
        AddProductViewModel vm = new AddProductViewModel();
        _vm = vm;
        BindingContext = vm;

        // Wire the ViewModel's save callback to this view's callback
        _vm.OnSaved = (product) => OnProductSaved?.Invoke(product);
    }

    public void setCategory(int i, string s) => _vm.SetCategory(i, s);
    // Called by ProductManagementViewModel for edit mode
    public void LoadProduct(Product product) => _vm.LoadProduct(product);
}
