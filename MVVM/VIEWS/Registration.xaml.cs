using Microsoft.Maui.ApplicationModel.Communication;
using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;
namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class Registration : ContentPage
{
    private TaskCompletionSource<bool> _tcs = new();
    RegistrationViewModels vm = new RegistrationViewModels();
    public Task WaitForCloseAsync() => _tcs.Task;
    public Registration(string mode)
	{
		InitializeComponent();
		BindingContext = vm;
		Title = mode;
	}

	public async void Button_Clicked(object sender,EventArgs e)
	{
        Button btn = sender as Button;

        switch (btn.Text)
        {
            case "Register":
                // Validate All
                if (string.IsNullOrWhiteSpace(vm.FirstName) ||
                    string.IsNullOrWhiteSpace(vm.LastName) ||
                    string.IsNullOrWhiteSpace(vm.Email) ||
                    string.IsNullOrWhiteSpace(vm.Password) ||
                    string.IsNullOrWhiteSpace(vm.ConfirmPassword))
                {
                    await DisplayAlert("Error", "All fields required", "OK");
                    return;
                }
                // Validate First Name
                if (string.IsNullOrWhiteSpace(vm.FirstName))
                {
                    await DisplayAlert("Validation Error", "First Name is required.", "OK");
                    return;
                }

                // Validate Last Name
                if (string.IsNullOrWhiteSpace(vm.LastName))
                {
                    await DisplayAlert("Validation Error", "Last Name is required.", "OK");
                    return;
                }

                // Validate Email
                if (string.IsNullOrWhiteSpace(vm.Email) || !vm.Email.Contains("@"))
                {
                    await DisplayAlert("Validation Error", "A valid Email is required.", "OK");
                    return;
                }

                // Validate Password
                if (string.IsNullOrWhiteSpace(vm.Password))
                {
                    await DisplayAlert("Validation Error", "Password is required.", "OK");
                    return;
                }

                if (vm.Password.Length < 6)
                {
                    await DisplayAlert("Validation Error", "Password must be at least 6 characters.", "OK");
                    return;
                }

                // Confirm Password
                if (vm.Password != vm.ConfirmPassword)
                {
                    await DisplayAlert("Validation Error", "Passwords do not match.", "OK");
                    return;
                }

                // If all validations pass
                vm.Register();
                _tcs.TrySetResult(true);
                await Navigation.PopModalAsync();
                break;

            case "Cancel":

                _tcs.TrySetResult(true);

                await Navigation.PopModalAsync();
                break;
        }
    }

}