using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

namespace POS_OLDWAY_SALOON.MVVM.VIEWS;


public partial class InventoryReport : ContentPage
{
	public InventoryReport()
	{
		InitializeComponent();
		BindingContext = new InventoryReportViewModel();
	}
}