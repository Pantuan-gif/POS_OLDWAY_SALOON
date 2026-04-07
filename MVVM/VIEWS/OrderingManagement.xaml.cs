namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class OrderingManagement : ContentPage
{
	public OrderingManagement()
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

	private void OnAddToCartClicked(object sender, EventArgs e)
	{
        if (sender is Button btn)
		{
			var product = btn.BindingContext as MVVM.MODELS.Product
					   ?? ((btn.Parent as VisualElement)?.BindingContext as MVVM.MODELS.Product);
			if (product != null)
			{
				MVVM.SERVICES.CartService.AddToCart(product, 1);
			}
		}
	}
}