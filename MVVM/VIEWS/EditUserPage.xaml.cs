using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;
namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class EditUserPage : ContentPage
{
	public EditUserPage(int user)
	{
		InitializeComponent();
        BindingContext = new EditUserViewModel();

    }
}