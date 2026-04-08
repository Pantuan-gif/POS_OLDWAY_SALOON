namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class TransactionReports : ContentPage
{
	public TransactionReports()
	{
		InitializeComponent();
		BindingContext = new MVVM.VIEWMODELS.TransactionReportsViewModel();
	}
}