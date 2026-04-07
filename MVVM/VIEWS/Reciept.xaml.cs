using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;


namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class Reciept : ContentPage
{
	public Reciept()
	{
		InitializeComponent();
		BindingContext = new RecieptViewModel();
	}
}