using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class AddUserCashier : ContentPage
{
    private TaskCompletionSource<bool> _tcs = new();
    public Task WaitForCloseAsync() => _tcs.Task;
    public AddUserCashier()
	{
		InitializeComponent();
        BindingContext = new AddCashierViewModel();
	}

	private async void OnCreateClicked(object sender, EventArgs e)
	{
		// Reuse the registration page to collect user data, but pre-fill role
		var reg = new Registration("Add User");
		if (reg.BindingContext is RegistrationViewModels rvm)
		{
			rvm.IsRoleSelectable = false;
			rvm.Role = "Cashier";
		}

		await Navigation.PushModalAsync(reg);
		await reg.WaitForCloseAsync();

		// After registration, persist users
		await MVVM.SERVICES.UserService.SaveAsync(LoginViewModels.User.ToList());

		// signal close
		_tcs.TrySetResult(true);
	}
}