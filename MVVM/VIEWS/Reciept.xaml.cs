namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class Reciept : ContentPage
{
	public Reciept()
	{
		InitializeComponent();
	}

	private async void OnBackClicked(object sender, EventArgs e)
	{
		// After showing receipt, go back to products (replace detail with products view)
		if (Application.Current?.MainPage is FlyoutPage flyout)
		{
            // Return to ordering management so cashier can continue ordering
			flyout.Detail = new NavigationPage(AppPages.NewOrderingManagementView());
		}
	}
}