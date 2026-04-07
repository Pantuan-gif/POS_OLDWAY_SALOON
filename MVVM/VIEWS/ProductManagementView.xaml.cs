using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class ProductManagementView : ContentPage
{
    ProductManagementViewModel vm = new ProductManagementViewModel();

    public ProductManagementView()
    {
        InitializeComponent();
        BindingContext = vm;
    }

    // Called by InventoryManagementViewModel before pushing this page
    public void SetCategory(Category category)
        => vm.SetCategory(category);

    // Called by AddProductViewModel after a product is saved
    public void ReceiveNewProduct(Product product)
        => vm.AddProduct();
}
