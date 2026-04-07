namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class OrderingManagementView : ContentPage
{
    public OrderingManagementView()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is MVVM.VIEWMODELS.OrderingManagementViewModel vm)
        {
            // Ensure products are loaded when the page appears
            _ = vm.EnsureLoadedAsync();
        }
    }
}
