using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;


namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class OutofStock : ContentPage
{
	public OutofStock()
	{
		InitializeComponent();
		BindingContext = new OutofStockViewModel();
	}
}