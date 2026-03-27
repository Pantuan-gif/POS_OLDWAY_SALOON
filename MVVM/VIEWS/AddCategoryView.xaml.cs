using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class AddCategoryView : ContentPage
{
    // Caller sets this before pushing the page
    public Action<Category>? OnCategoryAdded { get; set; }

    private readonly AddCategoryViewModel _vm;

    public AddCategoryView()
    {
        InitializeComponent();
        AddCategoryViewModel vm = new AddCategoryViewModel();
        _vm = vm;
        BindingContext = vm;

        // Wire the ViewModel's save callback to this view's callback
        _vm.OnSaved = (category) => OnCategoryAdded?.Invoke(category);
    }
}
