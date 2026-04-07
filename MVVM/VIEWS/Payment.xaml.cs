using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class Payment : ContentPage
{
	public Payment()
	{
		InitializeComponent();
		BindingContext = new PaymentViewModel();
	}
}