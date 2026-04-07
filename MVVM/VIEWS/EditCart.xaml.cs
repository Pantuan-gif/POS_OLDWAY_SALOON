using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

namespace POS_OLDWAY_SALOON.MVVM.VIEWS;


public partial class EditCart : ContentPage
{
	public EditCart()
	{
		InitializeComponent();
		BindingContext = new EditCartViewModel();
	}
}