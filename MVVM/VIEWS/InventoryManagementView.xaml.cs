using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class InventoryManagementView : ContentPage
{
    InventoryManagementViewModel vm = new InventoryManagementViewModel();

    public InventoryManagementView()
    {
        InitializeComponent();
        BindingContext = vm;
    }

    // Called by AddCategoryViewModel after a new category is created
    public void ReceiveNewCategory(Category category)
        => vm.AddCategory();
}
