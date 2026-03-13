using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;
namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class Registration : ContentPage
{
	public Registration()
	{
		InitializeComponent();
		BindingContext = new RegistrationViewModels();
	}
}