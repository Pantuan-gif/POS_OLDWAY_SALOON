using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;
namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class Registration : ContentPage
{
	public Registration(string mode)
	{
		InitializeComponent();
		BindingContext = new RegistrationViewModels();
		Title = mode;
	}
}