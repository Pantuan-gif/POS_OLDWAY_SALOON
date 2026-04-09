namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class Payment : ContentPage
{
	public Payment()
	{
		InitializeComponent();
	}

	private void OnPaymentMethodChanged(object sender, CheckedChangedEventArgs e)
	{
		if (BindingContext is MVVM.VIEWMODELS.PaymentViewModel vm)
		{
			if (Cash.IsChecked) vm.PaymentMethod = "Cash";
			else if (GCash.IsChecked) vm.PaymentMethod = "GCash";
			else if (PayMaya.IsChecked) vm.PaymentMethod = "PayMaya";
		}
	}
}