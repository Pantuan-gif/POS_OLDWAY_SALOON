using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;
using POS_OLDWAY_SALOON.MVVM.MODELS;
namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class EditUserPage : ContentPage
{
	public EditUserPage(User user)
	{
		InitializeComponent();
		var vm = new EditUserViewModel();
		vm.User = user;
        BindingContext = vm;

    }
}