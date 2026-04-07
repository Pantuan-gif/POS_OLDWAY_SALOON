using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

namespace POS_OLDWAY_SALOON.MVVM.VIEWS;


public partial class Reports : ContentPage
{
	public Reports()
	{
		InitializeComponent();
		BindingContext = new ReportsViewModel();
	}
}