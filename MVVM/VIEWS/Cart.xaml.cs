using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class Cart : ContentPage
{
	public Cart()
	{
		InitializeComponent();
		BindingContext = new CartViewModel();
	}
}