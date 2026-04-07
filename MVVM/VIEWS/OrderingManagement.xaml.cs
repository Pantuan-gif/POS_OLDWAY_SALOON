using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class OrderingManagement : ContentPage
{
	public OrderingManagement()
	{
		InitializeComponent();
		BindingContext = new OrderingManagementViewModel();
	}
}